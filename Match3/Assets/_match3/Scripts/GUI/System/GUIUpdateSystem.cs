using _match3.Game;
using Unity.Entities;

namespace _match3.GUI
{
    [UpdateInGroup(typeof(GUISystemGroup))]
    public partial class GUIUpdateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            //update points
            foreach (var (guiManager, gameScoreBuffer, gameScoreTargetBuffer) in SystemAPI
                         .Query<GUIManager,
                             DynamicBuffer<GameScoreBuffer>,
                             DynamicBuffer<GameScoreTargetBuffer>
                         >()
                         .WithChangeFilter<GameScoreBuffer>())
            {
                guiManager.UpdateScore(
                    gameScoreBuffer.Reinterpret<int>().AsNativeArray(),
                    gameScoreTargetBuffer.Reinterpret<int>().AsNativeArray()
                );
            }

            //update gui
            foreach (var (guiManager, gameStateSingleton) in SystemAPI
                         .Query<GUIManager, RefRO<GameStateSingleton>>()
                         .WithChangeFilter<GameStateSingleton>())
            {
                guiManager.SwitchToUI(gameStateSingleton.ValueRO.gameState);
            }
        }
    }
}