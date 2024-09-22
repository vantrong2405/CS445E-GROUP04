using Microsoft.AspNetCore.Http;
using System.Text;

namespace BooksStore.Web.Utilities
{
    public class FileExtensions
    {
        public static IFormFile ConvertStringToFormFile(string content, string fileName)
        {
            // Convert string to byte array
            byte[] byteArray = Encoding.UTF8.GetBytes(content);

            // Create a MemoryStream using the byte array
            using (var stream = new MemoryStream(byteArray))
            {
                // Create FormFile instance
                IFormFile formFile = new FormFile(stream, 0, stream.Length, "name", fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/plain"
                };

                return formFile;
            }
        }
    }
}
