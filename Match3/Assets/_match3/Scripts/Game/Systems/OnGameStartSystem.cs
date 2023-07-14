using _match3.Grid;
using _match3.Jelly.Movement;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _match3.Game
{
    [UpdateInGroup(typeof(GameManagerGroup))]
    public partial struct OnGameStartSystem : ISystem
    {
        private EntityQuery _onGameStartQuery;
        private EntityQuery _jellyQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _jellyQuery = SystemAPI.QueryBuilder()
                .WithAll<Jelly.Jelly>()
                .Build();

            _onGameStartQuery = SystemAPI.QueryBuilder()
                .WithAll<GameStateSingleton, GridSettingsSingleton, JellyPrefabSingleton>()
                .WithAllRW<GridBuffer, RandomSingleton>()
                .Build();
            _onGameStartQuery.AddChangedVersionFilter(ComponentType.ReadOnly<GameStateSingleton>());

            state.RequireForUpdate(_onGameStartQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (_onGameStartQuery.IsEmpty) return;

            var gameState = _onGameStartQuery.GetSingleton<GameStateSingleton>();

            //OnGameStart enter
            if (gameState.gameState != GameState.Game) return;

            //get all the data needed
            var jellyPrefab = _onGameStartQuery.GetSingleton<JellyPrefabSingleton>().jellyPrefab;

            var gridSettings = _onGameStartQuery.GetSingleton<GridSettingsSingleton>();
            var grid = _onGameStartQuery.GetSingletonBuffer<GridBuffer>();

            var random = _onGameStartQuery.GetSingletonRW<RandomSingleton>();

            //clear grid from jellies
            state.EntityManager.DestroyEntity(_jellyQuery);
            //there is no need to clear grid as it will be overriden anyway

            //spawn jellies
            var startPosition = gridSettings.startPosition;
            var gap = gridSettings.gap;
            var jellyTypeCount = gridSettings.jellyTypeCount;

            var jelliesArray = state.EntityManager.Instantiate(
                jellyPrefab,
                gridSettings.size.x * gridSettings.size.y,
                Allocator.Temp
            );

            for (var x = 0; x < gridSettings.size.x; x++)
            {
                for (var y = 0; y < gridSettings.size.y; y++)
                {
                    var entity = jelliesArray[y * gridSettings.size.x + x];

                    //set type
                    var type = random.ValueRW.random.NextInt(0, jellyTypeCount);
                    state.EntityManager.SetComponentData(entity, new Jelly.Jelly
                    {
                        type = type
                    });

                    //teleport to
                    var newPosition = new float3(
                        startPosition.x + (gap.x * x),
                        startPosition.y - (gap.y * y),
                        0f
                    );
                    state.EntityManager.SetComponentData(entity, new Destination
                    {
                        position = newPosition
                    });
                    newPosition += new float3(0f,gap.y * 2, 0f);
                    state.EntityManager.SetComponentData(entity, new LocalTransform
                    {
                        Position = newPosition,
                        Rotation = quaternion.identity,
                        Scale = 0.75f
                    });

                    //setup grid position
                    state.EntityManager.SetComponentData(entity, new GridPosition
                    {
                        position = new int2(x, y)
                    });
                    //fill the grid
                    grid[gridSettings.GetIndexFromSettings(x,y)] = new GridBuffer
                    {
                        type = type,
                        entity = entity
                    };
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}