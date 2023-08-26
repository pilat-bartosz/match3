using Unity.Entities;
using Unity.Mathematics;

namespace _match3.Switching
{
    public struct SwitchJellies: IComponentData
    {
        public Entity firstEntity;
        public int2 firstGridPosition;
        
        public Entity secondEntity;
        public int2 secondGridPosition;
    }
}