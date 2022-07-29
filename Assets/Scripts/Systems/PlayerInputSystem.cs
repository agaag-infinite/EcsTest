using Leopotam.EcsLite;
using UnityEngine;

namespace ECSLiteTest
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var filter = world.Filter<InputEventComponent>().End();
            if (filter.GetEntitiesCount() == 0) return;

            var eventPool = world.GetPool<InputEventComponent>();

            InputEventComponent lastEvent;
            lastEvent.TimeStamp = double.MinValue;
            lastEvent.TargetPoint = Vector3.zero;

            foreach (var inputEvent in filter)
            {
                var eventData = eventPool.Get(inputEvent);
                Debug.Log(eventData.TimeStamp + " " + eventData.TargetPoint);
                if (eventData.TimeStamp > lastEvent.TimeStamp) lastEvent = eventData;
                world.DelEntity(inputEvent);
            }
            Debug.Log("input event " + lastEvent.TargetPoint);
            SendCommands(world, lastEvent.TargetPoint);
        }

        private void SendCommands(EcsWorld world, Vector3 targetPoint)
        {
            var filter = world.Filter<CharacterComponent>().End();
            if (filter.GetEntitiesCount() == 0) return;

            var charactersPool = world.GetPool<CharacterComponent>();
            var commandPool = world.GetPool<MoveToTargetCommandComponent>();
            foreach (var character in filter)
            {
                if (!commandPool.Has(character)) commandPool.Add(character);
                ref var command = ref commandPool.Get(character);
                command.TargetPoint = targetPoint;
            }
        }
    }
}