using System.Threading;
using System.Threading.Tasks;

namespace LibUpdater.Utils;

public static class WaitHandleExtensions
{
    public static async Task WaitOneAsync(
        this WaitHandle waitHandle)
    {
        await Task.Run(() => waitHandle.WaitOne());
    }

    public static async Task WaitOneAsync(
        this WaitHandle waitHandle,
        int millisecondsTimeout)
    {
        await Task.Run(() => waitHandle.WaitOne(
            millisecondsTimeout));
    }

    public static async Task WaitOneAsync(
        this WaitHandle waitHandle,
        int millisecondsTimeout,
        CancellationToken token)
    {
        await Task.Run(() => waitHandle.WaitOne(
            millisecondsTimeout));
    }
}