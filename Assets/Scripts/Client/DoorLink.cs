using UnityEngine;
using Zenject;

namespace ECSLiteTest
{
    public class DoorLink : MonoBehaviour
    {
        [SerializeField] private ButtonController _button;
        [SerializeField] private DoorController _door;
        private int _entityId;
        private bool _initialized;

        [Inject] private WorldManager _worldManager;
        [Inject] private ObjectEntityMediator _entityMediator;


        void Start()
        {
            RegisterLink();
        }

        void Update()
        {
            if (!_initialized) RegisterLink();
        }

        private void RegisterLink()
        {
            if (!_entityMediator.TryGetEntity(_door.gameObject, out var doorEntity)) return;
            if(!_entityMediator.TryGetEntity(_button.gameObject, out var buttonEntity)) return;

            var buttonPool = _worldManager.GameWorld.GetPool<ButtonComponent>();
            if (!buttonPool.Has(buttonEntity)) return;

            var linkPool = _worldManager.GameWorld.GetPool<DoorLinkComponent>();
            if (linkPool.Has(buttonEntity)) linkPool.Del(buttonEntity);
            ref var linkData = ref linkPool.Add(buttonEntity);
            linkData.DoorEntity = doorEntity;
            _initialized = true;
        }
    }
}