using System.Collections.Generic;
using System.IO;

namespace BackupsExtra
{
    public interface IRepository
    {
        string AddFilesToArchive(List<string> files);
        string CreateZipCopyOfFile(string file);
    }
}