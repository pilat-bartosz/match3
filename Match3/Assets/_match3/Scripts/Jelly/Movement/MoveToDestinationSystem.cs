using Unity.Burst;
using Unity.Entities;
using Unity.Logging;

namespace _match3.Jelly.Movement
{
    [RequireMatchingQueriesForUpdate]
    public partial struct MoveToDestinationSystem : ISystem, ISystemStartStop
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

        [BurstCompile]
        public void OnStartRunning(ref SystemState state)
        {
            Log.Debug("OnStartRunning");
        }

        [BurstCompile]
        public void OnStopRunning(ref SystemState state)
        {
            Log.Debug("OnStopRunning");
        }
    }
}