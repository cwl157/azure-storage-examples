using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.App.FileAccess
{
    public interface IFileAccess
    {
        public Task Write(string filePath, string data);

        public Task<string> Read(string filePath);
    }
}
