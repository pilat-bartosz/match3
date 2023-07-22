using Unity.Entities;

namespace _match3.Game
{
    public struct GameState : IComponentData
    {
        public enum GameSubState
        {
            OnEnter,
            Game,
            OnExit,
        }
        public GameSubState subState;
    }
}