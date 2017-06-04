using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ZLogger
{
    public class AverageAnalyzer : IAnalyzer
    {
        public INumericalFactory FileFactory { get; }

        public AverageAnalyzer(INumericalFactory fileFactory)
        {
            FileFactory = fileFactory;
        }

        public Dictionary<string, Cabinet> AnalyzeAll(IEnumerable<Cabinet> logs)
        {
            return logs.ToDictionary(cabinet => cabinet.Label, Analyze);
        }

        public Cabinet Analyze(Cabinet cabinet)
        {
            var averageCabinet = new Cabinet(cabinet.Label);
            foreach (var folder in cabinet.GetAllFolders())
            {
                var averageFile = AverageToFile(folder.Value);
                averageCabinet.StoreFile(folder.Key, averageFile);
            }
            return averageCabinet;
        }

        private IFile AverageToFile(IEnumerable<IFile> files)
        {
            var average = AverageFolder(files);
            return FileFactory.Create(average);
        }

        private double AverageFolder(IEnumerable<IFile> files)
        {
            var average = files
                .Select(f => f as INumerical)
                .Where(f => f != null)
                .Average(f => f.Value());
            return (int) Math.Round(average);
        }
    }
}