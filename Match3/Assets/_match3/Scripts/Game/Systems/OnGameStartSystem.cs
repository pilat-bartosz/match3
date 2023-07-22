using _match3.Grid;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace _match3.Game
{
    [UpdateInGroup(typeof(GameManagerGroup))]
    public partial struct OnGameStartSystem : ISystem
    {
        private EntityQuery _onGameStartQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _onGameStartQuery = SystemAPI.QueryBuilder()
                .WithAll<GameState,
                    GridSettingsSingleton, 
                    JellyPrefabSingleton>()
                .WithAllRW<GridBuffer, RandomSingleton>()
                .Build();
            _onGameStartQuery.AddChangedVersionFilter(ComponentType.ReadOnly<GameState>());

            state.RequireForUpdate(_onGameStartQuery);
            state.RequireForUpdate<EndInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameState>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //on game start
            var gameState = SystemAPI.GetSingletonRW<GameState>();
            if (gameState.ValueRO.subState != GameState.GameSubState.OnEnter) return;
            gameState.ValueRW.subState = GameState.GameSubState.Game;

            //get all the data needed
            var jellyPrefab = _onGameStartQuery.GetSingleton<JellyPrefabSingleton>().jellyPrefab;

            var gridSettings = _onGameStartQuery.GetSingleton<GridSettingsSingleton>();
            var grid = _onGameStartQuery.GetSingletonBuffer<GridBuffer>();

            var random = _onGameStartQuery.GetSingletonRW<RandomSingleton>();


            var singleton = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = singleton.CreateCommandBuffer(state.WorldUnmanaged);

            //spawn jellies
            var startPosition = gridSettings.startPosition;
            var gap = gridSettings.gap;
            var jellyTypeCount = gridSettings.jellyTypeCount;

            var jelliesCount = gridSettings.size.x * gridSettings.size.y;
            var jelliesArray = state.EntityManager.Instantiate(
                jellyPrefab,
                jelliesCount,
                Allocator.TempJob
            );

            var types = new NativeArray<int>(jelliesCount, Allocator.TempJob);
            var jobHandle = new GenerateRandomTypes
            {
                jellyTypeCount = jellyTypeCount,
                random = random,
                types = types
            }.Schedule(jelliesCount, state.Dependency);

            jobHandle = new FillJelliesJob
            {
                startPosition = startPosition,
                gap = gap,
                gridSettings = gridSettings,
                ecb = ecb.AsParallelWriter(),
                jelliesArray = jelliesArray,
                types = types,
                grid = grid
            }.Schedule(jelliesCount, jobHandle);

            jobHandle = types.Dispose(jobHandle);
            jobHandle = jelliesArray.Dispose(jobHandle);

            state.Dependency = jobHandle;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}