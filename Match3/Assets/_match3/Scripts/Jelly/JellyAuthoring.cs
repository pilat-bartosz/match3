using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace _match3.Jelly
{
    public class JellyAuthoring : MonoBehaviour
    {
        public float spriteSize = 0f;
        [Range(0, 5)]
        public int type = 0;

        public class JellyBaker : Baker<JellyAuthoring>
        {
            public override void Bake(JellyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                var jelly = new Jelly
                {
                    type = authoring.type
                };

                AddComponent(entity, jelly);

                AddComponent(entity, new TilingVector2Override
                {
                    Value = authoring.spriteSize
                });

                AddComponent(entity, new OffsetVector2Override
                {
                    Value = new float2(authoring.spriteSize * authoring.type, 1 - authoring.spriteSize*2)
                });
            }
        }
    }
}