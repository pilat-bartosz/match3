using Unity.Entities;

namespace _match3.Game
{
    //As there is only 6 monsters I can setup this to have capacity for score inside a chunk
    [InternalBufferCapacity(6)]
    public struct GameScoreBuffer : IBufferElementData
    {
        public int score;
    }
    
    public struct GameScoreTargetBuffer : IBufferElementData
    {
        public int score;
    }
}