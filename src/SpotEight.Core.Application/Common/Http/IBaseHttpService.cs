using RestSharp;

namespace SpotEight.Core.Application.Common.Http;

public interface IBaseHttpService<T> where T : class
{
    Task<T> SendAsync(string url);
    Task<T> SendAsync(string url, Method method, object? body = null);
    Task<T> PatchAsync(string url, object body);
    Task<T> PostJsonAsync<TValue>(string url, TValue value);
}
