using System.Collections.Generic;

namespace RacingSite.Models
{
    public class PassedDistanceViewModel
    {
        public int SelectedRacer { get; set; }

        public List<Racer> Racers { get; set; }

        public float Distance { get; set; }
    }
}