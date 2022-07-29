using UnityEngine;
using Zenject;

namespace ECSLiteTest
{
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField] private Animator _characterAnimator;

        private int _entityId;

        [Inject] private WorldManager _worldManager;
        [Inject] private ObjectEntityMediator _entityMediator;

        void Start()
        {
            RegisterEntity();
        }

        void Update()
        {
            UpdatePosition();
        }

        private void RegisterEntity()
        {
            _entityId = _entityMediator.RegisterEntity(gameObject);
            var characterPool = _worldManager.GameWorld.GetPool<CharacterComponent>();
            ref var characterData = ref characterPool.Add(_entityId);
            characterData.Position = transform.position;
        }

        private void UpdatePosition()
        {
            var characterPool = _worldManager.GameWorld.GetPool<CharacterComponent>();
            if (!characterPool.Has(_entityId)) return;

            var characterData = characterPool.Get(_entityId);
            transform.position = characterData.Position;
            _characterAnimator.SetBool("IsMoving", characterData.IsMoving);
        }

        private void OnDestroy()
        {
            _entityMediator.DestroyEntity(gameObject);
        }
    }
}