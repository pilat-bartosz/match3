using Unity.Entities;
using Unity.Mathematics;

namespace _match3.Jelly.Movement
{
    public struct Destination : IComponentData
    {
        public float3 position;
    }
    
    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }
}