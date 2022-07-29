using ECSLiteTest;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<WorldManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        Container.Bind<ObjectEntityMediator>().AsSingle().NonLazy();
    }
}