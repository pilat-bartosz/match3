using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _match3.Jelly.Movement
{
    [BurstCompile]
    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    public partial struct MoveToDestinationJob : IJobEntity
    {
        [ReadOnly] public float deltaTime;

        private void Execute(
            ref LocalTransform localTransform,
            ref DynamicBuffer<Destination> destination,
            in MovementSpeed movementSpeed,
            EnabledRefRW<IsMoving> isMoving)
        {
            if (destination.Length == 0) return;

            var currentDestination = destination[0];
            var distanceSquared = math.distancesq(localTransform.Position, currentDestination.position);
            var maxDistanceDelta = movementSpeed.Value * deltaTime;
            var arrived = distanceSquared == 0 || distanceSquared <= maxDistanceDelta * maxDistanceDelta;

            localTransform.Position = math.select(
                localTransform.Position + (currentDestination.position - localTransform.Position) /
                math.sqrt(distanceSquared) * maxDistanceDelta,
                currentDestination.position,
                arrived);
            
            isMoving.ValueRW = arrived;

            if (arrived)
            {
                destination.RemoveAt(0);
            }
        }
    }
}