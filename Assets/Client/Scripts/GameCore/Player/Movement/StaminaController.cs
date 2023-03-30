using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Main Parameters")] 
    [SerializeField] private float _maxStamina = 100f;
    public float PlayerStamina { get; private set; } = 100f;
    public bool HasRegenerated { get; private set; } = true;
    private bool _isSprinting;

    [Header("Stamina Regen Parameters")]
    [SerializeField] private float _staminaDrain = 0.5f;
    [SerializeField] private float _staminaRegen = 0.5f;

    [Header("Stamina UI Elements")]
    [SerializeField] private Image _staminaProgressUI;
    [SerializeField] private CanvasGroup _sliderCanvasGroup;

    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        _isSprinting = _playerMovement.Sprinting;
        if (!_isSprinting)
        {
            if (PlayerStamina <= _maxStamina - 0.01)
            {
                PlayerStamina += _staminaRegen * Time.deltaTime;
                UpdateStamina(1);

                if (PlayerStamina >= _maxStamina)
                {
                    _sliderCanvasGroup.alpha = 0;
                    HasRegenerated = true;
                }
            }
        }
    }

    private void UpdateStamina(int value)
    {
        _staminaProgressUI.fillAmount = PlayerStamina / _maxStamina;

        if (value == 0)
        {
            _sliderCanvasGroup.alpha = 0;
        }
        else
        {
            _sliderCanvasGroup.alpha = 1;
        }
    }
    
    public void Sprinting()
    {
        PlayerStamina -= _staminaDrain * Time.deltaTime;
        UpdateStamina(1);

        if (PlayerStamina <= 0)
        {
            HasRegenerated = false;
            _sliderCanvasGroup.alpha = 0;
        }
    }
}