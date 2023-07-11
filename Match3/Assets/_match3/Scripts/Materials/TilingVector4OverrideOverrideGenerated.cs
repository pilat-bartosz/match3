using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_Tiling")]
    struct TilingVector2Override : IComponentData
    {
        public float2 Value;
    }
}
