using _match3.Extensions;
using _match3.Game;
using Unity.Entities;

namespace _match3.GUI
{
    [UpdateInGroup(typeof(GUISystemGroup))]
    public partial class GUICongratulationsSystem : SystemBase
    {
        private EntityQuery _query;

        protected override void OnCreate()
        {
            _query = SystemAPI.QueryBuilder()
                .WithAll<CongratulationsState, GUIManager>()
                .Build();
            RequireForUpdate(_query);
        }

        protected override void OnUpdate()
        {
            var guiEntity = _query.GetSingletonEntity();
            var guiManager = _query.GetSingleton<GUIManager>();

            //return to menu
            if (!guiManager.WasReturnToMenuButtonClicked) return;

            guiManager.WasReturnToMenuButtonClicked = false;
            EntityManager.SwitchState<GameState, MenuState>(guiEntity);
        }
    }
}