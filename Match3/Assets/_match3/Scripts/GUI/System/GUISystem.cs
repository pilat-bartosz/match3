using System.Threading;
using _match3.Game;
using Unity.Entities;

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

            //Start a new game and set score targets
            if (guiManager.BeginMenuButtonHandling())
            {
                //Setup score targets
                var buttonID = guiManager.GetLastButtonClicked();
                var scoreBuffer = SystemAPI.GetBuffer<GameScoreTargetBuffer>(entity);

                // 10 + (10 * 0 * 0) -> 10 + (10 * 3 * 3) 
                var endValue = 10 + (10 * buttonID * buttonID);
                for (var i = 0; i < scoreBuffer.Length; i++)
                {
                    scoreBuffer[i] = new GameScoreTargetBuffer { score = endValue };
                }

                //set next state to switch to
                EntityManager.SetComponentData(entity, new NextGameState
                {
                    nextGameState = GameState.Game
                });
                EntityManager.SetComponentEnabled<SwitchGameState>(entity, true);

                //Tell the GUI that handling was dane and it can unblock itself or whatever
                guiManager.EndMenuButtonHandling();
            }

            //return to menu
            if (guiManager.WasReturnToMenuButtonClicked)
            {
                guiManager.WasReturnToMenuButtonClicked = false;
                
                //set next state to switch to
                EntityManager.SetComponentData(entity, new NextGameState
                {
                    nextGameState = GameState.Menu
                });
                EntityManager.SetComponentEnabled<SwitchGameState>(entity, true);
            }
        }
    }
}