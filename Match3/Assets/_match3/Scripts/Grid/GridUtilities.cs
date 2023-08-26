using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace _match3.Grid
{
    public static partial class GridUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckBoundaries(this GridSettingsSingleton gridSettings, int2 gridPosition)
        {
            return gridPosition.x >= 0
                   && gridPosition.x < gridSettings.size.x
                   && gridPosition.y >= 0
                   && gridPosition.y < gridSettings.size.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int2 WorldToGridPosition(this GridSettingsSingleton gridSettings, float2 position)
        {
            var pos = new float2
            {
                x = position.x - gridSettings.startPosition.x,
                y = math.abs(position.y - gridSettings.startPosition.y)
            };
            pos += gridSettings.gap / 2;
            var gridPosition = new int2(math.floor(pos / gridSettings.gap));
            return gridPosition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GridToWorldPosition(this GridSettingsSingleton gridSettings, int2 gridPosition)
        {
            return GridToWorldPosition(gridSettings, gridPosition.x, gridPosition.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GridToWorldPosition(this GridSettingsSingleton gridSettings, int x, int y)
        {
            return new float3(
                gridSettings.startPosition.x + gridSettings.gap.x * x,
                gridSettings.startPosition.y - gridSettings.gap.y * y,
                0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int2 GetGridPositionFromIndex(this GridSettingsSingleton gridSettings, int index)
        {
            var y = (int)math.floor((float)index / gridSettings.size.x);
            return new int2(
                index - (y * gridSettings.size.x),
                y
            );
        }

        //grid position -> index
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetIndexFromGridPosition(this GridSettingsSingleton gridSettings, int2 gridPosition)
        {
            return GetIndexFromGridPosition(gridSettings, gridPosition.x, gridPosition.y);
        }

        //index -> grid position 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetIndexFromGridPosition(this GridSettingsSingleton gridSettings, int x, int y)
        {
            return y * gridSettings.size.x + x;
        }
    }
}