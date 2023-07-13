using _match3.Game;
using _match3.Grid;
using _match3.GUI;
using _match3.Selection;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace _match3.Managers
{
    /// <summary>
    /// Exists to gather all singletons on the one entity to minimize chunk count
    /// </summary>
    public class SingletonsAuthoringAuthoring : MonoBehaviour
    {
        public int jellyTypeCount;
        public GameObject menuPrefab;
        public GameObject jellyPrefab;

        [Header("Game Manager Settings")]
        public uint randomSeed;
        
        [Header("Grid Settings")]
        public int2 size;
        public float2 startPosition;
        public float2 gap;

        

        private class SingletonsAuthoringBaker : Baker<SingletonsAuthoringAuthoring>
        {
            public override void Bake(SingletonsAuthoringAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.None);

                //Game state
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

                //Random
                AddComponent(entity, new RandomSingleton
                {
                    random = Random.CreateFromIndex(authoring.randomSeed)
                });
                
                //Selection
                AddComponent(entity, new SelectionSingleton());

                //Prefabs
                var jellyPrefab = GetEntity(authoring.jellyPrefab, TransformUsageFlags.Dynamic);
                AddComponent(entity, new JellyPrefabSingleton
                {
                    jellyPrefab = jellyPrefab
                });
                
                //GUI
                AddComponentObject(entity, new GUIPrefab
                {
                    guiPrefab = authoring.menuPrefab
                });

                //Game Score
                var buffer = AddBuffer<GameScoreBuffer>(entity);
                for (var i = 0; i < authoring.jellyTypeCount; i++)
                {
                    buffer.Add(new GameScoreBuffer());
                }
                
                //Grid
                AddComponent(entity, new GridSettingsSingleton
                {
                    size = authoring.size,
                    startPosition = authoring.startPosition,
                    gap = authoring.gap,
                    jellyTypeCount = authoring.jellyTypeCount
                });
                var grid = AddBuffer<GridBuffer>(entity);
                var gridArray = new NativeArray<GridBuffer>(
                    authoring.size.x * authoring.size.y, 
                    Allocator.Temp);
                grid.AddRange(gridArray);
                

            }
        }
    }
}