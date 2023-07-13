using Unity.Entities;

namespace _match3.Selection
{
    public struct SelectionSingleton : IComponentData
    {
        public Entity currentSelectedEntity;
    }
}