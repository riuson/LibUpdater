using System;
using System.IO;
using System.Threading.Tasks;
using LibUpdater.Data;

namespace LibUpdater.Utils;

public interface IHasher
{
    string HashStream(Stream stream, long length = -1);
    Task<string> HashStreamAsync(Stream stream, long length = -1);

    event EventHandler<ProgressEventArgs> Progress;
}