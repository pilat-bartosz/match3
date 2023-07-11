using Unity.Entities;

namespace _match3.Game
{
    public struct GameScoreBuffer : IBufferElementData
    {
        public int score;
    }
    
    public struct GameScoreTargetBuffer : IBufferElementData
    {
        public int score;
    }
}