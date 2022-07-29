using Leopotam.EcsLite;

namespace ECSLiteTest
{
    public class CharacterMoveSystem : IEcsRunSystem, IEcsInitSystem
    {
        private WorldSharedData _worldSharedData;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<CharacterComponent>()
                .Inc<MoveToTargetCommandComponent>()
                .End();

            if (filter.GetEntitiesCount() == 0) return;

            foreach (var character in filter)
            {
                MoveCharacter(character, world);
            }
        }

        private void MoveCharacter(int entity, EcsWorld world)
        {
            var characterPool = world.GetPool<CharacterComponent>();
            if (!characterPool.Has(entity)) return;

            var commandPool = world.GetPool<MoveToTargetCommandComponent>();
            if (!commandPool.Has(entity)) return;

            ref var characterData = ref characterPool.Get(entity);
            var commandData = commandPool.Get(entity);
            var moveDistance = _worldSharedData.CharacterSpeed * _worldSharedData.DeltaTime;
            var moveVector = commandData.TargetPoint - characterData.Position;
            if (moveVector.magnitude > moveDistance)
            {
                moveVector = moveVector.normalized * moveDistance;
                characterData.IsMoving = true;
            }
            else
            {
                commandPool.Del(entity);
                characterData.IsMoving = false;
            }
                
            characterData.Position += moveVector;
        }

        public void Init(IEcsSystems systems)
        {
            _worldSharedData = systems.GetShared<WorldSharedData>();
        }
    }
}