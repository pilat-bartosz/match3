using System.Runtime.CompilerServices;
using Unity.Entities;

namespace _match3.Extensions
{
    public static class EntityManagerExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwitchState<TOld, TNew>(this EntityManager entityManager, Entity entity)
            where TOld : IComponentData
            where TNew : IComponentData
        {
            entityManager.RemoveComponent<TOld>(entity);
            entityManager.AddComponent<TNew>(entity);
        }
    }
}