using Unity.Entities;

namespace _match3.Jelly.Animations
{
    /// <summary>
    /// Readonly data struct
    /// </summary>
    public struct ScaleAnimationData : IComponentData
    {
        public float animationSpeed;
        public float minScale;
        public float maxScale;
    }
    
    
    public struct ScaleAnimationTime : IComponentData
    {
        public float currentAnimationTime;
    }
}