using CloudFlareR2Storage;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel;
using POSV1.TenantModel.Models.EntityModels.CloudR2;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Services.BackgroundJobs
{
    public class CloudR2SingleFileProcessor : BackgroundService
    {
        private readonly ILogger<CloudR2SingleFileProcessor> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SingleFileProcessingQueue _queue;

        public CloudR2SingleFileProcessor(
            ILogger<CloudR2SingleFileProcessor> logger,
            IServiceScopeFactory scopeFactory,
            SingleFileProcessingQueue queue)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var (itemId, itemType) = await _queue.DequeueAsync(stoppingToken);

                switch (itemType)
                {
                    case EnumFileProcessingType.Purchase:
                        //await ProcessPurchaseFile(itemId);
                        break;
                    default:
                        throw new Exception("invalid Queue Type");
                        break;
                }
                ;
            }
        }

        //public async Task ProcessPurchaseFile(int purId)
        //{
        //    using (var scope = _scopeFactory.CreateScope())
        //    {
        //        var QueueRepo = scope.ServiceProvider.GetRequiredService<IPurchaseRepo>();
        //        var r2Service = scope.ServiceProvider.GetRequiredService<R2StorageService>();

        //        _logger.LogInformation($"Process upload Checking Data for documentId: {purId}");

        //        pur01purchases data = await QueueRepo.GetDetailAsync(purId);

        //        var processingRecord = data.CloudR2Purchase
        //                .OrderByDescending(x => x.StartTime)
        //                .FirstOrDefault(p => p.ProcStatus == JobProcessingEnum.Created || p.ProcStatus == JobProcessingEnum.Failed);

        //        try
        //        {
        //            if (processingRecord == null)
        //            {
        //                if (!data.CloudR2Purchase.Any(p => p.PurchaseId == data.pur01uin))
        //                {
        //                    processingRecord = new CloudR2Purchase()
        //                    {
        //                        PurchaseId = data.pur01uin,
        //                        StartTime = DateTime.Now,
        //                        ProcStatus = JobProcessingEnum.Created
        //                    };
        //                    data.CloudR2Purchase.Add(processingRecord);
        //                }
        //                else
        //                {
        //                    _logger.LogWarning($"Duplicate entry prevented for OrganizationId {data.pur01uin}");
        //                    processingRecord = data.CloudR2Purchase.FirstOrDefault(p => p.PurchaseId == data.pur01uin);
        //                }
        //            }
        //            else
        //            {
        //                processingRecord.StartTime = DateTime.Now;
        //                processingRecord.ProcStatus = JobProcessingEnum.Created;
        //            }

        //            string organization_path = $"Organization/";
        //            data.CloudR2Purchase = await r2Service.UploadFileFromDiskAsync(organization_path, data.filePath);

        //            if (string.IsNullOrEmpty(data.CloudR2ImagePath))
        //            {
        //                throw new Exception("CloudR2ImagePath is empty. Upload may have failed.");
        //            }

        //            processingRecord.Path = data.filePath;
        //            processingRecord.CloudR2Path = data.CloudR2ImagePath;
        //            processingRecord.ProcStatus = JobProcessingEnum.Completed;
        //            processingRecord.EndTime = DateTime.Now;

        //            _logger.LogInformation($"Process upload completed for organization ID: {data.pur01uin}");
        //        }
        //        catch (FileNotFoundException ex)
        //        {
        //            _logger.LogError(ex, $"Failed to process upload for organization ID: {data.pur01uin}");
        //            processingRecord.ProcStatus = JobProcessingEnum.NoSourceFile;
        //            processingRecord.FailRemarks = ex.Message;
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, $"Failed to process upload for organization ID: {data.pur01uin}");
        //            processingRecord.ProcStatus = JobProcessingEnum.Failed;
        //            processingRecord.FailRemarks = ex.Message;
        //        }

        //        QueueRepo.Update(data);

        //        try
        //        {
        //            await QueueRepo.SaveAsync();
        //            _logger.LogInformation($"Successfully saved data for organization ID: {data.pur01uin}");
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Unexpected error occurred while saving organization data.");
        //        }
        //    }
        //}


        private static string getCurrentDate()
        {
            DateTime date = DateTime.Now;
            string currentDate = date.ToString("yyyy-MM");
            return currentDate;
        }
    }
}
