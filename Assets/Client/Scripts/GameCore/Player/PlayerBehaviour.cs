using System;
using System.Threading.Tasks;
using Client.Scripts.Data.Player;
using UnityEngine;

namespace Client
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private QuestManager _questManager;
        private NPC _currentNPC;
        
        [Header("Controller")] [SerializeField]
        private PlayerData _playerData;

        [SerializeField] private float _gravity;
        [SerializeField] private float _smoothTime;
        [SerializeField] private float _groundDistance;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private PlayerAudioData _audioData;

        [Header("Weapons")] 
        [SerializeField] private BowWeapon _bow;
        [SerializeField] private WeaponBehaviour _sword;

        private Collider _swordCollider;

        private PlayerInventory _playerInventory;

        private CharacterController _characterController;
        private AudioSource _audioSource;

        private float _speed;
        private float _health;
        private float _stamina;
        private float _smooth;

        private bool _isGrounded;
        private bool _isAim;
        private bool _isBow;
        private float _timerToAim;
        private float _timeBetweenChangeAnimation;

        private Vector3 _velocity;

        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        private static readonly int IsSwordAttack = Animator.StringToHash("isSwordAttack");
        private static readonly int Speed = Animator.StringToHash("Run");
        private static readonly int IsDie = Animator.StringToHash("isDie");
        private static readonly int IsAim = Animator.StringToHash("isAim");
        private static readonly int AimAttack = Animator.StringToHash("AimAttack");
        private static readonly int Z = Animator.StringToHash("InputZ");
        private static readonly int X = Animator.StringToHash("InputX");

        public Animator Animator { get; set; }

        public bool IsStanding { get; set; }


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
            Animator = GetComponent<Animator>();
            _playerInventory = GetComponent<PlayerInventory>();
            _swordCollider = _sword.gameObject.GetComponent<Collider>();
            
            Physics.IgnoreCollision(_swordCollider, GetComponent<Collider>());
        }

        private void Start()
        {
            _playerData.IsDied = false;
            _speed = _playerData.WalkSpeed;
            _health = _playerData.Health;
            _stamina = _playerData.Stamina;
            _timerToAim = 1f;
        }

        private void OnEnable()
        {
            QuestGiver.OnQuestGiven += AddQuest;
            _playerInventory.WeaponChanged += OnWeaponChanged;
        }

        private void OnDisable()
        {
            QuestGiver.OnQuestGiven -= AddQuest;
            _playerInventory.WeaponChanged -= OnWeaponChanged;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G) && _currentNPC)
            {
                if (_currentNPC.Interactable)
                {
                    _currentNPC.Interacted?.Invoke();
                }
            }
            
            
            //Debug.Log(_stamina);
            // Animator.SetBool(IsAttack, Input.GetKey(KeyCode.E));
            // Animator.SetBool(IsSwordAttack, Input.GetKey(KeyCode.E));
            
            if (Input.GetKeyDown(KeyCode.E)) Attack();
            
            _sword.Collidable = AnimatorIsPlaying("Standing Melee Attack Downward");
            
            
            Move();

            if (_isAim)
            {
                _timeBetweenChangeAnimation -= Time.deltaTime;
                if (_timeBetweenChangeAnimation <= 0)
                {
                    _isAim = false;
                    Animator.SetBool(IsAim, false);
                }
            }

            if (Input.GetMouseButtonDown(0) && _isBow)
            {
                _timeBetweenChangeAnimation = _timerToAim;
                _isAim = true;
                Animator.SetTrigger(AimAttack);
                Animator.SetBool(IsAim, true);
                _bow.Shoot();
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                _isBow = true;
                _bow.gameObject.SetActive(true);
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                _isBow = false;
                _bow.gameObject.SetActive(false);   
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
            {
                Standing();
            }
        }

        private async void Standing()
        {
            IsStanding = true;
            Animator.SetBool("isStanding", true);
            await Task.Delay(700);
            IsStanding = false;
            Animator.SetBool("isStanding", false);
        }

        public void ApplyDamage(float damage)
        {
            _health -= damage;

            if (_health <= 0)
            {
                _health = 0;
                Animator.SetBool(IsDie, true);
                Destroy(gameObject, 5);
            }

            HealthChanged?.Invoke(_health);
        }

        private void OnWeaponChanged(BaseWeapon weapon)
        {
            if (weapon is BowWeapon)
            {
                Debug.Log("[PLAYER] Changed to Bow");
            }
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
                        var eulerAngles = Camera.main.transform.eulerAngles;
                        var targetAngleforAim = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                                eulerAngles.y;
                        transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);

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
            Animator.SetFloat(Speed, 0f);
        }

        private void Walk()
        {
            _speed = _playerData.WalkSpeed;
            Animator.SetFloat(Speed, 0.4f);
        }

        private void Run()
        {
            _speed = _playerData.RunSpeed;
            Animator.SetFloat(Speed, 1f);
        }

        private void AimIlde()
        {
            _speed = 0f;
            Animator.SetFloat(Speed, 0f);
        }

        private void AimWalk()
        {
            var horizontalAnimTime = 0.2f;
            var verticalAnimTime = 0.2f;
            var InputX = Input.GetAxis("Horizontal");
            var InputZ = Input.GetAxis("Vertical");
            _speed = 2f;
            Animator.SetFloat(Z, InputZ, verticalAnimTime, Time.deltaTime * 2f);
            Animator.SetFloat(X, InputX, horizontalAnimTime, Time.deltaTime * 2f);
        }
        
        private void Attack()
        {
            Animator.SetTrigger(IsSwordAttack);
        }
        
        private bool AnimatorIsPlaying(){
            return Animator.GetCurrentAnimatorStateInfo(0).length >
                   Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    
        private bool AnimatorIsPlaying(string stateName){
            return AnimatorIsPlaying() && Animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out NPC npc))
            {
                npc.Approached?.Invoke(true);
                _currentNPC = npc;
            }
            
            if (Animator.GetBool(IsSwordAttack))
            {
                _sword.Collidable = true;
            }
            else
            {
                _sword.Collidable = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out NPC npc))
            {
                npc.Approached?.Invoke(false);
                _currentNPC = null;
            }
        }

        private void AddQuest(Quest quest)
        {
            if (_questManager.CurrentQuests.Contains(quest)) return;
            _questManager.AddQuest(quest);
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