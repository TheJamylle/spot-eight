using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using SpotEight.Core.Domain.Interfaces.Services;
using SpotEight.Infrastructure.AWS.Configurations;

namespace SpotEight.Infrastructure.AWS.Services;

public class S3Services : IStorageServices
{
    private readonly AwsConfig _config;
    private readonly AmazonS3Client _amazonS3Client;

    public S3Services(IOptions<AwsConfig> config)
    {
        _config = config.Value;
        var credentials = new BasicAWSCredentials(_config.AccessKey, _config.SecretKey);
        _amazonS3Client = new AmazonS3Client(credentials, new AmazonS3Config { RegionEndpoint = RegionEndpoint.GetBySystemName(_config.RegionEndpoint) });
    }

    public void UploadFile(MemoryStream stream, string buketName, string fileName)
    {
        try
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = buketName,
                CannedACL = S3CannedACL.NoACL
            };

            var transferUtility = new TransferUtility(_amazonS3Client);

            transferUtility.Upload(uploadRequest);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao subir arquivo: " + ex.Message, ex);
        }
    }

    public async Task UploadFileAsync(MemoryStream stream, string buketName, string fileName)
    {
        try
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = buketName,
                CannedACL = S3CannedACL.NoACL
            };

            var transferUtility = new TransferUtility(_amazonS3Client);

            await transferUtility.UploadAsync(uploadRequest);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao subir arquivo: " + ex.Message, ex);
        }
    }

    public string GetPreSignedURL(string fileName)
    {
        string? urlString;

        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _config.BucketName,
                Key = fileName,
                Expires = DateTime.UtcNow.AddHours(_config.Expires)
            };

            urlString = _amazonS3Client.GetPreSignedURL(request);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao gerar url: " + ex.Message, ex);
        }

        return urlString;
    }

    public string CreateLink(string fileNameOrFolderAndFileName)
    {
        //var link = $"https://{_config.BucketName}.s3.us-east-1.amazonaws.com/{fileNameOrFolderAndFileName}";
        var link = $"https://{_config.BucketName}.{_amazonS3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/{fileNameOrFolderAndFileName}";
        return link;
    }

    public bool IsValidFile(string fileName, string contentFile)
    {
        var file = Base64ToFile(fileName, contentFile);

        if (file.Length <= 0 || file.Length > 99999999)
            return false;

        var permittedExtensions = new[] { ".txt", ".csv", ".xls", ".xlsx" };

        var ext = Path.GetExtension(file.FileName);

        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            return false;

        return true;
    }

    public async Task<HttpStatusCode> UploadObject(string fileName, string path, string contentFile)
    {
        PutObjectResponse response;

        var client = new AmazonS3Client(_config.AccessKey, _config.SecretKey, RegionEndpoint.USEast1);

        var file = Base64ToFile(fileName, contentFile);

        byte[] fileBytes = new byte[file.Length];

        file.OpenReadStream().Read(fileBytes, 0, int.Parse(file.Length.ToString()));

        using (var stream = new MemoryStream(fileBytes))
        {
            var request = new PutObjectRequest
            {
                BucketName = _config.BucketName,
                Key = path,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead
            };

            response = await client.PutObjectAsync(request);
        };

        return response.HttpStatusCode;
    }

    public string GetFullPath(string fileName, string path)
    {
        var timeSpan = DateTime.Now.TimeOfDay.ToString().Replace(".", "").Replace(":", "");

        var fullName = string.Concat(path, "/", timeSpan, "-", fileName);

        return fullName;
    }

    private static IFormFile Base64ToFile(string fileName, string contentFile)
    {
        byte[] bytes = Convert.FromBase64String(contentFile);

        var stream = new MemoryStream(bytes);

        IFormFile file = new FormFile(stream, 0, bytes.Length, fileName, fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = GetMimeType(fileName).MimeType
        };

        return file;
    }

    private static ContentType GetMimeType(string fileName)
    {
        var mimeType = MimeTypes.GetMimeType(fileName);

        return ContentType.Parse(mimeType);
    }
}
