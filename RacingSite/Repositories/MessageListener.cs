using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RacingSite.Hubs;

namespace RacingSite.Repositories
{
    class MessageListener : IMessageListener
    {
        private readonly object _lock = new object();

        private readonly IRacerBuffer _buffer;

        private readonly IHubContext<RaceHub> _hubContext;

        private readonly QueueService _queueService = new QueueService();

        private bool _started;

        public MessageListener(IRacerBuffer buffer, IHubContext<RaceHub> hubContext)
        {
            _buffer = buffer;
            _hubContext = hubContext;
        }

        public void Start()
        {
            lock (_lock)
            {
                if (_started)
                    return;

                Task.Run(Listen);
                _started = true;
            }
        }

        private void Listen()
        {
            while (true)
            {
                var checkpointPassings = _queueService.ReceiveMessages()
                    .Select(m =>
                    {
                        _queueService.DeleteMessage(m);
                        return m.MessageText;
                    })
                    .Select(JsonConvert.DeserializeObject<CheckpointPassing>)
                    .ToArray();

                foreach (var passing in checkpointPassings)
                {
                    _buffer.AddCheckpointPassing(passing);
                }

                if (checkpointPassings.Length > 0)
                {
                    _hubContext.Clients.All.SendAsync(HubConstants.OnCheckpointPassed, _buffer.RacersCurrentStates);
                }

                Thread.Sleep(1000);
            }
        }
    }
}