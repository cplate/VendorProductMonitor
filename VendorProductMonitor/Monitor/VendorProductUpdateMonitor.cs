using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VendorProductMonitor.Monitor
{
    public class VendorProductUpdateMonitor : IVendorProductUpdateMonitor
    {
        public event EventHandler<VendorProductsUpdatedEventArgs> VendorProductsUpdated;

        private readonly IVendorProductUpdateRetriever _updateRetriever;

        private CancellationTokenSource _cancelTokenSource;
        private CancellationToken _cancelToken;
        private readonly int _pollFrequencyMillis;

        public VendorProductUpdateMonitor(IVendorProductUpdateRetriever updateRetriever)
        {
            _updateRetriever = updateRetriever;

            if (!int.TryParse(ConfigurationManager.AppSettings["PollFrequencyMilliseconds"], out _pollFrequencyMillis))
            {
                _pollFrequencyMillis = 1000;
            }
        }

        public void Start()
        {
            _cancelTokenSource = new CancellationTokenSource();
            _cancelToken = _cancelTokenSource.Token;
            Task.Run(async () => await Poll(_pollFrequencyMillis), _cancelToken);
        }

        private async Task Poll(int pollFrequencyMillis)
        {
            while (!_cancelToken.IsCancellationRequested)
            {
                try {
                    var updateMsgs = await _updateRetriever.CheckForProductUpdates();

                    if (updateMsgs != null)
                    {
                        var updateMsgList = updateMsgs.ToList();
                        VendorProductsUpdated?.Invoke(this, new VendorProductsUpdatedEventArgs(updateMsgList));
                    }

                    await Task.Delay(pollFrequencyMillis, _cancelToken); // avoid over-polling
                }
                catch (Exception)
                {
                    // Normally do some kind of logging or stop polling and alert user to problem and allow restart of polling
                    // But dont want any exception thrown from here as we're using Task.Run and our monitor will die
                }
            }            
        }

        public void Stop()
        {
            _cancelTokenSource.Cancel();
        }
    }
}
