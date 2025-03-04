using System;
using System.Text.Json.Serialization;
using static System.DateTime;

namespace DayPay;

public class BaseAddRequest
{
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = UtcNow;

    [JsonIgnore]
    public DateTime ModifiedAt { get; set; } = UtcNow;
}
