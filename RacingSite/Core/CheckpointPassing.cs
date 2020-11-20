using System;

namespace RacingSite.Core
{
    public class CheckpointPassing
    {
        public int Id { get; set; }

        public int RacerId { get; set; }

        public int CheckpointId { get; set; }

        public DateTime Time { get; set; }
    }
}