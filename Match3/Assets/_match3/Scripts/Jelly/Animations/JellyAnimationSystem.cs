using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _match3.Jelly.Animations
{
    public partial struct JellyAnimationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (animation, data) in SystemAPI
                         .Query<RefRW<JellyAnimation>, RefRO<JellyAnimationData>>()
                         .WithAll<IsAnimated>()
                    )
            {
                animation.ValueRW.currentAnimationTime =
                    (animation.ValueRO.currentAnimationTime + data.ValueRO.animationSpeed * deltaTime) % 1;
            }

            foreach (var animation in SystemAPI
                         .Query<RefRW<JellyAnimation>>()
                         .WithNone<IsAnimated>()
                         //.WithChangeFilter<IsAnimated>()
                    )
            {
                animation.ValueRW.currentAnimationTime = 0;
            }

            foreach (var (transform, animation, data) in SystemAPI
                         .Query<RefRW<LocalTransform>, RefRO<JellyAnimation>, RefRO<JellyAnimationData>>()
                         .WithChangeFilter<JellyAnimation>()
                    )
            {
                transform.ValueRW.Scale = math.lerp(
                    data.ValueRO.minScale,
                    data.ValueRO.maxScale,
                    math.sin(animation.ValueRO.currentAnimationTime * math.PI) 
                );
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}