using System.Collections.Generic;
using Core;

namespace RacingSite.Repositories
{
    public interface IRacerBuffer
    {
        List<RacerCurrentState> RacersCurrentStates { get; }

        void Initialize(Race race, Racer[] racers);

        void AddCheckpointPassing(CheckpointPassing checkpointPassing);
    }
}