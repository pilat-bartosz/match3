using Unity.Entities;
using Unity.Mathematics;

namespace _match3.Grid
{
    [InternalBufferCapacity(0)]
    public struct GridEntity : IBufferElementData
    {
        public Entity entity;
    }
    
    [InternalBufferCapacity(0)]
    public struct GridCell : IBufferElementData
    {
        public int type;
    }
    
    public struct GridPosition : IComponentData
    {
        public int2 position;
    }

    public struct GridSettingsSingleton : IComponentData
    {
        public float2 startPosition;
        public float2 gap;

        public int2 size;

        public int jellyTypeCount;
    }

    public struct SpawnJelliesInGrid : IComponentData
    {
        
    }

    public struct ClearGrid : IComponentData
    {
        
    }
    
    
}