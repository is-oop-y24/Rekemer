using System.Collections.Generic;
using System.IO;

namespace BackupsExtra.Algorithms.RestoreAlgorithms
{
    public class RestoreInCustomLocation : Restore
    {
        private string _customLocation;

        public RestoreInCustomLocation(string customLocation)
        {
            _customLocation = customLocation;
        }

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
                        string name = Path.GetFileName(newLocation);
                        if (File.Exists(Path.Combine(_customLocation, name)))
                        {
                            File.Delete(Path.Combine(_customLocation, name));
                        }

                        File.Move(newLocation, Path.Combine(_customLocation, name));
                        restorePoint.Files.Remove(newLocation);
                    }
                }
            }
        }
    }
}