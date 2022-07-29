using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ECSLiteTest
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private float _closedPosition;
        [SerializeField] private float _openPosition;
        [SerializeField] private float _transitionSpeed;
        [SerializeField] private Transform _doorTransform;
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
            var doorPool = _worldManager.GameWorld.GetPool<DoorComponent>();
            ref var doorData = ref doorPool.Add(_entityId);
            doorData.State = 0f;
            doorData.TransitionSpeed = _transitionSpeed;
        }

        private void UpdateState()
        {
            var doorPool = _worldManager.GameWorld.GetPool<DoorComponent>();
            if (!doorPool.Has(_entityId)) return;

            var doorData = doorPool.Get(_entityId);
            _doorTransform.localPosition = new Vector3(Mathf.Lerp(_closedPosition, _openPosition, doorData.State),
                _doorTransform.localPosition.y, _doorTransform.localPosition.z);
        }

        private void OnDestroy()
        {
            _entityMediator.DestroyEntity(gameObject);
        }
    }
}