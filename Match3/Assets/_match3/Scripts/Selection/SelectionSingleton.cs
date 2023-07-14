using Unity.Entities;
using Unity.Mathematics;

namespace _match3.Selection
{
    public struct SelectionSingleton : IComponentData
    {
        public Entity currentSelectedEntity;
        public int2 selectionPosition;
    }
}