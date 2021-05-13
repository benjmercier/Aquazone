using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Aquazone.InputActions;

namespace Aquazone.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour, PlayerInputAction.IPlayerActions
    {
        //[SerializeField]
        //private GameObject _playerPrefab;

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

        [SerializeField]
        private Rigidbody _rigidbody;
                
        [Header("Rotation")]
        [SerializeField]
        private float _bankingSpeed = 5f;
        [SerializeField]
        private float _maxBankingRot = 45f;
        private Quaternion _defaultRotation;
        //private Quaternion _prefabDefaultRotation;
        private Quaternion _bankLeft;
        private Quaternion _bankRight;
        private Quaternion _bankingRotation;

        private bool _horRotation;
               
        void Start()
        {
            if (_rigidbody == null)
            {
                TryGetComponent(out _rigidbody);
            }

            _defaultRotation = transform.rotation;
            
        }

        void Update()
        {

        }

        private void LateUpdate()
        {
            CalculateSteering(_moveDirection);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector3>();

            _moveDirection = new Vector3(_moveInput.x, _moveInput.z, _moveInput.y);
        }

        public void OnToggleHorizontalRotation(InputAction.CallbackContext context)
        {
            _horRotation = context.ReadValue<float>() == 1f;

            Debug.Log(_horRotation.ToString());
        }

        private void CalculateSteering(Vector3 direction)
        {
            _targetDirection = transform.position + direction;

            //CalculateBanking(direction.x);

            _desiredVelocity = (_targetDirection - transform.position).normalized;
            _desiredVelocity *= _maxSpeed;

            _steeringVelocity = _desiredVelocity - _currentVelocity;
            _steeringVelocity = Vector3.ClampMagnitude(_steeringVelocity, _maxForce);
            _steeringVelocity /= _rigidbody.mass;

            _currentVelocity += _steeringVelocity;
            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, _maxSpeed);

            //transform.Translate(_currentVelocity * Time.deltaTime, Space.World);
        }

        private void CalculateBanking(float axis)
        {
            if (axis > 0) // bank left
            {
                _bankLeft = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, -_maxBankingRot);

                _bankingRotation = _bankLeft;
            }
            else if (axis < 0) // bank right
            {
                _bankRight = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, _maxBankingRot);

                _bankingRotation = _bankRight;
            }
            else // default
            {
                _bankingRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z); // transform.rotation; // _defaultRotation;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, _bankingRotation, _bankingSpeed * Time.deltaTime);
        }

        private void CalculateHorizontalRotation(float axis)
        {
            _defaultRotation *= Quaternion.AngleAxis(axis, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, _defaultRotation, 50f * Time.deltaTime);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            
        }
    }
}

