using DayPay.Dtos.ESDto;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayPay.Services.Implements;

public class ESService<T> : IESService<T> where T : DayPayESDto
{
    private readonly ILogger<ESService<T>> _logger;
    private readonly string _indexName;
    private readonly ESOptions _option;
    private readonly ElasticsearchClient _client;

    public ESService(ILogger<ESService<T>> logger, IOptions<ESOptions> options)
    {
        _logger = logger;
        _option = options.Value;

        var settings = new ElasticsearchClientSettings(new Uri(_option.Url))
            //.Authentication()
            .DefaultIndex(_option.DefaultIndex);

        _client = new ElasticsearchClient(settings);
        _indexName = _option.Indexes[typeof(T).Name];
    }
    public async Task CreateIndexIfNotExists()
    {
        if (!_client.Indices.Exists(_indexName).Exists)
        {
            _ = await _client.Indices.CreateAsync(_indexName);
        }
    }

    public async Task<bool> AddOrUpdate(T document)
    {
        var response = await _client.IndexAsync(document, _indexName, document.Id);

        return response.IsValidResponse;
    }

    public async Task<bool> AddOrUpdateBulk(IEnumerable<T> documents)
    {
        var indexResponse = await _client.BulkAsync(idx => idx
               .Index(_indexName)
               .UpdateMany(documents, (ud, d) => ud.Doc(d).DocAsUpsert(true))
           );
        return indexResponse.IsValidResponse;
    }

    public async Task<T> Get(string key)
    {
        var res = await _client.GetAsync<T>(key, x => x.Index(_indexName));

        return res.IsValidResponse ? res.Source : default;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        var res = await _client.SearchAsync<T>(x => x.Index(_indexName));

        return res.IsValidResponse ? res.Documents.ToArray() : default;
    }

    public async Task<List<T>> Query(Query predicate)
    {
        var searchResponse = await _client.SearchAsync<T>(x => x.Index(_indexName).Query(predicate));

        return searchResponse.IsValidResponse ? searchResponse.Documents.ToList() : default;
    }

    public async Task<bool> Remove(string key)
    {
        var response = await _client.DeleteAsync<T>(key, d => d.Index(_indexName));

        return response.IsValidResponse;
    }

    public async Task<bool> RemoveAll()
    {
        var response = await _client.DeleteByQueryAsync<T>(d => d.Indices(_indexName).Query(q => q.QueryString(p => p.Query("*"))));

        return response.IsValidResponse;
    }
}
