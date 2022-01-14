using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public interface IAlgorithm
    {
        List<FileInfo> Operation(List<FileInfo> files,  IRepository repository);
    }
}