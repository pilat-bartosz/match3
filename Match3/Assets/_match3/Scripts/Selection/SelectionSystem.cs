using _match3.Game;
using _match3.Grid;
using _match3.Input;
using _match3.Jelly.Animations;
using Unity.Burst;
using Unity.Entities;

namespace _match3.Selection
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SelectionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<InputData>();
            state.RequireForUpdate<GridBuffer>();
            state.RequireForUpdate<GridSettingsSingleton>();
            state.RequireForUpdate<GameStateSingleton>();
            state.RequireForUpdate<SelectionSingleton>();
        }

        [BurstDiscard]
        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var gameState = SystemAPI.GetSingleton<GameStateSingleton>();
            if (gameState.gameState != GameState.Game) return;

            var inputData = SystemAPI.GetSingleton<InputData>();
            var gridSettings = SystemAPI.GetSingleton<GridSettingsSingleton>();
            var grid = SystemAPI.GetSingletonBuffer<GridBuffer>();

            var selection = SystemAPI.GetSingletonRW<SelectionSingleton>();

            if (inputData.lmbWasPressed)
            {
                var mouseGridPosition = GridUtilities
                    .GetGridPositionFromPosition(inputData.mouseWorldPosition, gridSettings);

                if (!gridSettings.CheckBoundaries(mouseGridPosition)) return;
                
                var entity = grid[gridSettings.GetIndexFromSettings(mouseGridPosition)].entity;
                //Switch selection
                if (selection.ValueRO.currentSelectedEntity != Entity.Null)
                {
                    state.EntityManager.SetComponentEnabled<IsAnimated>(selection.ValueRO.currentSelectedEntity, false);
                }

                state.EntityManager.SetComponentEnabled<IsAnimated>(entity, true);
                selection.ValueRW.currentSelectedEntity = entity;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}