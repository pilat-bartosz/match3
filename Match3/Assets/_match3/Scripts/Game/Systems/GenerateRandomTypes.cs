using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

namespace _match3.Game
{
    public struct GenerateRandomTypes : IJobFor
    {
        [ReadOnly] public int jellyTypeCount;

        [NativeDisableUnsafePtrRestriction] 
        public RefRW<RandomSingleton> random;
        
        public NativeArray<int> types;

        public void Execute(int index)
        {
            types[index] = random.ValueRW.random.NextInt(0, jellyTypeCount);
        }
    }
}