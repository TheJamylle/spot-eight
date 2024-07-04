namespace SpotEight.Core.Domain.Dtos.Response;

public class ResponseBase<T> where T : class
{
    public T? Data { get; set; }
    public bool HasError { get; set; } = false;
    public string? Error { get; set; }
}
