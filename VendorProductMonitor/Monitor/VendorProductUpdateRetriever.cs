using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
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
                // Required to delete messages after retrieving them, whether successfully processed or not
                queueMsgList.ForEach(async qmsg => await _queue.DeleteMessageAsync(qmsg));
                
                var updates = new List<VendorProductUpdate>();
                foreach (var queueMsg in queueMsgList)
                {                    
                    var updateMsgObj = JsonConvert.DeserializeObject<VendorProductUpdateQueueMessage>(queueMsg.AsString);
                    updates.AddRange(updateMsgObj.Updates);
                }

                return updates;
            }

            return null;
        }
    }
}
