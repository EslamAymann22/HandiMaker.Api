using Microsoft.AspNetCore.Http;

namespace HandiMaker.Services.Services.HelperStatic
{
    public static class DocumentServices
    {

        public static string UploadFile(IFormFile File, string FolderName, IHttpContextAccessor _httpContextAccessor)
        {

            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);

            string FileName = $"{Guid.NewGuid()}{File.FileName.Replace(" ", "_")}";

            string FilePath = Path.Combine(FolderPath, FileName);
            Directory.CreateDirectory(FolderPath);

            using var FS = new FileStream(FilePath, FileMode.Create);
            File.CopyTo(FS);

            var Request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var FileUrl = $"{baseUrl}/Files/{FolderName}/{FileName}";


            return FileUrl;
        }

        public static void DeleteFile(string FileName, string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, FileName);

            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
