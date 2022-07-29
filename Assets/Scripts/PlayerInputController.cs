using System.Collections;
using System.Collections.Generic;
using ECSLiteTest;
using UnityEngine;
using Zenject;

namespace ECSLiteTest
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private Transform _plane;
        [SerializeField] private Camera _camera;

        [Inject] private WorldManager _worldManager;

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                var hits = Physics.RaycastAll(ray);

                foreach (var hit in hits)
                {
                    if (hit.transform == _plane)
                    {
                        var eventPool = _worldManager.GameWorld.GetPool<InputEventComponent>();
                        var inputEvent = _worldManager.GameWorld.NewEntity();
                        ref var eventData = ref eventPool.Add(inputEvent);
                        eventData.TimeStamp = Time.timeAsDouble;
                        eventData.TargetPoint = hit.point;
                    }
                }
            }
        }
    }
}