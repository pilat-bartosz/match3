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
    public partial struct OnMenuEnterSystem : ISystem
    {
        private EntityQuery _onMenuEnterQuery;
        private EntityQuery _jellyQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _jellyQuery = SystemAPI.QueryBuilder()
                .WithAll<Jelly.Jelly>()
                .Build();

            _onMenuEnterQuery = SystemAPI.QueryBuilder()
                .WithAll<GameStateSingleton>()
                .Build();
            _onMenuEnterQuery.AddChangedVersionFilter(ComponentType.ReadOnly<GameStateSingleton>());

            state.RequireForUpdate(_onMenuEnterQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (_onMenuEnterQuery.IsEmpty) return;

            var gameState = _onMenuEnterQuery.GetSingleton<GameStateSingleton>();

            //OnGameStart enter
            if (gameState.gameState != GameState.Menu) return;
            
            //clear grid from jellies
            state.EntityManager.DestroyEntity(_jellyQuery);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}