using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace _match3.Grid
{
    /// <summary>
    /// Aspect to contain grid code 
    /// </summary>
    public readonly partial struct GridAspect : IAspect
    {
        public readonly Entity gridEntity;
        
        private readonly RefRO<GridSettingsSingleton> gridSettings;
        private readonly DynamicBuffer<GridEntity> grid;
        private readonly DynamicBuffer<GridCell> gridCells;
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CheckBoundaries(int2 gridPosition)
        {
            return gridSettings.ValueRO.CheckBoundaries(gridPosition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity EntityFromGrid(int2 gridPosition)
        {
            return grid[gridSettings.ValueRO.GetIndexFromGridPosition(gridPosition)].entity;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float3 GridToWorldPosition(int2 gridPosition)
        {
            return gridSettings.ValueRO.GridToWorldPosition(gridPosition);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int2 WorldToGridPosition(float2 worldPosition)
        {
            return gridSettings.ValueRO.WorldToGridPosition(worldPosition);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CheckSwitch(int2 firstGridPosition, int2 secondGridPosition)
        {
            Assert.IsTrue(CheckBoundaries(firstGridPosition));
            Assert.IsTrue(CheckBoundaries(secondGridPosition));
            
            var firstIndex = gridSettings.ValueRO.GetIndexFromGridPosition(firstGridPosition);
            var secondIndex = gridSettings.ValueRO.GetIndexFromGridPosition(secondGridPosition);

            var ySize = gridSettings.ValueRO.size.x;
            
            //TODO can I check it in some other way instead of making a copy?
            //make a copy
            var gridTypes = gridCells.ToNativeArray(Allocator.Temp).Reinterpret<int>();
            
            //swap places
            (gridTypes[firstIndex], gridTypes[secondIndex]) = (gridTypes[secondIndex], gridTypes[firstIndex]);
            
            //check
            for (int x = 0; x < 3; x++)
            {
                //check first index with second type
                var horizontal = new int3
                {
                    x = gridTypes[gridSettings.ValueRO.GetIndexFromGridPosition(firstGridPosition-new int2(-2,0))],
                    y = gridTypes[gridSettings.ValueRO.GetIndexFromGridPosition(firstGridPosition-new int2(-1,0))],
                    z = gridTypes[firstIndex]
                };
                var vertical = new int3
                {
                    x = gridTypes[gridSettings.ValueRO.GetIndexFromGridPosition(firstGridPosition-new int2(0,-2))],
                    y = gridTypes[gridSettings.ValueRO.GetIndexFromGridPosition(firstGridPosition-new int2(0,-1))],
                    z = gridTypes[firstIndex]
                };
            }
            


            gridTypes.Dispose();
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CheckSwitch2(int2 firstGridPosition, int2 secondGridPosition)
        {
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwitchCells(int2 firstGridPosition, int2 secondGridPosition)
        {
        }
    }
}