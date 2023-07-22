using _match3.Grid;
using _match3.Jelly.Movement;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace _match3.Game
{
    public struct FillJelliesJob : IJobFor
    {
        [ReadOnly] public float2 startPosition;
        [ReadOnly] public float2 gap;
        [ReadOnly] public GridSettingsSingleton gridSettings;

        [ReadOnly] public NativeArray<int> types;

        public NativeArray<Entity> jelliesArray;
        public DynamicBuffer<GridBuffer> grid;

        public EntityCommandBuffer.ParallelWriter ecb;

        public void Execute(int index)
        {
            var entity = jelliesArray[index];

            //y * gridSettings.size.x + x
            var y = (int)math.floor((float)index / gridSettings.size.x);
            var x = index - (y * gridSettings.size.x);

            //set type
            var type = types[index];
            ecb.SetComponent(index, entity, new Jelly.Jelly
            {
                type = type
            });

            //teleport to
            var newPosition = new float3(
                startPosition.x + (gap.x * x),
                startPosition.y - (gap.y * y),
                0f
            );
            ecb.SetComponent(index, entity, new Destination
            {
                position = newPosition
            });
            newPosition += new float3(0f, gap.y * 2, 0f);
            ecb.SetComponent(index, entity, new LocalTransform
            {
                Position = newPosition,
                Rotation = quaternion.identity,
                Scale = 0.75f
            });

            //setup grid position
            ecb.SetComponent(index, entity, new GridPosition
            {
                position = new int2(x, y)
            });
            //fill the grid
            grid[index] = new GridBuffer
            {
                type = type,
                entity = entity
            };
        }
    }
}