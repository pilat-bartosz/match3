using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace _match3
{
    public static class MathExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 MoveTowards(float3 current, float3 target, float maxDistanceDelta)
        {
            var distanceSquared = math.distancesq(current, target);
            return math.select(
                current + (target - current) / math.sqrt(distanceSquared) * maxDistanceDelta,
                target,
                distanceSquared == 0 || distanceSquared <= maxDistanceDelta * maxDistanceDelta);
        }
    }
}