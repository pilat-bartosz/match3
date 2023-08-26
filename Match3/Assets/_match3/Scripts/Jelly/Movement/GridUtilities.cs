using System.Runtime.CompilerServices;
using _match3.Grid;
using Unity.Mathematics;

namespace _match3.Jelly.Movement
{
    public static partial class GridUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Destination GetDestinationFromGrid(this GridSettingsSingleton gridSettings, int2 gridPosition)
        {
            return new Destination
            {
                position = gridSettings.GridToWorldPosition(gridPosition)
            };
        }
    }
}