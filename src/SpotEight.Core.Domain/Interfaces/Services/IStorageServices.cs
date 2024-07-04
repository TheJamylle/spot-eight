using System.Net;

namespace SpotEight.Core.Domain.Interfaces.Services;

public interface IStorageServices
{
    Task UploadFileAsync(MemoryStream stream, string buketName, string fileName);
    void UploadFile(MemoryStream stream, string buketName, string fileName);
    string GetPreSignedURL(string fileName);
    bool IsValidFile(string fileName, string contentFile);
    Task<HttpStatusCode> UploadObject(string fileName, string path, string contentFile);
    string GetFullPath(string fileName, string path);
    string CreateLink(string fileNameOrFolderAndFileName);
}
