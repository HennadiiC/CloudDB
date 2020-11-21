using System.Linq;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace Core
{
    public class QueueService
    {
        private static readonly string QueueName = "race-queue";
        private static readonly string AccountConnectionString = @"DefaultEndpointsProtocol=https;AccountName=queuemessage;AccountKey=EziaS6ZvsMoMdjXw47QxfAhtOgbXErdYFYyYDM3jIBhng+ammeV+kqHxGMmviycdtOe4kTJSMnEIC3ebGwvpXw==;EndpointSuffix=core.windows.net";
        private readonly QueueClient _queueClient;

        public QueueService()
        {
            _queueClient = new QueueClient(AccountConnectionString, QueueName);
        }

        public void SendMessage(string value)
        {
            _queueClient.SendMessage(value);
        }

        public QueueMessage[] ReceiveMessages()
        {
            return _queueClient.ReceiveMessages(32).Value.ToArray();
        }

        public void DeleteMessage(QueueMessage message)
        {
            _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }
    }
}