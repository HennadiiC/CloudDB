using System;

namespace RacingSite.Models
{
    public class CheckpointPass
    {
        public int Id { get; set; }

        public int RacerId { get; set; }

        public int CheckpointId { get; set; }

        public DateTime Time { get; set; }
    }
}