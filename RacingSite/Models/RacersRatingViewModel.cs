using System;
using System.Collections;
using System.Collections.Generic;

namespace RacingSite.Models
{
    public class RacersRatingViewModel
    {
        public List<CheckPointPassStats> Rating { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public int SelectedCheckPoint { get; set; }
    }

    public class CheckPointPassStats
    {
        public int RacerId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}