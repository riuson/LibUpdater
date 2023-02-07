using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibUpdater.Utils
{
    public class DirectoryScanner
    {
        private readonly Hasher hasher = new Hasher();

        public IEnumerable<IFileItem> ScanTree(string path, int degreeOfParallelism = 1)
        {
            var di = new DirectoryInfo(path);

            var result = di.EnumerateFiles("*", SearchOption.AllDirectories)
                .Select(x => new FileItem() { Path = x.FullName, Size = x.Length })
                .OrderBy(x => x.Path)
                .ToArray();

            Parallel.ForEach(
                result,
                new ParallelOptions()
                {
                    MaxDegreeOfParallelism = degreeOfParallelism,
                },
                CalculateHash);

            return result;
        }

        private void CalculateHash(FileItem item)
        {
            using (var stream = new FileStream(item.Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var hash = hasher.HashStream(stream);
                item.Hash = hash;
            }
        }
    }
}