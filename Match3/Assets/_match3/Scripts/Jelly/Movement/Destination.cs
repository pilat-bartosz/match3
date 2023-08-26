using Unity.Entities;
using Unity.Mathematics;

namespace _match3.Jelly.Movement
{
    [InternalBufferCapacity(5)]
    public struct Destination : IBufferElementData
    {
        public float3 position;
    }
    
    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }
    
    public struct IsMoving : IComponentData, IEnableableComponent
    {
    }
}