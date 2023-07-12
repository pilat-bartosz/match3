using Unity.Entities;

namespace _match3.Jelly.Animations
{
    /// <summary>
    /// Readonly data struct
    /// </summary>
    public struct JellyAnimationData : IComponentData
    {
        public float animationSpeed;
        public float minScale;
        public float maxScale;
    }
    
    
    public struct JellyAnimation : IComponentData
    {
        public float currentAnimationTime;
    }
    
    public struct IsAnimated : IComponentData, IEnableableComponent
    {
        
    }
}