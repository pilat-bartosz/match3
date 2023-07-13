using Unity.Entities;
using Unity.Mathematics;

namespace _match3.Game
{
    public struct RandomSingleton : IComponentData
    {
        public Random random;
    }
}