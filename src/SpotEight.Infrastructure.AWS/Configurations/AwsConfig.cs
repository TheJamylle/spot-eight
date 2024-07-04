namespace SpotEight.Infrastructure.AWS.Configurations;

public class AwsConfig
{
    public string? AccessKey { get; set; } = string.Empty;
    public string? SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string RegionEndpoint { get; set; } = string.Empty;
    public int Expires { get; set; } 
}
