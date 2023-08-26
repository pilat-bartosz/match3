using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Entities;
using Unity.Mathematics;

namespace _match3.Selection
{
    public static class SelectionSingletonExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeselectEntity(this RefRW<SelectionSingleton> selection, ref SystemState state)
        {
            Assert.IsTrue(state.EntityManager.HasComponent<IsSelected>(selection.ValueRW.entity));

            state.EntityManager.SetComponentEnabled<IsSelected>(selection.ValueRW.entity, false);
            selection.ValueRW.entity = Entity.Null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectEntity(this RefRW<SelectionSingleton> selection, ref SystemState state,
            Entity newEntity, int2 gridPosition)
        {
            Assert.IsTrue(state.EntityManager.HasComponent<IsSelected>(newEntity));

            state.EntityManager.SetComponentEnabled<IsSelected>(newEntity, true);
            selection.ValueRW.entity = newEntity;
            selection.ValueRW.gridPosition = gridPosition;
        }
    }
}