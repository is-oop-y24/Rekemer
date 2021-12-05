using System.Collections.Generic;
using System.IO;

namespace BackupsExtra.Algorithms.RestoreAlgorithms
{
    public class RestoreInOriginalLocation : Restore
    {
        public override void RestoreFiles(RestorePoint restorePoint)
        {
            List<string> originalLocation = restorePoint.OriginalFiles;
            ExtractZip(restorePoint.Files);
            var files = GetFilesOfZip(restorePoint.Files);
            DeleteFiles(restorePoint.Files);
            foreach (var newLocation in files)
            {
                foreach (var oldLocation in originalLocation)
                {
                    if (Path.GetFileName(oldLocation) == Path.GetFileName(newLocation))
                    {
                        if (File.Exists(oldLocation))
                        {
                            File.Delete(oldLocation);
                        }

                        File.Move(newLocation, oldLocation);
                        restorePoint.Files.Remove(newLocation);
                    }
                }
            }
        }
    }
}