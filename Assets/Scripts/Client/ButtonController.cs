using UnityEngine;
using Zenject;

namespace ECSLiteTest
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private float _radius;

        private int _entityId;

        [Inject] private WorldManager _worldManager;
        [Inject] private ObjectEntityMediator _entityMediator;

        void Start()
        {
            RegisterEntity();
        }

        void Update()
        {
            UpdateState();
        }

        private void RegisterEntity()
        {
            _entityId = _entityMediator.RegisterEntity(gameObject);
            var buttonPool = _worldManager.GameWorld.GetPool<ButtonComponent>();
            ref var buttonData = ref buttonPool.Add(_entityId);
            buttonData.Position = transform.position;
            buttonData.Radius = _radius;
            buttonData.Pressed = false;
        }

        private void UpdateState()
        {
            var buttonPool = _worldManager.GameWorld.GetPool<ButtonComponent>();
            if (!buttonPool.Has(_entityId)) return;

            var buttonData = buttonPool.Get(_entityId);
            transform.localScale = new Vector3(1, buttonData.Pressed ? 0.1f : 0.2f, 1);
            //button animation/sound
        }

        private void OnDestroy()
        {
            _entityMediator.DestroyEntity(gameObject);
        }
    }
}