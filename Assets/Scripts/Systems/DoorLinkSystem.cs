using Leopotam.EcsLite;

namespace ECSLiteTest
{
    public class DoorLinkSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<ButtonComponent>().Inc<DoorLinkComponent>().End();
            if (filter.GetEntitiesCount() == 0) return;

            var buttonPool = world.GetPool<ButtonComponent>();
            var linkPool = world.GetPool<DoorLinkComponent>();
            var triggerPool = world.GetPool<DoorTriggerComponent>();

            foreach (var button in filter)
            {
                var buttonData = buttonPool.Get(button);
                var linkData = linkPool.Get(button);
                var doorEntity = linkData.DoorEntity;

                if (buttonData.Pressed)
                {
                    if (!triggerPool.Has(doorEntity)) triggerPool.Add(doorEntity);
                }
                else
                {
                    if(triggerPool.Has(doorEntity)) triggerPool.Del(doorEntity);
                }
            }
        }
    }
}