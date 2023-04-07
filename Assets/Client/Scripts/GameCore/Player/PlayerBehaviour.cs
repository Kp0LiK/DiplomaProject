using System;
using Client.Scripts.Data.Player;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    private NPC _currentNPC;
    private Quest _currentQuest;

    private Animator _animator;
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    public event Action<int> HealthChanged;
    
    private float _speed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _smoothTime;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance;
    [SerializeField] private LayerMask _groundMask;
    
    private CharacterController _characterController;

    public PlayerData PlayerData => _playerData;

    private Vector3 _velocity;
    private bool _isGrounded;
    private float _smooth;
    private static readonly int Speed = Animator.StringToHash("Run");


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _speed = _playerData.WalkSpeed;
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


        if (Input.GetKeyDown(KeyCode.F) && _currentQuest != null)
        {
            if (_currentQuest.IsActive)
            {
                _currentQuest.QuestGoal.EnemyKilled();
                if (_currentQuest.QuestGoal.IsReached())
                {
                    _currentQuest.CompleteQuest();
                }
            }
        }
    }

    public void ApplyDamage(float damage)
    {
        if (_playerData.Health <= 0)
        {
            _playerData.Health = 0;
            _playerData.IsDied = true;
        }

        if (_playerData.IsDied)
        {
            //todo death state
        }
        else
        {
            _playerData.Health -= damage;
        }

        UpdateHealth();
    }

    private void UpdateHealth()
    {
        HealthChanged?.Invoke(Mathf.RoundToInt(_playerData.Health));
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

    public void SetActiveQuest(Quest quest)
    {
        _currentQuest = quest;
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
