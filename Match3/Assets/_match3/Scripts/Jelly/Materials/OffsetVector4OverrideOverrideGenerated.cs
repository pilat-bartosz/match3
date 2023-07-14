using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_Offset")]
    struct OffsetVector2Override : IComponentData
    {
        public float2 Value;
    }
}
