using Unity.Entities;

namespace _match3.Game
{
    public enum GameState
    {
        Menu,
        Game,
        Congratulations,
    }
    
    public struct GameStateSingleton : IComponentData
    {
        public GameState gameState;
        public GameState nextGameState;
    }
    
    public struct SwitchGameState : IComponentData, IEnableableComponent
    {
    }
    
    public struct NextGameState : IComponentData
    {
        public GameState nextGameState;
    }
}