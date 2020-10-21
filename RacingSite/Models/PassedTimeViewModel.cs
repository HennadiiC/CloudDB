using System.Collections.Generic;

namespace RacingSite.Models
{
    public class PassedTimeViewModel
    {
        public PassedTimeFilter Filter { get; set; }

        public long PassedTimeInMinutes { get; set; }
        public List<CheckPoint> Checkpoints { get; set; }
        public List<Racer> Racers { get; set; }
    }
}