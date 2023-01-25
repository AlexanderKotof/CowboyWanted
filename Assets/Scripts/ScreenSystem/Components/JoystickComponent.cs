using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Components
{
    public class JoystickComponent : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public Transform joystick;

        public float MovementRange = 100;
        private Vector3 _startPos;

        public event Action<Vector2> JoystickInput;

        private void Start()
        {
            _startPos = joystick.transform.position;
        }

        public void OnDrag(PointerEventData data)
        {
            var delta = new Vector3(data.position.x, data.position.y) - _startPos;
            delta = Vector3.ClampMagnitude(delta, MovementRange);

            joystick.transform.position = _startPos + delta;
            UpdateInput(delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            joystick.transform.position = _startPos;
            UpdateInput(Vector3.zero);
        }

        private void UpdateInput(Vector3 input)
        {
            JoystickInput?.Invoke(input);
        }
    }
}