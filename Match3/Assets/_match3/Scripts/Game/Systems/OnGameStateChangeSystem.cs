using Unity.Burst;
using Unity.Entities;

namespace _match3.Game
{
    [UpdateInGroup(typeof(GameManagerGroup))]
    public partial struct OnGameStateChangeSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //Change game state
            foreach (var (gameState,
                         switchGameState,
                         nextGameState
                         ) in SystemAPI.Query<
                         RefRW<GameStateSingleton>,
                         EnabledRefRW<SwitchGameState>,
                         RefRO<NextGameState>
                     >()
                    )
            {
                //Change state
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