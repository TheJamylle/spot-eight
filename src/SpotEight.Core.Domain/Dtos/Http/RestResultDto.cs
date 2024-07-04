using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SpotEight.Core.Domain.Dtos.Http;

public class RestResultDto
{

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("data")]
    public List<object> Data { get; set; }

    [JsonProperty("hasError", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("hasError")]
    public bool? HasError { get; set; }

    [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;
}
