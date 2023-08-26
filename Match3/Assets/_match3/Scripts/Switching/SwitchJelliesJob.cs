using _match3.Grid;
using _match3.Jelly.Movement;
using Unity.Burst;
using Unity.Entities;

namespace _match3.Switching
{
    [BurstCompile]
    public partial struct SwitchJelliesJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public GridAspect grid;

        private void Execute(
            Entity entity,
            int sort,
            SwitchJellies switchJellies)
        {
            //Check is switch is good or bad
            if (grid.CheckSwitch(switchJellies.firstGridPosition, switchJellies.secondGridPosition))
            {
                //if it is good then 
                //destroy
                //move down and spawn new
                //repeat until there is nothing to destroy
            }
            else
            {
                //if switch is wrong - play animation
                var firstDestination = new Destination
                {
                    position = grid.GridToWorldPosition(switchJellies.firstGridPosition)
                };
                var secondDestination = new Destination
                {
                    position = grid.GridToWorldPosition(switchJellies.secondGridPosition)
                };
                    
                //add animation as destinations
                ecb.AppendToBuffer(sort, switchJellies.firstEntity, secondDestination);
                ecb.AppendToBuffer(sort, switchJellies.firstEntity, firstDestination);
                    
                ecb.AppendToBuffer(sort, switchJellies.secondEntity, firstDestination);
                ecb.AppendToBuffer(sort, switchJellies.secondEntity, secondDestination);
            }
            
            ecb.DestroyEntity(sort, entity);
        }
    }
}