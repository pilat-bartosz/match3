using _match3.Game;
using _match3.Grid;
using _match3.Input;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

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

        [BurstCompile]
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
                
                //Deselect old
                if (selection.ValueRO.currentSelectedEntity != Entity.Null)
                {
                    //check if new is neighbour of selected
                    var dif = math.abs(selection.ValueRO.selectionPosition - mouseGridPosition);
                    if (dif.x + dif.y == 1)
                    {
                        //switch positions
                    }
                    
                    //stop animation
                    state.EntityManager.SetComponentEnabled<IsSelected>(selection.ValueRO.currentSelectedEntity, false);
                }

                //Select new
                state.EntityManager.SetComponentEnabled<IsSelected>(entity, true);
                selection.ValueRW.currentSelectedEntity = entity;
                selection.ValueRW.selectionPosition = mouseGridPosition;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}