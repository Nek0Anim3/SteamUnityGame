namespace Player
{
    public struct PlayerStats
    {
        public readonly float speed;
        public readonly float dirX;
        public readonly float dirZ;

        public PlayerStats(float speed, float dirX, float dirZ)
        {
            this.speed = speed;
            this.dirX = dirX;
            this.dirZ = dirZ;
        }
    }
}