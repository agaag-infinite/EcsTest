using Leopotam.EcsLite;

namespace ECSLiteTest
{
    public class ButtonSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var characterFilter = world.Filter<CharacterComponent>().End();
            var buttonFilter = world.Filter<ButtonComponent>().End();
            var characterPool = world.GetPool<CharacterComponent>();
            var buttonPool = world.GetPool<ButtonComponent>();

            foreach (var character in characterFilter)
            {
                var characterData = characterPool.Get(character);

                foreach (var button in buttonFilter)
                {
                    ref var buttonData = ref buttonPool.Get(button);
                    var distance = (buttonData.Position - characterData.Position).magnitude;
                    buttonData.Pressed = buttonData.Radius >= distance;
                }
            }
        }
    }
}