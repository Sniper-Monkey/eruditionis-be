using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace eruditionis
{
    public class FileSystemService
    {
        public const string IMAGE_PATH = "wwwroot/Docs/";
        public async Task<string> Create(string document, int docId)
        {

            string webRootPath = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                webRootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            }

            string response = "";
            string format = document.Substring(0, 5);

            if (format.Contains("JVBER"))
                format = ".pdf";
            else if (format.Contains("UEsDB"))
                format = ".docx";
            else
                throw new Exception("Wrong picture format. Acceptable file types are PDF and DOCX");


            var fileName = Guid.NewGuid();
            var imagePath = Path.Combine(webRootPath + IMAGE_PATH, docId.ToString());
            DirectoryInfo dInfo = new DirectoryInfo(imagePath);
            if (!dInfo.Exists)
            {
                dInfo.Create();
            }

            string base64 = document.Substring(document.IndexOf(',') + 1);
            byte[] data = Convert.FromBase64String(base64);

            using (var stream = System.IO.File.Create(Path.Combine(imagePath, fileName.ToString() + format)))
            {
                await stream.WriteAsync(data);
                stream.Close();
            }

            response = "Docs/" + docId + "/" + fileName + format;
            return response;
        }
    }
}
