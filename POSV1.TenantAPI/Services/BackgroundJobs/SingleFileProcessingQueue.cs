using POSV1.TenantAPI.Models;
using System.Collections.Concurrent;

namespace POSV1.TenantAPI.Services.BackgroundJobs
{
    public class SingleFileProcessingQueue
    {
        private readonly ConcurrentQueue<(int filePath, EnumFileProcessingType ItemType)> _queue = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void Enqueue(int filePath, EnumFileProcessingType itemType)
        {
            _queue.Enqueue((filePath, itemType));
            _signal.Release();
        }

        public async Task<(int Id, EnumFileProcessingType ItemType)> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _queue.TryDequeue(out var result);
            return result;
        }
    }
}
