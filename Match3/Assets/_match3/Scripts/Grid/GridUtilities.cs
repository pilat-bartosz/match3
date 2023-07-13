using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace _match3.Grid
{
    public static class GridUtilities
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
        public static int GetIndexFromSettings(this GridSettingsSingleton gridSettings, int2 gridPosition)
        {
            return GetIndexFromSettings(gridSettings, gridPosition.x, gridPosition.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetIndexFromSettings(this GridSettingsSingleton gridSettings, int x, int y)
        {
            return y * gridSettings.size.x + x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int2 GetGridPositionFromPosition(float2 position, in GridSettingsSingleton gridSettings)
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
        public static float3 GetPositionFromGrid(int2 gridPosition, in GridSettingsSingleton gridSettings)
        {
            return GetPositionFromGrid(gridPosition.x, gridPosition.y, gridSettings);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetPositionFromGrid(int x, int y, in GridSettingsSingleton gridSettings)
        {
            return new float3(
                gridSettings.startPosition.x + gridSettings.gap.x * x,
                gridSettings.startPosition.y - gridSettings.gap.y * y,
                0);
        }
    }
}