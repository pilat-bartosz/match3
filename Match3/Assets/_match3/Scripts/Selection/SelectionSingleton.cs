using Unity.Entities;
using Unity.Mathematics;

namespace _match3.Selection
{
    /// <summary>
    /// Keeps data of currently selected entity.
    /// If entity == Entity.Null then gridPosition is invalid
    /// </summary>
    public struct SelectionSingleton : IComponentData
    {
        public Entity entity;
        public int2 gridPosition;
    }
}