using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace RacingSite.Repositories
{
    public class RaceBuffer : IRacerBuffer
    {
        private readonly object _lock = new object();

        private readonly Dictionary<int, RacerCurrentState> _currentStates = new Dictionary<int, RacerCurrentState>();

        private Race _race;

        private Race Race
        {
            get => _race;
            set
            {
                _race = value;
                Checkpoints = Race.Checkpoints.ToLookup(c => c.Id);
            }
        }

        private ILookup<int, Checkpoint> Checkpoints { get; set; }

        private ILookup<int, Racer> Racers { get; set; }

        public List<RacerCurrentState> RacersCurrentStates
        {
            get
            {
                lock (_lock)
                {
                    return _currentStates.Values.OrderBy(s => s.TimeInRace).ToList();
                }
            }
        }

        public void Initialize(Race race, Racer[] racers)
        {
            lock (_lock)
            {
                _currentStates.Clear();
                Race = race;
                Racers = racers.ToLookup(r => r.Id);
            }
        }

        public void AddCheckpointPassing(CheckpointPassing checkpointPassing)
        {
            lock (_lock)
            {
                var racerId = checkpointPassing.RacerId;

                if (!_currentStates.ContainsKey(racerId))
                {
                    _currentStates.Add(racerId, new RacerCurrentState
                    {
                        Racer = Racers[racerId].First(),
                        Start = checkpointPassing.Time,
                    });
                }

                var state = _currentStates[racerId];
                state.PassedCheckpoint = Checkpoints[checkpointPassing.CheckpointId].First();
                state.CheckpointPassedTime = checkpointPassing.Time;
            }
        }
    }

    public class RacerCurrentState
    {
        public Racer Racer { get; set; }

        public DateTime Start { get; set; }

        public Checkpoint PassedCheckpoint { get; set; }

        public DateTime CheckpointPassedTime { get; set; }

        public TimeSpan TimeInRace => CheckpointPassedTime - Start;
    }
}