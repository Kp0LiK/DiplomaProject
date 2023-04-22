using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Client
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private PlayerInventoryChangerButton _changerButton;
        
        [SerializeField] private List<BaseWeapon> _weapons;
        public event Action<BaseWeapon> WeaponChanged;

        public BaseWeapon CurrentWeapon { get; private set; }

        public List<BaseWeapon> Weapons { get; private set; }

        private void Start()
        {
            CurrentWeapon = _weapons[0];
        }

        private void OnEnable()
        {
            _changerButton.FirstAlpha.action.performed += FirstAlphaPressed;
        }

        private void OnDisable()
        {
            _changerButton.FirstAlpha.action.performed -= FirstAlphaPressed;
        }

        private void FirstAlphaPressed(InputAction.CallbackContext obj)
        {
            //ChangeWeapon<BowWeapon>();
        }

        private void ChangeWeapon<T>() where T : BaseWeapon
        {
            var currentWeapon = _weapons.FirstOrDefault(w => w is T);

            if (ReferenceEquals(currentWeapon, null))
            {
                Debug.LogError("[Inventory] weapon is null");
                return;
            }

            if (ReferenceEquals(CurrentWeapon, currentWeapon))
            {
                Debug.LogError("[Inventory] current weapon already get");
                return;
            }

            CurrentWeapon = currentWeapon;
            WeaponChanged?.Invoke(currentWeapon);
            Debug.Log("[Inventory] Current Weapon Changed");
        }
    }

    [Serializable]
    public class PlayerInventoryChangerButton
    {
        public InputActionReference FirstAlpha;
    }
}