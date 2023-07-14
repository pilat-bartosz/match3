using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace _match3.Jelly
{
    public partial struct SpriteOffsetSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (offset, jelly, spriteData) in
                     SystemAPI.Query<RefRW<OffsetVector2Override>, RefRO<Jelly>, RefRO<SpriteData>>()
                         .WithChangeFilter<Jelly>()
                    )
            {
                offset.ValueRW.Value = new float2(
                    spriteData.ValueRO.spriteSize * jelly.ValueRO.type,
                    1 - spriteData.ValueRO.spriteSize * 2
                );
            }
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}