using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.App.FileAccess
{
    public class LocalFileAccess : IFileAccess
    {
        public async Task<string> Read(string filePath)
        {
            return await System.IO.File.ReadAllTextAsync(filePath);
        }

        public async Task Write(string filePath, string data)
        {
            await System.IO.File.WriteAllTextAsync(filePath, data);
        }
    }
}
