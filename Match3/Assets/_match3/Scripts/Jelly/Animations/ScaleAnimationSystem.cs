using _match3.Selection;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _match3.Jelly.Animations
{
    public partial struct ScaleAnimationSystem : ISystem
    {
        private EntityQuery _resetAnimationTimeQuery;
        
        private EntityQuery _animateQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            //TODO remove query when .WithNone<T>().WithDisabled<T>() will be fixed by Unity
            //TODO is IsSelected wrong component?
            //TODO should there be a mirror components like i.e. IsAnimated for sake of modularity?
            _resetAnimationTimeQuery = SystemAPI.QueryBuilder()
                .WithAll<ScaleAnimationTime>()
                .WithDisabled<IsSelected>()
                .Build();
            _resetAnimationTimeQuery.AddOrderVersionFilter();
            _resetAnimationTimeQuery.AddChangedVersionFilter(ComponentType.ReadOnly<IsSelected>());

            _animateQuery = SystemAPI.QueryBuilder()
                .WithAll<ScaleAnimationTime, ScaleAnimationData>()
                .WithAllRW<LocalTransform>()
                .Build();
            _animateQuery.AddOrderVersionFilter();
            _animateQuery.AddChangedVersionFilter(ComponentType.ReadOnly<ScaleAnimationTime>());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var jobHande = new ProgressAnimationTimeJob
            {
                deltaTime = SystemAPI.Time.DeltaTime
            }.Schedule(state.Dependency);

            jobHande = new ResetAnimationTimeJob().Schedule(_resetAnimationTimeQuery, jobHande);

            jobHande = new AnimationJob().Schedule(_animateQuery, jobHande);

            state.Dependency = jobHande;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }

    [BurstCompile]
    [WithAll(typeof(IsSelected))]
    public partial struct ProgressAnimationTimeJob : IJobEntity
    {
        [ReadOnly] public float deltaTime;

        private void Execute(ref ScaleAnimationTime animationTime, in ScaleAnimationData data)
        {
            animationTime.currentAnimationTime =
                (animationTime.currentAnimationTime + data.animationSpeed * deltaTime) % 1;
        }
    }

    [BurstCompile]
    public partial struct ResetAnimationTimeJob : IJobEntity
    {
        private void Execute(ref ScaleAnimationTime animationTime)
        {
            animationTime.currentAnimationTime = 0;
        }
    }

    [BurstCompile]
    public partial struct AnimationJob : IJobEntity
    {
        private void Execute(ref LocalTransform transform, in ScaleAnimationTime animationTime, in ScaleAnimationData data)
        {
            transform.Scale = math.lerp(
                data.minScale,
                data.maxScale,
                math.sin(animationTime.currentAnimationTime * math.PI)
            );
        }
    }
}