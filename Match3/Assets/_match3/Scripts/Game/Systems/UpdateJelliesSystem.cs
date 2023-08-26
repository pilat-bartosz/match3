using _match3.Grid;
using Unity.Burst;
using Unity.Entities;

namespace _match3.Game
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(GameManagerGroup), OrderLast = true)]
    public partial struct UpdateJelliesSystem : ISystem
    {
        private EntityQuery _onGameStartQuery;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _onGameStartQuery = SystemAPI.QueryBuilder()
                .WithAll<GameState,
                    JellyPrefabSingleton>()
                .WithAspect<GridAspect>()
                .Build();
            _onGameStartQuery.AddChangedVersionFilter(ComponentType.ReadOnly<GameState>());

            state.RequireForUpdate(_onGameStartQuery);
            state.RequireForUpdate<EndInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameState>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //get all the data needed
            var jellyPrefab = _onGameStartQuery.GetSingleton<JellyPrefabSingleton>().jellyPrefab;

            var gridSettings = _onGameStartQuery.GetSingleton<GridSettingsSingleton>();
            var gridEntities = _onGameStartQuery.GetSingletonBuffer<GridEntity>();
            var gridCells = _onGameStartQuery.GetSingletonBuffer<GridCell>();

            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}