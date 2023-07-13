using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _match3.Jelly.Animations
{
    public partial struct JellyAnimationSystem : ISystem
    {
        private EntityQuery _resetAnimationTimeQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            //TODO remove query when .WithNone<T>().WithDisabled<T>() will be fixed by Unity
            _resetAnimationTimeQuery = SystemAPI.QueryBuilder()
                .WithAll<JellyAnimation>()
                .WithDisabled<IsAnimated>()
                .Build();
            _resetAnimationTimeQuery.AddOrderVersionFilter();
            _resetAnimationTimeQuery.AddChangedVersionFilter(ComponentType.ReadOnly<IsAnimated>());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var jobHande = new ProgressAnimationTimeJob
            {
                deltaTime = SystemAPI.Time.DeltaTime
            }.Schedule(state.Dependency);

            jobHande = new ResetAnimationTimeJob().Schedule(_resetAnimationTimeQuery, jobHande);

            jobHande = new AnimationJob().Schedule(jobHande);

            state.Dependency = jobHande;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }

    [BurstCompile]
    [WithAll(typeof(IsAnimated))]
    public partial struct ProgressAnimationTimeJob : IJobEntity
    {
        [ReadOnly] public float deltaTime;

        private void Execute(ref JellyAnimation animation, in JellyAnimationData data)
        {
            animation.currentAnimationTime =
                (animation.currentAnimationTime + data.animationSpeed * deltaTime) % 1;
        }
    }

    [BurstCompile]
    public partial struct ResetAnimationTimeJob : IJobEntity
    {
        private void Execute(ref JellyAnimation animation)
        {
            animation.currentAnimationTime = 0;
        }
    }

    [BurstCompile]
    [WithChangeFilter(typeof(JellyAnimation))]
    public partial struct AnimationJob : IJobEntity
    {
        private void Execute(ref LocalTransform transform, in JellyAnimation animation, in JellyAnimationData data)
        {
            transform.Scale = math.lerp(
                data.minScale,
                data.maxScale,
                math.sin(animation.currentAnimationTime * math.PI)
            );
        }
    }
}