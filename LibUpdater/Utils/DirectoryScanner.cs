using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibUpdater.Utils
{
    public class DirectoryScanner
    {
        public IEnumerable<IFileItem> ScanTree(string path)
        {
            var di = new DirectoryInfo(path);

            var result = di.EnumerateFiles("*", SearchOption.AllDirectories)
                .Select(x => new FileItem() { Path = x.FullName, Size = x.Length })
                .OrderBy(x => x.Path)
                .ToArray();

            return result;
        }
    }
}