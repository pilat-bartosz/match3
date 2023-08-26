using _match3.Game;
using _match3.Grid;
using _match3.Input;
using _match3.Switching;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace _match3.Selection
{
    /// <summary>
    /// Handles selecting and detects a switch
    /// </summary>
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SelectionSystem : ISystem
    {
        private EntityQuery _gridQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameState>();
            
            state.RequireForUpdate<InputData>();
            state.RequireForUpdate<SelectionSingleton>();

            _gridQuery = SystemAPI.QueryBuilder()
                .WithAspect<GridAspect>()
                .Build();

            state.RequireForUpdate(_gridQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var inputData = SystemAPI.GetSingleton<InputData>();

            //There was no input so there is nothing to do
            if (!inputData.lmbWasPressed) return;

            var selection = SystemAPI.GetSingletonRW<SelectionSingleton>();
            var grid = SystemAPI.GetAspect<GridAspect>(_gridQuery.GetSingletonEntity());

            var mouseGridPosition = grid.WorldToGridPosition(inputData.mouseWorldPosition);

            //if something is selected
            if (selection.ValueRO.entity != Entity.Null)
            {
                //DESELECT
                //if out of bounds or same entity
                if (!grid.CheckBoundaries(mouseGridPosition)
                    || mouseGridPosition.Equals(selection.ValueRO.gridPosition))
                {
                    selection.DeselectEntity(ref state);
                    return;
                }
                
                //check for out of bounds is above
                var mouseEntity = grid.EntityFromGrid(mouseGridPosition);
                
                //if inside of bounds but far away from current selection
                if (math.csum(math.abs(mouseGridPosition - selection.ValueRO.gridPosition)) > 1)
                {
                    //deselect old one
                    selection.DeselectEntity(ref state);
                    selection.SelectEntity(ref state,mouseEntity, mouseGridPosition);
                    return;
                }
                
                //if everything is good - initialize switch
                //SWITCH 
                var switchComponent = new SwitchJellies
                {
                    firstGridPosition = mouseGridPosition,
                    firstEntity = mouseEntity,
                    secondGridPosition = selection.ValueRO.gridPosition,
                    secondEntity = selection.ValueRO.entity
                };

                //deselect current selection
                selection.DeselectEntity(ref state);
                
                //create entity and add switch component to it
                var switchSingletonEntity = state.EntityManager.CreateEntity();
                state.EntityManager.AddComponentData(switchSingletonEntity, switchComponent);
            }
            else
            //if no entity is selected
            //SELECT
            {
                //check if selection is inside of bounds 
                if (grid.CheckBoundaries(mouseGridPosition))
                {
                    selection.SelectEntity(ref state, grid.EntityFromGrid(mouseGridPosition), mouseGridPosition);
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}