using Microsoft.AspNetCore.Http;


namespace TalanLunch.Application.Interfaces

{

   public interface IBlobStorageService
   {
        //Task<string> UploadFileAsync(IFormFile file);
       
       Task<string> UploadFileAsync(IFormFile file, string containerName);
        

    }



}
