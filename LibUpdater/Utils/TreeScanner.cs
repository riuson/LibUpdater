using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibUpdater.Data;

namespace LibUpdater.Utils
{
    public class TreeScanner
    {
        private readonly Hasher _hasher = new Hasher();

        public IEnumerable<IFileItem> ScanTree(string path, int degreeOfParallelism = 1)
        {
            var di = new DirectoryInfo(path);

            var result = di.EnumerateFiles("*", SearchOption.AllDirectories)
                .Select(x => new FileItem() { Path = x.FullName, Size = x.Length })
                .AsParallel().WithDegreeOfParallelism(degreeOfParallelism)
                .Select(CalculateHash)
                .OrderBy(x => x.Path)
                .ToArray();

            return result;
        }

        private FileItem CalculateHash(FileItem item)
        {
            using (var stream = new FileStream(item.Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var hash = _hasher.HashStream(stream);
                item.Hash = hash;
            }

            return item;
        }
    }
}