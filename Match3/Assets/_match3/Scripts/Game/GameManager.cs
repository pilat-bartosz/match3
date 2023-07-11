using Unity.Burst;
using Unity.Entities;

namespace _match3.Game
{
    public partial struct GameManager : ISystem
    {

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //Change game state
            foreach (var (gameState, switchGameState, nextGameState) in SystemAPI
                         .Query<
                             RefRW<GameStateSingleton>,
                             EnabledRefRW<SwitchGameState>,
                             RefRO<NextGameState>
                         >()
                    )
            {
                gameState.ValueRW.gameState = nextGameState.ValueRO.nextGameState;
                switchGameState.ValueRW = false;
            }
            
            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}