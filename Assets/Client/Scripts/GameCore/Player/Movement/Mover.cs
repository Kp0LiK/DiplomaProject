using Client.Scripts.Data.Player;
using UnityEngine;

namespace Client
{
    public class Mover : MonoBehaviour
    {
        private float _speed;
        [SerializeField] private float _gravity;
        [SerializeField] private float _smoothTime;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundDistance;
        [SerializeField] private LayerMask _groundMask;

        private PlayerData _playerData;
        private CharacterController _characterController;
        private Animator _animator;

        public PlayerData PlayerData => _playerData;

        private Vector3 _velocity;
        private bool _isGrounded;
        private float _smooth;
        private static readonly int Speed = Animator.StringToHash("Run");

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            //_speed = _playerData.WalkSpeed;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E))
            {
                _animator.SetBool("isAttack", true);
            }
            else
            {
                _animator.SetBool("isAttack", false);
            }

            Move();
        }

        private void Move()
        {
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var direction = transform.right * horizontal + transform.forward * vertical;

            if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else
            {
                if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
                {
                    Run();
                }
                else
                {
                    if (direction == Vector3.zero)
                    {
                        Idle();
                    }
                }
            }

            /*if (direction.magnitude >= 0.1f)
            {
                var rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle,
                    ref _smooth, _smoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                var move = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
                _characterController.Move(move.normalized * _speed * Time.deltaTime);
            }*/
            direction = (Vector3.right * horizontal + Vector3.forward * vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                    Camera.main.transform.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                    ref _smooth, _smoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _characterController.Move(moveDir.normalized * _speed * Time.deltaTime);
            }

            _velocity.y += _gravity * Time.deltaTime;

            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void Idle()
        {
            _speed = 0f;
            _animator.SetFloat(Speed, 0f);
        }

        private void Walk()
        {
            _speed = _playerData.WalkSpeed;
            _animator.SetFloat(Speed, 0.4f);
        }

        private void Run()
        {
            _speed = _playerData.RunSpeed;
            _animator.SetFloat(Speed, 1f);
        }
    }
}