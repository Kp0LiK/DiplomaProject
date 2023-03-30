using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Essentials")]
    [SerializeField] private Transform _camera;
    private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;
    private CharacterController _characterController;
    private Animator _animator;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    private Vector2 _movement;
    public bool Sprinting { get; private set; }
    private float _trueSpeed;
    private StaminaController _staminaController;

    [Header("Jumping")]
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity;
    private bool _isGrounded;
    private Vector3 _velocity;

    private int _isGroundedHash;
    private int _speedHash;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _staminaController = GetComponent<StaminaController>();
        _animator = GetComponentInChildren<Animator>();

        _trueSpeed = _walkSpeed;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _isGroundedHash = Animator.StringToHash("isGrounded");
        _speedHash = Animator.StringToHash("Speed");
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(transform.position, .1f, 1);
        _animator.SetBool(_isGroundedHash, _isGrounded);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -1;
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_staminaController.HasRegenerated)
            {
                _trueSpeed = _sprintSpeed;
                Sprinting = true;
                _staminaController.Sprinting();
            }
            else
            {
                _trueSpeed = _walkSpeed;
                Sprinting = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _trueSpeed = _walkSpeed;
            Sprinting = false;
        }

        _animator.transform.localPosition = Vector3.zero;
        _animator.transform.localEulerAngles = Vector3.zero;
        
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 direction = new Vector3(_movement.x, 0f, _movement.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _characterController.Move(moveDirection.normalized * (_trueSpeed * Time.deltaTime));

            float speedValue = Sprinting ? 2 : 1;
            
            _animator.SetFloat(_speedHash, speedValue);
        }
        else
        {
            _animator.SetFloat(_speedHash, 0);
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt((_jumpHeight * 10) * -2f * _gravity);
        }

        if (_velocity.y > -20)
        {
            _velocity.y += (_gravity * 10) * Time.deltaTime;
        }
        
        _velocity.y += (_gravity * 10) * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
