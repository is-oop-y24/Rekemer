using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using BackupsExtra.Algorithms.LogInterface;
using BackupsExtra.Algorithms.SaveAlgorithms;

namespace BackupsExtra.Algorithms.DeleteAlgorithms
{
    public class PointsMerger
    {
        public bool IsMerge { get; set; }
        protected void ProccessOldPoints(ref List<RestorePoint> restorePoints, List<RestorePoint> restorePointsToDelete)
        {
            if (IsMerge && restorePointsToDelete.Count != 0)
            {
                
                foreach (var oldRestorePoint in restorePointsToDelete)
                {
                    // if restore old point has object but new doesnt not have it, update new points with this object
                    var newPoint = restorePoints[^1];
                    var files = newPoint.Files;
                    
                    List<string> filesOfNewPoints = GetFilesOfZip(files);
                    var filesOfOldPoint = GetFilesOfZip(oldRestorePoint.Files);
                    
                    var difference = filesOfOldPoint.Except(filesOfNewPoints).ToList();
                    if (difference.Count > 0)
                    {
                        ExtractZip(files);
                        ExtractZip(oldRestorePoint.Files);
                        DeleteFiles(files);
                        DeleteFiles(oldRestorePoint.Files);
                        filesOfNewPoints = filesOfNewPoints.Union(difference).ToList();
                        
                        newPoint.Files = newPoint.Files.Union(filesOfNewPoints).ToList();
                        
                        if (files.Count == 1)
                        {
                            foreach (var file in files)
                            {
                                newPoint.Files.Remove(file);
                            }
                            var singleStorageSave = new SingleStorageSave();
                            singleStorageSave.Operation(newPoint.Files,
                                new Repository(Path.GetDirectoryName(files[0])));
                            DeleteFiles(newPoint.Files);
                        }
                        else
                        {
                            // archive every single one
                            var splitStorageSave = new SplitStorageSave();
                            splitStorageSave.Operation(newPoint.Files,
                                new Repository(Path.GetDirectoryName(filesOfNewPoints[0])));
                        }
                        var message = MergeIsDone(newPoint);
                        Log.Instance.Log(message);
                    }
                    // if new points has singlestorage delete old or  if old and new is the same delete old
                    if (newPoint.Alghoritm == new SingleStorageSave().NameOfAlgorithm() || difference.Count == 0)
                    {
                        restorePoints.Remove(oldRestorePoint);
                        DeleteFiles(oldRestorePoint.Files);
                        var message = RestorePointIsDeleted(oldRestorePoint);
                        Log.Instance.Log(message);
                    }
                    
                }
            }
            else
            {
                if (restorePointsToDelete.Count != 0)
                {
                    if (restorePoints.Count- 1 == restorePointsToDelete.Count   )
                    {
                        throw new Exception("you cannot delete all points, change parameters");
                    }
                    else
                    {
                        restorePoints = restorePoints.Except(restorePointsToDelete).ToList();
                        var message = RestorePointIsDeleted(restorePointsToDelete);
                        Log.Instance.Log(message);
                    }
                }
              
            }
        }

        private void DeleteFiles(List<string> files)
        {
            foreach (var file in  files)
            {
                if (File.Exists(file))
                {
                    File.Delete( file);
                }
               
            }
        }
        private string RestorePointIsDeleted(List<RestorePoint> restorePoints)
        {
            var message = "";
            foreach (var point in restorePoints)
            {
                message += RestorePointIsDeleted(point);
            }

            return message;
        }
        private string RestorePointIsDeleted(RestorePoint restorePoint)
        {
            string message = "RestorePoint is deleted" + Environment.NewLine;
            var time = restorePoint._time;
            var alghoritm = restorePoint.Alghoritm;
            var files = restorePoint.Files;
            message += "Time: " + time + " algorithm: " + alghoritm + "files: " + files + Environment.NewLine;
            return message;
        }

        private string MergeIsDone(RestorePoint restorePoint)
        {
            string message = "RestorePoint is merged" + Environment.NewLine;
            var time = restorePoint._time;
            var alghoritm = restorePoint.Alghoritm;
            var files = restorePoint.Files;
            message += "Time: " + time + " algorithm: " + alghoritm + "files: " + files;
            return message;
        }

        private void ExtractZip(List<string> files)
        {
            foreach (var file in files)
            {
                try
                {
                    ZipFile.ExtractToDirectory(file, Path.GetDirectoryName(file));
                }
                catch
                {

                }
            }
        }
        private  List<string>  GetFilesOfZip(List<string> files)
        {
            List<string> filesOfNewPoints = new List<string>();
            foreach (var file in files)
            {
                using (ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read))
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        filesOfNewPoints.Add(Path.Combine(Path.GetDirectoryName(file), entry.Name));
                    }
            }
            return filesOfNewPoints;
        }
    }
    
}