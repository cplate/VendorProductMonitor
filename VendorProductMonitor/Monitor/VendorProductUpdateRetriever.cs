using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace VendorProductMonitor.Monitor
{
    public class VendorProductUpdateRetriever : IVendorProductUpdateRetriever
    {
        private readonly CloudQueue _queue;

        public VendorProductUpdateRetriever()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(ConfigurationManager.AppSettings["ProductUpdateQueueName"]);            
        }

        public async Task<IEnumerable<VendorProductUpdate>> CheckForProductUpdates()
        {
            IEnumerable<CloudQueueMessage> queueMsgs = await _queue.GetMessagesAsync(10); // Could parameterize this if needed  

            var queueMsgList = queueMsgs?.ToList();
            if (queueMsgList != null && queueMsgList.Any())
            {
                var updates = new List<VendorProductUpdate>();
                foreach (var queueMsg in queueMsgList)
                {
                    var updateMsgObj = JsonConvert.DeserializeObject<VendorProductUpdateQueueMessage>(queueMsg.AsString);
                    updates.AddRange(updateMsgObj.Updates);

                    // Could wait until successfully processing the message before deleting message, forcing caller to
                    // call another method to delete the message.  Keeping simple to start
                    await _queue.DeleteMessageAsync(queueMsg);
                }

                return updates;
            }

            return null;
        }
    }
}
