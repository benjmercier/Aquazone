using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Aquazone.InputActions;

namespace Aquazone.Scripts.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerRigidbodyController : MonoBehaviour, PlayerInputAction.IPlayerActions
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        private Vector3 _moveInput;
        private Vector3 _moveDirection;
        private Vector3 _targetDirection;

        [Header("Steering")]
        [SerializeField]
        private float _maxSpeed = 15f;
        [Range(0f, 5f), SerializeField]
        private float _maxForce = 0.25f;

        private Vector3 _currentVelocity;
        private Vector3 _desiredVelocity;
        private Vector3 _steeringVelocity;

        // Rigidbody Movement
        private Vector3 _linearForce = Vector3.one;
        private Vector3 _angularForce = Vector3.one;

        private float _reverseMultiplier = 1f;
        private float _forceMultiplier = 10f;

        private Vector3 _appliedLinearForce = Vector3.zero;
        private Vector3 _appliedAngularForce = Vector3.zero;

        // _moveDirection
        private float _pitch;
        private float _yaw;
        private float _roll;
        private float _strafe;
        private float _throttle;

        private float _throttleSpeed = 0.5f;

        private void Awake()
        {
            if (_rigidbody == null)
            {
                TryGetComponent(out _rigidbody);
            }
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            //_appliedLinearForce = ApplyForce(new Vector3(0f, _pitch, _throttle), _linearForce);
            //_appliedAngularForce = ApplyForce(new Vector3(0f, _yaw, 0f), _angularForce);
        }

        private void FixedUpdate()
        {
            CalculateSteering(_moveDirection);

            //_rigidbody.AddRelativeForce(_appliedLinearForce * _forceMultiplier, ForceMode.Force);
            //_rigidbody.AddRelativeTorque(_appliedAngularForce * _forceMultiplier, ForceMode.Force);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector3>();

            _moveDirection = new Vector3(_moveInput.x, _moveInput.z, _moveInput.y);

            _pitch = _moveDirection.y;
            _yaw = _moveDirection.x;
            _throttle = _moveDirection.z;
        }

        private Vector3 ApplyForce(Vector3 a, Vector3 b)
        {
            Vector3 ret;

            ret.x = a.x * b.x;
            ret.y = a.y * b.y;
            ret.z = a.z * b.z;

            return ret;
        }

        private void CalculateSteering(Vector3 direction)
        {
            _targetDirection = transform.position + direction;

            _desiredVelocity = (_targetDirection - transform.position).normalized;
            _desiredVelocity *= _maxSpeed;

            _steeringVelocity = _desiredVelocity - _currentVelocity;
            _steeringVelocity = Vector3.ClampMagnitude(_steeringVelocity, _maxForce);
            _steeringVelocity /= _rigidbody.mass;

            _currentVelocity += _steeringVelocity;
            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, _maxSpeed);

            _appliedLinearForce = new Vector3(0f, _currentVelocity.y, _currentVelocity.z); //ApplyForce(new Vector3(0f, _pitch, _throttle), _linearForce);
            _appliedAngularForce = new Vector3(0f, _currentVelocity.x, 0f);

            _rigidbody.AddRelativeForce(_appliedLinearForce, ForceMode.Force);
            _rigidbody.AddRelativeTorque(_appliedAngularForce, ForceMode.Force);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            
        }

        public void OnToggleHorizontalRotation(InputAction.CallbackContext context)
        {
            
        }
    }
}

