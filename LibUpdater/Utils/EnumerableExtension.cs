using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibUpdater.Utils;

internal static class EnumerableExtension
{
    // https://medium.com/@nayanava.de01/controlling-degree-of-parallelism-with-task-whenall-in-c-1ef1e9ff2bc
    public static async Task ForEachAsync<T>(this IEnumerable<T> collection, int degreeOfParallelism,
        Func<T, Task> method)
    {
        using var semaphore = new SemaphoreSlim(degreeOfParallelism, degreeOfParallelism);

        var tasks = collection.Select(async item =>
        {
            await semaphore.WaitAsync();

            try
            {
                await method(item);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
    }
}