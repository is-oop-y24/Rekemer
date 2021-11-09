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
        [Test]
        public void MakeZipCopyOfFile_FileHasZipCopy()
        {
            string directoryWithFiles = @"C:\lab-3\texts";
            string repositoryDirectory = @"C:\lab-3";
            IAlgorithm algorithm = new SplitStorageSave();
            IRepository repository = new Repository(repositoryDirectory);
            BackupJob backupJob = new BackupJob(repository);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>();
            files.Add(directoryInfo.GetFiles().First(t => t.Name == "note.txt"));
            backupJob.AddFiles(files);
            backupJob.SetAlghoritm(algorithm);
            backupJob.Save();
            bool isSaved = File.Exists(Path.Combine(repositoryDirectory, "note.zip"));
            Assert.AreEqual(true, isSaved);
        }

        [Test]
        public void ArchiveFiles_ArchiveExists()
        {
            string directoryWithFiles = @"C:\lab-3\texts";
            string repositoryDirectory = @"C:\lab-3";
            IRepository repository = new Repository(repositoryDirectory);
            BackupJob backupJob = new BackupJob(repository);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>(directoryInfo.GetFiles());
            backupJob.AddFiles(files);
            IAlgorithm algorithm = new SingleStorageSave();
            backupJob.SetAlghoritm(algorithm);
            backupJob.Save();
            bool isExists = File.Exists(Path.Combine(repositoryDirectory, "myzip.zip"));
            Assert.AreEqual(true, isExists);
        }

        [Test]
        public void ArchiveTwoFiles_ArchiveContainsTwoFiles()
        {
            string directoryWithFiles = @"C:\lab-3\texts";
            string repositoryDirectory = @"C:\lab-3";
            IRepository repository = new Repository(repositoryDirectory);
            BackupJob backupJob = new BackupJob(repository);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>(directoryInfo.GetFiles());
            backupJob.AddFiles(files);
            IAlgorithm algorithm = new SingleStorageSave();
            backupJob.SetAlghoritm(algorithm);
            backupJob.Save();
            Assert.AreEqual(2, ZipFileCount(@"C:\lab-3/myzip.zip"));
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
            string directoryWithFiles = @"C:\lab-3\texts";
            string repositoryDirectory = @"C:\lab-3";
            IRepository repository = new Repository(repositoryDirectory);
            BackupJob backupJob = new BackupJob(repository);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>(directoryInfo.GetFiles());
            backupJob.AddFiles(files);
            IAlgorithm algorithm = new SingleStorageSave();
            backupJob.SetAlghoritm(algorithm);
            backupJob.Save();
            backupJob.RemoveFile(files[0]);
            backupJob.Save();
            Assert.AreEqual(2, backupJob.RestorePoints.Count);
        }

        [Test]
        public void SaveFiles_RestorePointsDifferent()
        {
            string directoryWithFiles = @"C:\lab-3\texts";
            string repositoryDirectory = @"C:\lab-3";
            IRepository repository = new Repository(repositoryDirectory);
            BackupJob backupJob = new BackupJob(repository);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>(directoryInfo.GetFiles());
            backupJob.AddFiles(files);
            IAlgorithm algorithm = new SingleStorageSave();
            backupJob.SetAlghoritm(algorithm);
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
            string directoryWithFiles = @"C:\lab-3\texts";
            
            string repositoryDirectory = @"C:\lab-3";
            IRepository repository = new Repository(repositoryDirectory);
            BackupJob backupJob = new BackupJob(repository);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<FileInfo> files = new List<FileInfo>(directoryInfo.GetFiles());
            backupJob.AddFiles(files);
            IAlgorithm algorithm = new SingleStorageSave();
            backupJob.SetAlghoritm(algorithm);
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