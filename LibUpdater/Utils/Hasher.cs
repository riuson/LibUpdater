using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LibUpdater.Data;

namespace LibUpdater.Utils;

internal class Hasher : IHasher
{
    public string HashStream(Stream stream, long length = -1)
    {
        using var hashAlg = new SHA1Managed();
        var id = Guid.NewGuid();

        var buffer = new byte[1024 * 1024];
        var totalReaded = 0L;

        while (true)
        {
            var readed = stream.Read(buffer, 0, buffer.Length);
            totalReaded += readed;

            if (readed > 0)
            {
                hashAlg.TransformBlock(buffer, 0, readed, buffer, 0);

                if (length > 0) ReportProgress(totalReaded, length, id);
            }
            else
            {
                hashAlg.TransformFinalBlock(buffer, 0, readed);
                ReportProgress(totalReaded, totalReaded, id);
                return ByteArrayToString(hashAlg.Hash);
            }
        }
    }

    public async Task<string> HashStreamAsync(Stream stream, long length = -1)
    {
        using var hashAlg = new SHA1Managed();
        var id = Guid.NewGuid();

        var buffer = new byte[1024 * 1024];
        var totalReaded = 0L;

        while (true)
        {
            var readed = await stream.ReadAsync(buffer, 0, buffer.Length);
            totalReaded += readed;

            if (readed > 0)
            {
                hashAlg.TransformBlock(buffer, 0, readed, buffer, 0);

                if (length > 0) ReportProgress(totalReaded, length, id);
            }
            else
            {
                hashAlg.TransformFinalBlock(buffer, 0, readed);
                ReportProgress(totalReaded, totalReaded, id);
                return ByteArrayToString(hashAlg.Hash);
            }
        }
    }

    public event EventHandler<ProgressEventArgs> Progress;

    private string ByteArrayToString(byte[] array)
    {
        var sb = new StringBuilder(array.Length * 2);

        foreach (var b in array)
            // can be "x2" if you want lowercase
            sb.Append(b.ToString("X2"));

        return sb.ToString().ToLower();
    }

    private void ReportProgress(long current, long total, Guid id)
    {
        Progress?.Invoke(
            this,
            new ProgressEventArgs
            {
                Current = current,
                Total = total,
                Id = id
            });
    }
}