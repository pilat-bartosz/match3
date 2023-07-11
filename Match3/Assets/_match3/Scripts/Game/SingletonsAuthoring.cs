using _match3.Game;
using _match3.GUI;
using Unity.Entities;
using UnityEngine;

namespace _match3.Managers
{
    /// <summary>
    /// Exists to gather all singletons on the one entity to minimize chunk count
    /// </summary>
    public class SingletonsAuthoringAuthoring : MonoBehaviour
    {
        public int jellyTypeCount;
        public GameObject menuPrefab;

        private class SingletonsAuthoringBaker : Baker<SingletonsAuthoringAuthoring>
        {
            public override void Bake(SingletonsAuthoringAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.None);

                AddComponent(entity, new GameStateSingleton
                {
                    gameState = GameState.Menu
                });

                AddComponent(entity, new NextGameState
                {
                    nextGameState = GameState.Menu
                });

                AddComponent(entity, new SwitchGameState());
                SetComponentEnabled<SwitchGameState>(entity, false);

                AddComponentObject(entity, new GUIPrefab
                {
                    guiPrefab = authoring.menuPrefab
                });

                var buffer = AddBuffer<GameScoreBuffer>(entity);
                for (var i = 0; i < authoring.jellyTypeCount; i++)
                {
                    buffer.Add(new GameScoreBuffer());
                }
            }
        }
    }
}