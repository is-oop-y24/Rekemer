using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using BackupsExtra.Algorithms.DeleteAlgorithms;
using BackupsExtra.Algorithms.LogInterface;
using BackupsExtra.Algorithms.RestoreAlgorithms;
using BackupsExtra.Algorithms.SaveAlgorithms;
using BackupsExtra.Algorithms.TypeOfHybrids;
using BackupsExtra.Jobs;
using BackupsExtra.Save;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    public class BackupExtraTests
    {
        private string directoryWithFiles;
        private string repositoryDirectory;
        private string curPath = Directory.GetCurrentDirectory();

        [SetUp]
        public void Setup()
        {
            directoryWithFiles = Path.GetFullPath(Path.Combine(curPath, @"../../../../Tests/texts"));
            SaveSystem.PathOfSaving = Path.GetFullPath(Path.Combine(curPath, @"../../../../Tests/lab5/saveData.bin"));
            repositoryDirectory = Path.GetFullPath(Path.Combine(curPath, @"../../../../Tests/lab-3"));
        }

        [Test]
        public void SystemSave_SystemIsSaved()
        {
            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();
            IAlgorithm algorithm = new SingleStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new AmountOfPointsDelete(3);
            delete.IsMerge = true;
            BackupJob backupJob = new BackupJob(repository, algorithm, delete);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();
            var filesTosave = directoryInfo.GetFiles();
            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob.AddFiles(files);
            backupJob.Save();
            bool isFileEmpty = new FileInfo(SaveSystem.PathOfSaving).Length == 0;
            Assert.AreEqual(false, isFileEmpty);
        }

        [Test]
        public void SystemLoads_SystemCanBeLoaded()
        {
            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();

            IAlgorithm algorithm = new SingleStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new AmountOfPointsDelete(3);
            delete.IsMerge = true;
            delete.IsMerge = true;
            BackupJob backupJob1 = new BackupJob(repository, algorithm, delete);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();
            var filesTosave = directoryInfo.GetFiles();
            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob1.AddFiles(files);
            backupJob1.Save();
            BackupJob backupJob = new BackupJob();
            Assert.AreEqual(true, backupJob.Load());
        }

        [Test]
        public void PointsAreTooMany_RestorePointsAreClearedWithAmountLimit()
        {
            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();
            IAlgorithm algorithm = new SingleStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new AmountOfPointsDelete(3);
            delete.IsMerge = true;
            BackupJob backupJob = new BackupJob(repository, algorithm, delete);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();
            var filesTosave = directoryInfo.GetFiles();
            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob.AddFiles(files);
            backupJob.Save();
            backupJob.Save();
            backupJob.Save();
            var firstPoint = backupJob.RestorePoints[0];
            backupJob.Save();
            bool isPointExists = backupJob.RestorePoints.Contains(firstPoint);
            Assert.AreEqual(false, isPointExists);
        }

        [Test]
        public void PointsAreTooMany_RestorePointsAreClearedWithDateLimit()
        {
            Time.Instance.ResetTime();

            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();
            if (!Directory.Exists(directoryWithFiles))
            {
                Assert.Pass();
            }

            IAlgorithm algorithm = new SingleStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new ValidTimeDelete(new Vector3(0, 2, 0));
            BackupJob backupJob = new BackupJob(repository, algorithm, delete);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();
            var filesTosave = directoryInfo.GetFiles();
            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob.AddFiles(files);
            backupJob.Save();
            var firstPoint = backupJob.RestorePoints[0];
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            var secondPoint = backupJob.RestorePoints[1];
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            bool isPointExists1 = backupJob.RestorePoints.Contains(firstPoint);
            bool isPointExists2 = backupJob.RestorePoints.Contains(secondPoint);
            Assert.AreEqual(true, !isPointExists1 && !isPointExists2);
        }

        [Test]
        public void PointsAreTooMany_RestorePointsAreClearedWithHybridPartialLimit()
        {
            Time.Instance.ResetTime();


            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();
            IAlgorithm algorithm = new SingleStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new HybridDelete(new PartialHybrid(2, new Vector3(0, 2, 0)));
            BackupJob backupJob = new BackupJob(repository, algorithm, delete);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();
            var filesTosave = directoryInfo.GetFiles();
            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob.AddFiles(files);
            backupJob.Save();

            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            var points = backupJob.RestorePoints;
            bool passed = false;
            if (points.Count == 2)
            {
                foreach (var restorePoint in points)
                {
                    var month = Convert.ToDateTime(restorePoint._time).Month;
                    if (month <= Time.Instance.CurrentTime.Month - month)
                    {
                        passed = true;
                    }
                }

                Assert.AreEqual(false, passed);
            }
        }

        [Test]
        public void PointsAreTooMany_RestorePointsAreClearedWithHybridAbsoluteLimit()
        {
            Time.Instance.ResetTime();


            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();


            IAlgorithm algorithm = new SingleStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new HybridDelete(new AbsoluteHybrid(2, new Vector3(0, 2, 0)));
            BackupJob backupJob = new BackupJob(repository, algorithm, delete);

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();
            var filesTosave = directoryInfo.GetFiles();
            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob.AddFiles(files);
            backupJob.Save();
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            Time.Instance.AddTime(0, 1, 0);
            backupJob.Save();
            var points = backupJob.RestorePoints;
            bool passed = false;
            if (points.Count == 2)
            {
                foreach (var restorePoint in points)
                {
                    var month = Convert.ToDateTime(restorePoint._time).Month;
                    if (month <= Time.Instance.CurrentTime.Month - month)
                    {
                        passed = true;
                    }
                }

                Assert.AreEqual(false, passed);
            }
        }

        [Test]
        public void PointsAreSame_RepeatedPointsAreDeleted()
        {
            Time.Instance.ResetTime();
            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();
            IAlgorithm algorithm = new SingleStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new ValidTimeDelete((new Vector3(0, 2, 0)));
            delete.IsMerge = true;
            BackupJob backupJob = new BackupJob(repository, algorithm, delete);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();
            var filesTosave = directoryInfo.GetFiles();
            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob.AddFiles(files);
            backupJob.Save();
            backupJob.Save();
            string path = Path.Combine(directoryWithFiles , "helloWorld.txt");
            files.Add(path);
            backupJob.AddFiles(files);
            Time.Instance.AddTime(1, 0, 0);
            backupJob.Save();
            Assert.AreEqual(true, backupJob.RestorePoints.Count == 1);
        }

        // [Test]
        // public void OldPointsHaveNewObject_UpdateNewPoint()
        // {
        //     Time.Instance.ResetTime();
        //     Log.Init(new ConsoleLog());
        //     File.Create(SaveSystem.PathOfSaving).Close();
        //     IAlgorithm algorithm = new SingleStorageSave();
        //     Repository repository = new Repository(repositoryDirectory);
        //     var delete = new HybridDelete(new PartialHybrid(3, new Vector3(0, 2, 0)));
        //     delete.IsMerge = true;
        //     BackupJob backupJob = new BackupJob(repository, algorithm, delete);
        //     DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
        //     List<string> files = new List<string>();
        //     string path = Path.Combine(directoryWithFiles , "helloWorld.txt");
        //     var filesTosave = directoryInfo.GetFiles();
        //
        //     foreach (var fileInfo in filesTosave)
        //     {
        //         files.Add(fileInfo.FullName);
        //     }
        //
        //     backupJob.AddFiles(files);
        //     backupJob.Save();
        //
        //     backupJob.RemoveFile(path);
        //     Time.Instance.AddTime(1, 0, 0);
        //     backupJob.Save();
        //     bool isPassed = backupJob.RestorePoints.Count == 1 && backupJob.RestorePoints[0].Files.Count == 3;
        //     Assert.AreEqual(true, isPassed);
        // }

        [Test]
        public void NewPointIsSingleStorage_DeleteOldPoint()
        {
            Time.Instance.ResetTime();

            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();
            IAlgorithm algorithm = new SplitStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new HybridDelete(new PartialHybrid(2, new Vector3(0, 2, 0)));
            delete.IsMerge = true;
            BackupJob backupJob = new BackupJob(repository, algorithm, delete);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();

            var filesTosave = directoryInfo.GetFiles();

            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob.AddFiles(files);
            backupJob.Save();
            Time.Instance.AddTime(1, 0, 0);
            backupJob.SetAlghoritm(new SingleStorageSave());
            backupJob.Save();
            bool isPassed = backupJob.RestorePoints.Count == 1 &&
                            backupJob.RestorePoints[0].Alghoritm == new SingleStorageSave().NameOfAlgorithm();
            Assert.AreEqual(true, isPassed);
        }

        [Test]
        public void TryDeleteAllPoints_ThrowException()
        {
            Time.Instance.ResetTime();
            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();
            IAlgorithm algorithm = new SplitStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new HybridDelete(new PartialHybrid(2, new Vector3(0, 2, 0)));
            BackupJob backupJob = new BackupJob(repository, algorithm, delete);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();

            var filesTosave = directoryInfo.GetFiles();

            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob.AddFiles(files);
            backupJob.Save();
            backupJob.SetAlghoritm(new SingleStorageSave());
            backupJob.Save();
            backupJob.Save();
            Time.Instance.AddTime(1, 0, 0);
            Assert.Throws<Exception>(delegate { backupJob.Save(); });
        }

        [Test]
        public void RestoreFilesToOriginalLocation_FilesAreRestored()
        {
            Time.Instance.ResetTime();
            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();
            IAlgorithm algorithm = new SplitStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new HybridDelete(new PartialHybrid(2, new Vector3(0, 2, 0)));
            BackupJob backupJob = new BackupJob(repository, algorithm, delete);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();
            var filesTosave = directoryInfo.GetFiles();

            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob.AddFiles(files);
            backupJob.Save();
            backupJob.Restore(backupJob.RestorePoints[0], new RestoreInOriginalLocation());
            Assert.AreEqual(filesTosave.Length, new DirectoryInfo(directoryWithFiles).GetFiles().Length);
        }

        [Test]
        public void RestoreFilesToCustomLocation_FilesAreRestored()
        {
            Time.Instance.ResetTime();

            Log.Init(new ConsoleLog());
            File.Create(SaveSystem.PathOfSaving).Close();
            IAlgorithm algorithm = new SplitStorageSave();
            Repository repository = new Repository(repositoryDirectory);
            var delete = new HybridDelete(new PartialHybrid(2, new Vector3(0, 2, 0)));
            BackupJob backupJob = new BackupJob(repository, algorithm, delete);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryWithFiles);
            List<string> files = new List<string>();


            var filesTosave = directoryInfo.GetFiles();

            foreach (var fileInfo in filesTosave)
            {
                files.Add(fileInfo.FullName);
            }

            backupJob.AddFiles(files);
            backupJob.Save();
            string location = Path.GetFullPath(Path.Combine(curPath, @"../../../../Tests/lab5/files"));
            backupJob.Restore(backupJob.RestorePoints[0], new RestoreInCustomLocation(location));
            Assert.AreEqual(filesTosave.Length, new DirectoryInfo(location).GetFiles().Length);
        }
    }
}