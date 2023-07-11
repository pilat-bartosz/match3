using _match3.Managers;
using Unity.Entities;

namespace _match3.GUI
{
    [UpdateInGroup(typeof(GUISystemGroup))]
    public partial class GUISystem  : SystemBase
    {
        protected override void OnUpdate()
        {

            foreach (var (guiManager, gameStateSingleton) in SystemAPI
                         .Query<GUIManager, GameStateSingleton>()
                         .WithChangeFilter<GameStateSingleton>())
            {
                guiManager.SwitchToUI(gameStateSingleton.gameState);
            }
        }
    }
}