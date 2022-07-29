using Leopotam.EcsLite;
using UnityEngine;

namespace ECSLiteTest
{
    public class DoorSystem : IEcsRunSystem, IEcsInitSystem
    {
        private WorldSharedData _sharedData;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<DoorComponent>().Inc<DoorTriggerComponent>().End();
            if (filter.GetEntitiesCount() == 0) return;

            var doorPool = world.GetPool<DoorComponent>();
            foreach (var door in filter)
            {
                ref var doorData = ref doorPool.Get(door);
                var shift = doorData.TransitionSpeed * _sharedData.DeltaTime;
                doorData.State = Mathf.Min(doorData.State + shift, 1f);
            }
        }

        public void Init(IEcsSystems systems)
        {
            _sharedData = systems.GetShared<WorldSharedData>();
        }
    }
}