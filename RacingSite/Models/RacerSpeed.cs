using System.Collections.Generic;

namespace RacingSite.Models
{
    public class RacerSpeed
    {
        public List<Racer> Racers { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public double Speed { get; set; }
        public int SelectedRacer { get; set; }
        public int SelectedCheckPoint { get; set; }
    }
}