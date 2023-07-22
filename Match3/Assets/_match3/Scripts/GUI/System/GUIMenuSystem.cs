using System;
using _match3.Extensions;
using _match3.Game;
using Unity.Entities;

namespace _match3.GUI
{
    [UpdateInGroup(typeof(GUISystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial class GUIMenuSystem : SystemBase
    {
        private EntityQuery _query;

        protected override void OnCreate()
        {
            _query = SystemAPI.QueryBuilder()
                .WithAll<GUIManager>()
                .WithAllRW<MenuState>()
                .Build();
            RequireForUpdate(_query);
        }
        
        protected override void OnUpdate()
        {
            var guiEntity = _query.GetSingletonEntity();
            var guiManager = _query.GetSingleton<GUIManager>();
            var menuState = _query.GetSingletonRW<MenuState>();

            switch (menuState.ValueRO.subState)
            {
                //on menu enter
                case MenuState.MenuSubState.OnEnter:
                {
                    guiManager.UpdateUI(GUIManager.GUIState.Menu);
                    
                    menuState.ValueRW.subState = MenuState.MenuSubState.Menu;

                    //cleanup game grid
                    var jellyQuery = SystemAPI.QueryBuilder().WithAll<Jelly.Jelly>().Build();
                    EntityManager.DestroyEntity(jellyQuery);
                }
                    break;
                case MenuState.MenuSubState.Menu:
                {
                    //start a new game and set score targets
                    if (guiManager.BeginMenuButtonHandling())
                    {
                        //setup score targets
                        var buttonID = guiManager.GetLastButtonClicked();
                        var scoreBuffer = SystemAPI.GetBuffer<GameScoreTargetBuffer>(guiEntity);

                        // 10 + (10 * 0 * 0) -> 10 + (10 * 3 * 3)
                        var endValue = 10 + (10 * buttonID * buttonID);
                        for (var i = 0; i < scoreBuffer.Length; i++)
                        {
                            scoreBuffer[i] = new GameScoreTargetBuffer { score = endValue };
                        }

                        //set next state to switch to
                        EntityManager.SwitchState<MenuState, GameState>(guiEntity);

                        //tell the GUI that handling was dane and it can unblock itself or whatever
                        guiManager.EndMenuButtonHandling();
                    }
                }
                    break;
                case MenuState.MenuSubState.OnExit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}