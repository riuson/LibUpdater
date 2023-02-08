using System;
using System.Collections.Generic;
using System.IO;
using LibUpdater.Data;

namespace LibUpdater.Tests.Utils;

internal class ItemsEqualityComparer : IEqualityComparer<IFileItem>
{
    private readonly string targetDirectory;

    public ItemsEqualityComparer(string targetDirectory)
    {
        this.targetDirectory = targetDirectory;
    }

    public bool Equals(IFileItem x, IFileItem y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;

        if (x.Size != y.Size) return false;
        if (x.Hash.ToLower() != y.Hash.ToLower()) return false;

        var pathX = Path.IsPathRooted(x.Path) ? x.Path : Path.Combine(targetDirectory, x.Path);
        var pathY = Path.IsPathRooted(y.Path) ? y.Path : Path.Combine(targetDirectory, y.Path);

        // Normalize path separators.
        var fileInfoX = new FileInfo(pathX);
        var fileInfoY = new FileInfo(pathY);

        return fileInfoX.FullName == fileInfoY.FullName;
    }

    public int GetHashCode(IFileItem obj)
    {
        unchecked
        {
            var path = Path.IsPathRooted(obj.Path) ? obj.Path : Path.Combine(targetDirectory, obj.Path);
            var fileInfo = new FileInfo(path);
            path = fileInfo.FullName;

            var hashCode = path != null ? path.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ obj.Size.GetHashCode();
            hashCode = (hashCode * 397) ^ (obj.Hash != null ? obj.Hash.ToLower().GetHashCode() : 0);
            return hashCode;
        }
    }
}