using _match3.Grid;
using _match3.Jelly.Animations;
using _match3.Jelly.Movement;
using _match3.Selection;
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

        [Header("Movement")]
        public float movementSpeed = 1f;
        
        [Header("Animations")]
        public float animationSpeed;
        public float minScale;
        public float maxScale;

        public class JellyBaker : Baker<JellyAuthoring>
        {
            public override void Bake(JellyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                //Core
                AddComponent(entity, new Jelly
                {
                    type = authoring.type
                });
                
                //Visuals
                AddComponent(entity, new SpriteData
                {
                    spriteSize = authoring.spriteSize
                });
                AddComponent(entity, new TilingVector2Override
                {
                    Value = authoring.spriteSize
                });
                AddComponent(entity, new OffsetVector2Override
                {
                    Value = new float2(authoring.spriteSize * authoring.type, 1 - authoring.spriteSize * 2)
                });

                //Grid
                AddComponent(entity, new GridPosition());

                //Movement
                AddComponent(entity, new Destination());
                AddComponent(entity, new MovementSpeed
                {
                    Value = authoring.movementSpeed
                });

                //Animations
                AddComponent(entity, new ScaleAnimationTime());
                AddComponent(entity, new ScaleAnimationData
                {
                    animationSpeed = authoring.animationSpeed,
                    minScale = authoring.minScale,
                    maxScale = authoring.maxScale
                });
                AddComponent(entity, new IsSelected());
                SetComponentEnabled<IsSelected>(entity, false);
            }
        }
    }
}