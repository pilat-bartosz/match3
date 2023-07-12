using Unity.Burst;
using Unity.Entities;

namespace _match3.Jelly.Movement
{
    [RequireMatchingQueriesForUpdate]
    public partial struct MoveToDestinationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new MoveToDestinationJob
            {
                deltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}