using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NUnit.Framework;

namespace Backups.Tests
{
    public class BackupTests
    {
        
        private string directoryWithFiles;
        private string repositoryDirectory;
        private string curPath = Directory.GetCurrentDirectory();

        [SetUp]
        public void Setup()
        {
            directoryWithFiles = Path.GetFullPath(Path.Combine(curPath, @"../../../../Tests/texts"));
            repositoryDirectory = Path.GetFullPath(Path.Combine(curPath, @"../../../../Tests/lab-3"));
        }
        
        [Test]
        public void MakeZipCopyOfFile_FileHasZipCopy()
        {
            
            IAlgorithm algorithm = new SplitStorageSave();
            IRepository repository = new Repository(repositoryDirectory);
            BackupJob backupJob = new BackupJob(repository, algorithm);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>();
            files.Add(directoryInfo.GetFiles().First(t => t.Name == "nano.txt"));
            backupJob.AddFiles(files);
            backupJob.Save();
            bool isSaved = File.Exists(Path.Combine(repositoryDirectory, "nano.zip"));
            Assert.AreEqual(true, isSaved);
        }

        [Test]
        public void ArchiveFiles_ArchiveExists()
        {
            
            IRepository repository = new Repository(repositoryDirectory);

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>(directoryInfo.GetFiles());
            IAlgorithm algorithm = new SingleStorageSave();
            BackupJob backupJob = new BackupJob(repository, algorithm);
            backupJob.AddFiles(files);
            backupJob.Save();
            bool isExists = File.Exists(Path.Combine(repositoryDirectory, "myzip.zip"));
            Assert.AreEqual(true, isExists);
        }

        [Test]
        public void ArchiveTwoFiles_ArchiveContainsTwoFiles()
        {
          
            IRepository repository = new Repository(repositoryDirectory);

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>(directoryInfo.GetFiles());
            IAlgorithm algorithm = new SingleStorageSave();
            BackupJob backupJob = new BackupJob(repository, algorithm);
            backupJob.AddFiles(files);
            backupJob.Save();
            var amountOfFiles = Directory.GetFiles(directoryWithFiles, "*", SearchOption.AllDirectories).Length;
            Assert.AreEqual(amountOfFiles, 
                ZipFileCount( repositoryDirectory + "/myzip.zip"));
        }

        private int ZipFileCount(string zipFileName)
        {
            using (ZipArchive archive = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
            {
                int count = 0;
                foreach (var entry in archive.Entries)
                    if (!String.IsNullOrEmpty(entry.Name))
                        count += 1;

                return count;
            }
        }

        [Test]
        public void SaveFiles_RestorePointsExist()
        {
           
            IRepository repository = new Repository(repositoryDirectory);
            IAlgorithm algorithm = new SingleStorageSave();
            BackupJob backupJob = new BackupJob(repository, algorithm);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>(directoryInfo.GetFiles());
            backupJob.AddFiles(files);
            backupJob.Save();
            backupJob.RemoveFile(files[0]);
            backupJob.Save();
            Assert.AreEqual(2, backupJob.RestorePoints.Count);
        }

        [Test]
        public void SaveFiles_RestorePointsDifferent()
        {
          
            IRepository repository = new Repository(repositoryDirectory);
            IAlgorithm algorithm = new SingleStorageSave();
            BackupJob backupJob = new BackupJob(repository, algorithm);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>(directoryInfo.GetFiles());
            backupJob.AddFiles(files);
            backupJob.Save();
            backupJob.RemoveFile(files[0]);
            backupJob.Save();
            List<RestorePoint> restorePoints = backupJob.RestorePoints;
            Assert.AreNotEqual(ZipFileCount(restorePoints[0].Files[0].ToString()),
                ZipFileCount(restorePoints[1].Files[0].ToString()));
        }

        [Test]
        public void SaveFewTimes_RepositoryIsTracking()
        {
          
            IRepository repository = new Repository(repositoryDirectory);
            IAlgorithm algorithm = new SingleStorageSave();
            BackupJob backupJob = new BackupJob(repository,algorithm);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>(directoryInfo.GetFiles());
            backupJob.AddFiles(files);
            int fCountBefore = Directory
                .GetFiles(backupJob.Repository.DirectoryInfo.FullName, "*", SearchOption.TopDirectoryOnly).Length;
            backupJob.Save();
            backupJob.RemoveFile(files[0]);
            algorithm = new SplitStorageSave();
            backupJob.SetAlghoritm(algorithm);
            backupJob.Save();
            List<RestorePoint> restorePoints = backupJob.RestorePoints;
            int fCountAfter = Directory
                .GetFiles(backupJob.Repository.DirectoryInfo.FullName, "*", SearchOption.TopDirectoryOnly).Length;
            Assert.AreEqual(restorePoints[0].Files.Count + restorePoints[1].Files.Count, fCountAfter - fCountBefore);
        }
    }
}