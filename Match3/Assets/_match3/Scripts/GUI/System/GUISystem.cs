using _match3.Game;
using Unity.Entities;
using Unity.Logging;

namespace _match3.GUI
{
    [UpdateInGroup(typeof(GUISystemGroup))]
    public partial class GUISystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<GUIManager>();
        }

        protected override void OnUpdate()
        {
            var entity = SystemAPI.ManagedAPI.GetSingletonEntity<GUIManager>();
            var guiManager = SystemAPI.ManagedAPI.GetSingleton<GUIManager>();
            
            //Update score targets
            if (guiManager.BeginMenuButtonHandling())
            {
                //add GameScoreTargetBuffer
                var buttonID = guiManager.GetLastButtonClicked();

                //TODO move it
                var scoreBuffer = EntityManager.AddBuffer<GameScoreTargetBuffer>(entity);
                    
                EntityManager.SetComponentData(entity, new NextGameState
                {
                    nextGameState = GameState.Game
                });
                EntityManager.SetComponentEnabled<SwitchGameState>(entity, true);

                const int baseValue = 10;
                // 10 + (10 * 0 * 0) -> 10 + (10 * 3 * 3) 
                var endValue = baseValue + (baseValue * buttonID * buttonID);
                scoreBuffer.Add(new GameScoreTargetBuffer { score = endValue });
                scoreBuffer.Add(new GameScoreTargetBuffer { score = endValue });
                scoreBuffer.Add(new GameScoreTargetBuffer { score = endValue });
                scoreBuffer.Add(new GameScoreTargetBuffer { score = endValue });
                scoreBuffer.Add(new GameScoreTargetBuffer { score = endValue });
                scoreBuffer.Add(new GameScoreTargetBuffer { score = endValue });
                    
                guiManager.EndMenuButtonHandling();
            }
        }
    }
}