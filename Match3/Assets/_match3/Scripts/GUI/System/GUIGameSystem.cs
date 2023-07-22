using _match3.Extensions;
using _match3.Game;
using Unity.Entities;

namespace _match3.GUI
{
    [UpdateInGroup(typeof(GUISystemGroup))]
    public partial class GUIGameSystem : SystemBase
    {
        private EntityQuery _query;

        protected override void OnCreate()
        {
            _query = SystemAPI.QueryBuilder()
                .WithAll<
                    GameState,
                    GUIManager,
                    GameScoreBuffer,
                    GameScoreTargetBuffer
                >()
                .Build();
            RequireForUpdate(_query);
        }

        protected override void OnUpdate()
        {
            var guiEntity = _query.GetSingletonEntity();
            var guiManager = _query.GetSingleton<GUIManager>();

            var gameScoreBuffer = _query.GetSingletonBuffer<GameScoreBuffer>();
            var gameScoreTargetBuffer = _query.GetSingletonBuffer<GameScoreTargetBuffer>();

            guiManager.UpdateUI(GUIManager.GUIState.Game);

            //return to menu
            if (guiManager.WasReturnToMenuButtonClicked)
            {
                guiManager.WasReturnToMenuButtonClicked = false;
                EntityManager.SwitchState<GameState, MenuState>(guiEntity);
                return;
            }

            //update points
            guiManager.UpdateScore(
                gameScoreBuffer.Reinterpret<int>().AsNativeArray(),
                gameScoreTargetBuffer.Reinterpret<int>().AsNativeArray()
            );
        }
    }
}