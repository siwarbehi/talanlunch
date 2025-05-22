using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TalanLunch.Application.Interfaces;

public class BlobStorageService : IBlobStorageService
{
    private readonly string _connectionString;

    public BlobStorageService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureBlobStorage:ConnectionString"]!;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string containerName)
    {
        var blobContainerClient = new BlobContainerClient(_connectionString, containerName);
        await blobContainerClient.CreateIfNotExistsAsync();

        string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var blobClient = blobContainerClient.GetBlobClient(uniqueFileName);

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString(); // Renvoie l’URL publique de l’image
    }
}
