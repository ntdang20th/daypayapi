using DayPay.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using System;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Http.Client;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using static Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;
using static System.StringSplitOptions;

namespace DayPay.Host;

[DependsOn(
    typeof(DayPayHttpApiModule),
    typeof(DayPayApplicationModule),
    typeof(DayPayEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpHttpClientModule)
)]
public class DayPayHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        Configure<AbpDbContextOptions>(o => o.UseSqlServer());
        ConfigureAuthentication(context, configuration);
        ConfigureLocalization();
        //ConfigureCors(context, configuration);
        _ = context.Services.AddCors(o => o.AddPolicy("Default", b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        ConfigureSwaggerServices(context, configuration);

        ConfigureOpenTelemetry(context, configuration);
    }

    private static void ConfigureOpenTelemetry(ServiceConfigurationContext context, IConfiguration configuration)
    {
        // Note: Switch between Zipkin/OTLP/Console by setting UseTracingExporter in appsettings.json.
        var tracingExporter = configuration.GetValue("Logging:Logging:OpenTelemetry:UseTracingExporter", defaultValue: "console")!.ToLowerInvariant();

        // Note: Switch between Prometheus/OTLP/Console by setting UseMetricsExporter in appsettings.json.
        var metricsExporter = configuration.GetValue("Logging:OpenTelemetry:UseMetricsExporter", defaultValue: "console")!.ToLowerInvariant();

        // Note: Switch between Console/OTLP by setting UseLogExporter in appsettings.json.
        var logExporter = configuration.GetValue("Logging:OpenTelemetry:UseLogExporter", defaultValue: "console")!.ToLowerInvariant();

        // Note: Switch between Explicit/Exponential by setting HistogramAggregation in appsettings.json
        var histogramAggregation = configuration.GetValue("Logging:OpenTelemetry:HistogramAggregation", defaultValue: "explicit")!.ToLowerInvariant();

        _ = context.Services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(
                serviceName: configuration.GetValue("Logging:OpenTelemetry:ServiceName", defaultValue: "DayPay_Log"),
                serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknow",
                serviceInstanceId: Environment.MachineName))
            .WithLogging(l =>
            {
                switch (logExporter)
                {
                    case "otlp":
                        l.AddOtlpExporter(option => option.Endpoint = new Uri(configuration.GetValue("Logging:OpenTelemetry:Otlp:Endpoint", defaultValue: "http://localhost:4317")!));
                        break;
                    default:
                        l.AddConsoleExporter();
                        break;
                }
            });

        _ = context.Services.AddLogging(x => x.AddOpenTelemetry(d => d.AddOtlpExporter()));
    }

    private static void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        => _ = context.Services.AddAuthentication(AuthenticationScheme).AddJwtBearer(o =>
        {
            o.Authority = configuration["AuthServer:Authority"];
            o.Audience = configuration["AuthServer:ApiName"];
        });

    private void ConfigureLocalization() => Configure<AbpLocalizationOptions>(o =>
    {
        o.Languages.Add(new LanguageInfo("en", "en", "English"));
        o.Languages.Add(new LanguageInfo("vi", "vi", "Tiếng Việt"));
    });

    private static void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        => context.Services.AddCors(s => s.AddDefaultPolicy(b => b
                                .WithOrigins(configuration["App:CorsOrigins"]
                                    .Split(",", RemoveEmptyEntries)
                                    .Select(o => o
                                        .RemovePostFix("/")).ToArray())
                                        .WithAbpExposedHeaders()
                                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials()));

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        _ = context.Services.AddAbpSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"PayDay API - {hostingEnvironment.EnvironmentName}",
                Version = "1.0"
            });

            o.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
            o.HideAbpEndpoints();
            //o.EnableAnnotations();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        _ = context.GetEnvironment().IsDevelopment() ? app.UseDeveloperExceptionPage() : app.UseHsts();
        _ = app.UseHttpsRedirection();
        _ = app.UseCorrelationId();
        _ = app.UseStaticFiles();
        _ = app.UseRouting();

        _ = app.UseAbpRequestLocalization(o =>
        {
            _ = o.SetDefaultCulture("vi");
            _ = o.AddSupportedCultures("vi");
            _ = o.AddSupportedUICultures("vi");
        });

        _ = app.UseCors();
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
        _ = app.UseSwagger();

        _ = app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "PayDay API");
            c.OAuthClientId(context.ServiceProvider.GetRequiredService<IConfiguration>()["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("PayDay");
            c.DefaultModelsExpandDepth(-1);
        });

        _ = app.UseUnitOfWork();
        _ = app.UseAuditing();
        _ = app.UseAbpSerilogEnrichers();

        _ = app.UseConfiguredEndpoints();
    }
}
