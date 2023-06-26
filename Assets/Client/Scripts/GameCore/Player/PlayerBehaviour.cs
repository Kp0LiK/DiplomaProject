using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Data.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private QuestManager _questManager;
        protected NPC _currentNPC;

        [Header("Controller")] [SerializeField]
        private PlayerData _playerData;

        [SerializeField] protected float _gravity;
        [SerializeField] protected float _smoothTime;
        [SerializeField] protected float _groundDistance;
        [SerializeField] private float _mouseLookSpeed;
        [SerializeField] protected Transform _groundCheck;
        [SerializeField] protected LayerMask _groundMask;
        [SerializeField] private PlayerAudioData _audioData;
        [SerializeField] protected Camera _mainCamera;

        [Header("Weapons")] [SerializeField] protected BowWeapon _bow;
        [SerializeField] protected SwordBehaviour _sword;
        [SerializeField] protected KobyzWeapon _kobyz;
        [SerializeField] protected KobyzWeapon _kobyz2;
        [SerializeField] private List<WeaponsData> _combo;
        [SerializeField] protected Camera _aimCamera;
        [SerializeField] protected GameObject _aimTarget;

        [SerializeField] private float _lookXLimit = 45.0f;

        private Collider _swordCollider;

        private PlayerInventory _playerInventory;

        protected CharacterController _characterController;
        private AudioSource _audioSource;

        protected float _speed;
        private float _health;
        private float _stamina;
        private float _mana;
        private bool _isDie;
        protected float _smooth;

        protected bool _isGrounded;
        protected bool _isAim;
        protected bool _isBow;
        protected bool _isSword;
        protected bool _isKobyz;

        protected Vector3 _velocity;

        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        private static readonly int IsSwordAttack = Animator.StringToHash("isSwordAttack");
        private static readonly int Speed = Animator.StringToHash("Run");
        private static readonly int IsDie = Animator.StringToHash("isDie");
        protected static readonly int IsAim = Animator.StringToHash("isAim");
        protected static readonly int AimAttack = Animator.StringToHash("AimAttack");
        private static readonly int Z = Animator.StringToHash("InputZ");
        private static readonly int X = Animator.StringToHash("InputX");

        public Animator Animator { get; set; }

        public bool IsStanding { get; set; }


        public List<WeaponsData> Combo
        {
            get => _combo;
            set => _combo = value;
        }


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

                if (_health <= 0)
                {
                    _health = 0;
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
                    _audioSource.Stop();
                }

                if (_stamina <= 0 && !_audioSource.isPlaying)
                {
                    _audioSource.PlayOneShot(_audioData.OnStaminaRecovery);
                    _stamina = 0;
                    Walk();
                }
            }
        }

        public float Mana
        {
            get => _mana;
            set
            {
                ManaChanged?.Invoke(value);
                _mana = value;
                if (_mana >= _playerData.Mana)
                {
                    _mana = _playerData.Mana;
                }

                if (!(_mana <= 0)) return;
                _mana = 0;
            }
        }

        public event Action<float> HealthChanged;
        public event Action<float> StaminaChanged;

        public event Action<float> ManaChanged;

        public Action<int> ExecuteComboSound;
        public Action<bool> ExecuteStaminaRecoveryClip;

        public static Action OnEncounter;
        public static Action OnQuestAccept;

        public static Action OnManySpiders;
        public static Action OnGoblins;
        public static Action OnConfusingRoad;
        public static Action OnBigDragon;
        public static Action OnGuardian;

        public AudioSource AudioSource => _audioSource;

        private float _rotationX;

        protected virtual void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
            _playerInventory = GetComponent<PlayerInventory>();
            _swordCollider = _sword.gameObject.GetComponent<Collider>();
            _audioSource = GetComponent<AudioSource>();

            Physics.IgnoreCollision(_swordCollider, GetComponent<Collider>());
        }

        protected virtual void Start()
        {
            _sword.gameObject.SetActive(false);
            _bow.gameObject.SetActive(false);
            _playerData.IsDied = false;
            _speed = _playerData.WalkSpeed;
            _health = _playerData.Health;
            _stamina = _playerData.Stamina;
            _mana = _playerData.Mana;
        }

        protected virtual void OnEnable()
        {
            QuestGiver.OnQuestGiven += AddQuest;
            _playerInventory.WeaponChanged += OnWeaponChanged;
            ExecuteComboSound += PlayComboSound;
            
            OnEncounter += PlayEncounterSound;
            OnQuestAccept += PlayQuestAcceptSound;
            
            OnManySpiders += PlayManySpidersSound;
            OnGoblins += PlayGoblinsSound;
            OnConfusingRoad += PlayConfusingRoadSound;
            OnBigDragon += PlayBigDragonSound;
            OnGuardian += PlayGuardianSound;
        }

        protected virtual void OnDisable()
        {
            QuestGiver.OnQuestGiven -= AddQuest;
            _playerInventory.WeaponChanged -= OnWeaponChanged;
            ExecuteComboSound -= PlayComboSound;
            
            OnEncounter -= PlayEncounterSound;
            OnQuestAccept -= PlayQuestAcceptSound;
            
            OnManySpiders -= PlayManySpidersSound;
            OnGoblins -= PlayGoblinsSound;
            OnConfusingRoad -= PlayConfusingRoadSound;
            OnBigDragon -= PlayBigDragonSound;
            OnGuardian -= PlayGuardianSound;
        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && _currentNPC)
            {
                if (_currentNPC.Interactable)
                {
                    _currentNPC.Interacted?.Invoke();
                }
            }

            Move();

            // if (_isAim)
            // {
            //     _timeBetweenChangeAnimation -= Time.deltaTime;
            //     if (_timeBetweenChangeAnimation <= 0)
            //     {
            //         _isAim = false;
            //         Animator.SetBool(IsAim, false);
            //     }
            // }

            if (Input.GetMouseButtonDown(0) && _isAim && _isBow)
            {
                Animator.SetTrigger(AimAttack);
                _bow.Shoot();
            }

            if (Input.GetMouseButtonDown(1) && _isBow)
            {
                _isAim = true;
                Animator.SetBool(IsAim, true);
                _aimCamera.gameObject.SetActive(true);
                _aimTarget.gameObject.SetActive(true);
                _audioSource.PlayOneShot(_audioData.OnAim);
            }

            if (Input.GetMouseButtonUp(1) && _isBow)
            {
                _isAim = false;
                Animator.SetBool(IsAim, false);
                _aimCamera.gameObject.SetActive(false);
                _aimTarget.gameObject.SetActive(false);
            }

            if (_isAim && _isKobyz)
            {
                if (Input.GetKeyDown(KeyCode.E) && Mana >= 20f)
                {
                    Animator.SetTrigger(AimAttack);
                    _kobyz.Shoot(ProjectileType.FIREBALL);
                    Mana -= 20f;
                }
                else if (Input.GetKeyDown(KeyCode.R) && Mana >= 20f)
                {
                    Animator.SetTrigger(AimAttack);
                    _kobyz.Shoot(ProjectileType.ICE);
                    Mana -= 20f;
                }
            }

            if (Input.GetMouseButtonDown(1) && _isKobyz)
            {
                _isAim = true;
                Animator.SetBool(IsAim, true);
                _aimCamera.gameObject.SetActive(true);
                _aimTarget.gameObject.SetActive(true);
                _audioSource.PlayOneShot(_audioData.OnAim);
            }

            if (Input.GetMouseButtonUp(1) && _isKobyz)
            {
                _isAim = false;
                Animator.SetBool(IsAim, false);
                _aimCamera.gameObject.SetActive(false);
                _aimTarget.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && !_isSword)
            {
                _isSword = true;
                _isBow = false;
                _isAim = false;
                _isKobyz = false;
                Animator.SetBool(IsAim, false);
                _aimCamera.gameObject.SetActive(false);
                _aimTarget.gameObject.SetActive(false);
                _sword.gameObject.SetActive(true);
                _bow.gameObject.SetActive(false);
                _kobyz.gameObject.SetActive(false);
                _kobyz2.gameObject.SetActive(true);
                _audioSource.PlayOneShot(_audioData.OnSwordDraw);
            }

            if (Input.GetKey(KeyCode.Alpha2) && !_isBow)
            {
                _isBow = true;
                _isSword = false;
                _isKobyz = false;
                _bow.gameObject.SetActive(true);
                _sword.gameObject.SetActive(false);
                _kobyz.gameObject.SetActive(false);
                _kobyz2.gameObject.SetActive(true);
                _audioSource.PlayOneShot(_audioData.OnRangedDraw);
            }

            if (Input.GetKey(KeyCode.Alpha3) & !_isKobyz)
            {
                _isKobyz = true;
                _isBow = false;
                _isSword = false;
                _kobyz.gameObject.SetActive(true);
                _kobyz2.gameObject.SetActive(false);

                _bow.gameObject.SetActive(false);
                _sword.gameObject.SetActive(false);

                _audioSource.PlayOneShot(_audioData.OnRangedDraw);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space) && IsStanding == false)
            {
                StartStanding();
            }

            if (Input.GetMouseButtonDown(0) && _isSword)
            {
                _sword.Collidable = true;
                _sword.StartCombo();
            }
            else
            {
                _sword.EndAttack();
            }
        }

        protected async void StartStanding()
        {
            IsStanding = true;
            Animator.SetBool("isStanding", true);
            _audioSource.PlayOneShot(_audioData.OnDodge);

            await Task.Delay(800);
            IsStanding = false;
            Animator.SetBool("isStanding", false);
        }

        public void ApplyDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0 && !_isDie)
            {
                _isDie = true;
                Health = 0;
                Animator.SetBool(IsDie, true);
                _audioSource.PlayOneShot(_audioData.OnDie);
                Destroy(gameObject, 5f);
            }

            if (_isDie) return;
            _audioSource.PlayOneShot(_audioData.OnHit);
            HealthChanged?.Invoke(_health);
        }

        private void OnWeaponChanged(BaseWeapon weapon)
        {
            if (weapon is BowWeapon)
            {
                Debug.Log("[PLAYER] Changed to Bow");
            }
        }

        protected virtual void Move()
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
                    if (Stamina >= 60f)
                    {
                        Stamina -= 1f;
                    }
                    else
                    {
                        Stamina -= 0.5f;
                    }
                }
                else if (_isAim)
                {
                    AimWalk();
                }
                else
                {
                    if (direction == Vector3.zero)
                    {
                        Idle();
                    }
                    else
                    {
                        AimIdle();
                    }
                }
            }

            direction = (Vector3.right * horizontal + Vector3.forward * vertical).normalized;

            if (direction.magnitude >= 0.1f || _characterController.velocity.magnitude > 0.1f)
            {
                if (_mainCamera != null)
                {
                    //Player Movement with AimState
                    if (_isAim)
                    {
                        var eulerAngles = _aimCamera.transform.eulerAngles;
                        var targetAngleforAim = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                                eulerAngles.y;
                        transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
                        var moveDirForAim = Quaternion.Euler(0f, targetAngleforAim, 0f) * Vector3.forward;
                        _characterController.Move(moveDirForAim.normalized * (_speed * Time.deltaTime));
                        
                        _rotationX += -Input.GetAxis("Mouse Y") * _mouseLookSpeed;
                        _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
                        _aimCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
                        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X"), 0);
                    }
                    //Player Movement without AimState
                    else
                    {
                        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                          _mainCamera.transform.eulerAngles.y;
                        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                            ref _smooth, _smoothTime);
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);

                        var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                        _characterController.Move(moveDir.normalized * (_speed * Time.deltaTime));
                    }
                }
            }
            else
            {
                //Look for camera, when Player AimIdle
                if (_isAim)
                {
                    _rotationX += -Input.GetAxis("Mouse Y") * _mouseLookSpeed;
                    _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
                    _aimCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
                    transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X"), 0);
                }
            }


            _velocity.y += _gravity * Time.deltaTime;

            _characterController.Move(_velocity * Time.deltaTime);
        }

        protected void Idle()
        {
            _speed = 0f;
            Animator.SetFloat(Speed, 0f);
        }

        protected void Walk()
        {
            _speed = _playerData.WalkSpeed;
            Animator.SetFloat(Speed, 0.4f);
        }

        protected void Run()
        {
            _speed = _playerData.RunSpeed;
            Animator.SetFloat(Speed, 1f);
        }

        protected void AimIdle()
        {
            _speed = 0f;
            Animator.SetFloat(Speed, 0f);
        }

        protected void AimWalk()
        {
            var horizontalAnimTime = 0.2f;
            var verticalAnimTime = 0.2f;
            var InputX = Input.GetAxis("Horizontal");
            var InputZ = Input.GetAxis("Vertical");
            Animator.SetFloat(Z, InputZ, verticalAnimTime, Time.deltaTime * 2f);
            Animator.SetFloat(X, InputX, horizontalAnimTime, Time.deltaTime * 2f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out NPC npc))
            {
                npc.Approached?.Invoke(true);
                _currentNPC = npc;
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

        private void PlayComboSound(int combo)
        {
            switch (combo)
            {
                case 1:
                    _audioSource.PlayOneShot(_audioData.OnCombo1);
                    break;
                case 2:
                    _audioSource.PlayOneShot(_audioData.OnCombo2);
                    break;
                case 3:
                    _audioSource.PlayOneShot(_audioData.OnCombo3);
                    break;
            }
        }

        private void PlayEncounterSound()
        {
            if (Random.Range(0, 100) <= 20)
            {
                _audioSource.PlayOneShot(_audioData.OnEncounter[Random.Range(0, 3)]);
            }
        }

        private void PlayQuestAcceptSound()
        {
            _audioSource.PlayOneShot(_audioData.OnQuestAccept[Random.Range(0, 5)]);
        }

        private void PlayManySpidersSound()
        {
            _audioSource.PlayOneShot(_audioData.ManySpiders);
        }

        private void PlayGoblinsSound()
        {
            _audioSource.PlayOneShot(_audioData.Goblins);
        }

        private void PlayConfusingRoadSound()
        {
            _audioSource.PlayOneShot(_audioData.ConfusingRoad);
        }

        private void PlayBigDragonSound()
        {
            _audioSource.PlayOneShot(_audioData.BigDragon);
        }

        private void PlayGuardianSound()
        {
            _audioSource.PlayOneShot(_audioData.Guardian);
        }

        [Serializable]
        public class PlayerAudioData
        {
            public AudioClip OnCombo1;
            public AudioClip OnCombo2;
            public AudioClip OnCombo3;
            public AudioClip OnDie;
            public AudioClip OnHit;
            public AudioClip OnSwordDraw;
            public AudioClip OnRangedDraw;
            public AudioClip OnDodge;
            public AudioClip OnAim;
            public AudioClip OnStaminaRecovery;
            public AudioClip[] OnEncounter;
            public AudioClip[] OnQuestAccept;
            public AudioClip ManySpiders;
            public AudioClip ConfusingRoad;
            public AudioClip Goblins;
            public AudioClip BigDragon;
            public AudioClip Guardian;
        }
    }
}