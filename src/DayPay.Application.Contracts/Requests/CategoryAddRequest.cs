namespace DayPay.Requests;

public sealed class CategoryAddRequest : BaseAddRequest
{
    public required string Name { get; set; }

    public string Code { get; set; }
}
