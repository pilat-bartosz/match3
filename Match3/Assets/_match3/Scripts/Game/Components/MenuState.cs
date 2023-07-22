using Unity.Entities;

namespace _match3.Game
{
    public struct MenuState : IComponentData
    {
        public enum MenuSubState
        {
            OnEnter,
            Menu,
            OnExit,
        }

        public MenuSubState subState;
    }
}