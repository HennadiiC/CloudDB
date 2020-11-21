using System.Linq;
using Azure.Storage.Queues;

namespace RaceFiller
{
    public class QueueService
    {
        public static readonly string QueueName = "race-queue";
        public static readonly string AccountConnectionString = @"DefaultEndpointsProtocol=https;AccountName=queuemessage;AccountKey=EziaS6ZvsMoMdjXw47QxfAhtOgbXErdYFYyYDM3jIBhng+ammeV+kqHxGMmviycdtOe4kTJSMnEIC3ebGwvpXw==;EndpointSuffix=core.windows.net";
        private readonly QueueClient _queueClient;

        public QueueService()
        {
            _queueClient = new QueueClient(AccountConnectionString, QueueName);
        }

        public void SendMessage(string value)
        {
            _queueClient.SendMessage(value);
        }

        public string[] ReceiveMessages()
        {
            return _queueClient.ReceiveMessages(32).Value.Select(m => m.MessageText).ToArray();
        }
    }
}