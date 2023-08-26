using _match3.Grid;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace _match3.Switching
{
    /// <summary>
    /// This system checks if switch leads to removal
    /// If bad it play switch&back animation
    /// If good, switches and removes jellies
    /// </summary>
    public partial struct GridUpdateSystem : ISystem
    {
        private EntityQuery _gridQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _gridQuery = SystemAPI.QueryBuilder()
                .WithAspect<GridAspect>()
                .Build();

            state.RequireForUpdate(_gridQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var grid = SystemAPI.GetAspect<GridAspect>(_gridQuery.GetSingletonEntity());

            var ecb = new EntityCommandBuffer(Allocator.Temp, PlaybackPolicy.SinglePlayback);
            foreach (var (switchJellies, entity) in SystemAPI.Query<SwitchJellies>().WithEntityAccess())
            {
                
                
                
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            
            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}