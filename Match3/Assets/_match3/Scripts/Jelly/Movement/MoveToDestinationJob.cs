using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace _match3.Jelly.Movement
{
    [BurstCompile]
    [WithChangeFilter(typeof(Destination))]
    public partial struct MoveToDestinationJob : IJobEntity
    {
        [ReadOnly] public float deltaTime;
        
        private void Execute(
            ref LocalTransform localTransform,
            in Destination destination,
            in MovementSpeed movementSpeed
            )
        {
            localTransform.Position = MathExtensions.MoveTowards(localTransform.Position,
                destination.position, movementSpeed.Value * deltaTime);
        }
    }
}