namespace RacingSite.Core
{
    public class Race
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Checkpoint[] Checkpoints { get; set; }
    }
}