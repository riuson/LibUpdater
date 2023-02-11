using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LibUpdater.Data;

namespace LibUpdater.Utils;

public interface IHasher
{
    string HashStream(
        Stream stream,
        CancellationToken token,
        long length = -1);

    Task<string> HashStreamAsync(
        Stream stream,
        CancellationToken token,
        long length = -1);

    event EventHandler<ProgressEventArgs> Progress;
}