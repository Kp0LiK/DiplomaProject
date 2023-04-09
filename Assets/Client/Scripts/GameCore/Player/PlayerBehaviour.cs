using System;
using Client.Scripts.Data.Player;
using UnityEngine;

namespace Client
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [Header("Controller")]
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private float _gravity;
        [SerializeField] private float _smoothTime;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundDistance;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private PlayerAudioData _audioData;

        [Header("Weapons")] 
        [SerializeField] private GameObject _bow;

        private Animator _animator;
        private CharacterController _characterController;
        private AudioSource _audioSource;


        private float _speed;
        private float _health;
        private float _stamina;
        private float _smooth;
        private bool _isGrounded;
        private bool _isAim;
        private float _timerToAim;
        private float _timeBetweenChangeAnimation;
        private Vector3 _velocity;
        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        private static readonly int Speed = Animator.StringToHash("Run");
        private static readonly int IsDie = Animator.StringToHash("isDie");
        private static readonly int IsAim = Animator.StringToHash("isAim");

        public float Health
        {
            get => _health;
            set
            {
                HealthChanged?.Invoke(value);
                _health = value;
                if (_health >= _playerData.Health)
                {
                    _health = _playerData.Health;
                }
            }
        }

        public float Stamina
        {
            get => _stamina;
            set
            {
                StaminaChanged?.Invoke(value);
                _stamina = value;
                if (_stamina >= _playerData.Stamina)
                {
                    _stamina = _playerData.Stamina;
                }

                if (_stamina <= 0)
                {
                    _stamina = 0;
                    Walk();
                }
            }
        }

        public event Action<float> HealthChanged;
        public event Action<float> StaminaChanged;

        public AudioSource AudioSource => _audioSource;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _speed = _playerData.WalkSpeed;
            _health = _playerData.Health;
            _stamina = _playerData.Stamina;
            _timerToAim = 3f;
        }

        private void Update()
        {
            Debug.Log(_stamina);
            _animator.SetBool(IsAttack, Input.GetKey(KeyCode.E));
            if (Input.GetKey(KeyCode.F))
            {
                Health = 0f;
                _animator.SetBool(IsDie, true);
                Destroy(gameObject, 5);
            }

            Move();

            if (_isAim == true)
            {
                _timeBetweenChangeAnimation -= Time.deltaTime;
                if (_timeBetweenChangeAnimation <= 0)
                {
                    _isAim = false;
                    //_bow.gameObject.SetActive(false);
                    _animator.SetBool(IsAim, false);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                _timeBetweenChangeAnimation = _timerToAim;
                _isAim = true;
                _animator.SetTrigger("AimAttack");
                _animator.SetBool(IsAim, true);
            }

            if (Input.GetKey(KeyCode.Z))
            {
                _bow.gameObject.SetActive(true);
            }

            Debug.Log(_timeBetweenChangeAnimation);
        }

        public void ApplyDamage(float damage)
        {
            if (Health <= 0)
            {
                Health = 0;
                _playerData.IsDied = true;
            }

            if (_playerData.IsDied)
            {
                _animator.SetBool(IsDie, true);
            }
            else
            {
                Health -= damage;
                _audioSource.PlayOneShot(_audioData.OnDetect);
            }

            UpdateHealth();
        }

        private void UpdateHealth()
        {
            HealthChanged?.Invoke(Mathf.RoundToInt(_playerData.Health));
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

            if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift) && _isAim == false)
            {
                Walk();
            }
            else
            {
                if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift) && _isAim == false)
                {
                    Run();
                    if (Stamina >= 60f)
                    {
                        Stamina -= 1f;
                    }
                    else
                    {
                        Stamina -= 0.5f;
                    }
                }
                else if (_isAim == true)
                {
                    AimWalk();
                }
                else
                {
                    if (direction == Vector3.zero && _isAim == false)
                    {
                        Idle();
                    }
                    else
                    {
                        AimIlde();
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
                    if (Camera.main is { })
                    {
                        if (_isAim == true)
                        {
                            var targetAngleforAim = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                              Camera.main.transform.eulerAngles.y;
                            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);

                            var moveDirForAim = Quaternion.Euler(0f, targetAngleforAim, 0f) * Vector3.forward;
                            _characterController.Move(moveDirForAim.normalized * _speed * Time.deltaTime);
                        }
                        else
                        {
                            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                              Camera.main.transform.eulerAngles.y;
                            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                                ref _smooth, _smoothTime);
                            transform.rotation = Quaternion.Euler(0f, angle, 0f);

                            var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                            _characterController.Move(moveDir.normalized * _speed * Time.deltaTime);
                        }
                        
                    }
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

        private void AimIlde()
        {
            _speed = 0f;
            _animator.SetFloat(Speed, 0f);
        }

        private void AimWalk()
        {
            var  horizontalAnimTime = 0.2f;
            var  verticalAnimTime = 0.2f;
            var InputX = Input.GetAxis("Horizontal");
            var InputZ = Input.GetAxis("Vertical");
            _speed = 2f;
            _animator.SetFloat("InputZ", InputZ, verticalAnimTime, Time.deltaTime *2f);
            _animator.SetFloat("InputX", InputX, horizontalAnimTime, Time.deltaTime * 2f);
        }

        [Serializable]
        public class PlayerAudioData
        {
            public AudioClip OnAttack;
            public AudioClip OnDetect;
            public AudioClip OnUnDetect;
            public AudioClip OnMove;
            public AudioClip OnHeal;
            public AudioClip OnCast;
            public AudioClip OnDie;
        }
    }
}