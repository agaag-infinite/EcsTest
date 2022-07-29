using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ECSLiteTest
{
    public class ObjectEntityMediator
    {
        [Inject] private WorldManager _worldManager;
        private readonly Dictionary<GameObject, int> _entityMap = new Dictionary<GameObject, int>();

        public int RegisterEntity(GameObject obj)
        {
            if (_entityMap.ContainsKey(obj)) return _entityMap[obj];

            var entityId = _worldManager.GameWorld.NewEntity();
            _entityMap.Add(obj, entityId);
            return entityId;
        }

        public bool TryGetEntity(GameObject obj, out int entityId)
        {
            if (_entityMap.ContainsKey(obj))
            {
                entityId = _entityMap[obj];
                return true;
            }

            entityId = -1;
            return false;
        }

        public void DestroyEntity(GameObject obj)
        {
            if (!_entityMap.ContainsKey(obj)) return;

            var entityId = _entityMap[obj];
            _entityMap.Remove(obj);
            _worldManager.GameWorld.DelEntity(entityId);
        }
    }
}