using Leopotam.EcsLite;
using System;
using UnityEngine;

namespace ECSLiteTest
{
    public class WorldManager : MonoBehaviour, IDisposable
    {
        public EcsWorld GameWorld => _gameWorld;

        private EcsWorld _gameWorld;
        private EcsSystems _gameSystems;
        private WorldSharedData _worldSharedData;

        public WorldManager()
        {
            _worldSharedData = new WorldSharedData();
            _gameWorld = new EcsWorld();
            _gameSystems = new EcsSystems(_gameWorld, _worldSharedData);

            _gameSystems
                .Add(new PlayerInputSystem())
                .Add(new CharacterMoveSystem())
                .Add(new ButtonSystem())
                .Add(new DoorLinkSystem())
                .Add(new DoorSystem())
                .Init();
        }

        public void Update()
        {
            _worldSharedData.DeltaTime = Time.deltaTime;
            _gameSystems?.Run();
        }

        public void Dispose()
        {
            _gameSystems?.Destroy();
            _gameWorld?.Destroy();
        }
    }
}