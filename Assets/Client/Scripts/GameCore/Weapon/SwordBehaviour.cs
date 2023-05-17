using System.Collections.Generic;
using Client.Scripts.Data.Player;
using UnityEngine;
using Zenject;

namespace Client
{
    public class SwordBehaviour : MonoBehaviour
    {
        private bool _collided;
        private float _lastClickedTime;
        private float _lastComboEnd;
        private int _comboCounter;

        [Inject] private  PlayerBehaviour _playerBehaviour;
        private IDamageable _enemy;

        public IDamageable Enemy => _enemy;

        public bool Collidable;
        public bool Collided => _collided;
        
        private void OnTriggerEnter(Collider other)
        {
            if (Collidable)
            {
                if (_collided == false && other.TryGetComponent(out IDamageable enemy))
                {
                    _enemy = enemy;
                    _collided = true;
                    Enemy.ApplyDamage(_playerBehaviour.Combo[_comboCounter].Damage);
                }
            }
        }

        // private void OnTriggerExit(Collider other)
        // {
        //     if (other.TryGetComponent(out IDamageable enemy))
        //     {
        //         _enemy = null;
        //         _collided = false;
        //         Collidable = false;
        //     }
        // }

        public void StartCombo()
        {
            if (Time.time - _lastComboEnd >= 0.8f)
            {
                CancelInvoke(nameof(EndCombo));
                
                if (_comboCounter >= _playerBehaviour.Combo.Count)
                {
                    _comboCounter = 0;
                }
                if (Time.time - _lastClickedTime >= 1.2f)
                {
                    _playerBehaviour.Animator.runtimeAnimatorController = _playerBehaviour.Combo[_comboCounter].AnimatorOv;
                    _playerBehaviour.Animator.Play("ComboAttack", 0, 0);
                    _comboCounter++;
                    _lastClickedTime = Time.time;
                }
            }
        }

        public void EndAttack()
        {
            if (_playerBehaviour.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f &&
                _playerBehaviour.Animator.GetCurrentAnimatorStateInfo(0).IsTag("ComboAttack"))
            {
                Collidable = false;
                Invoke(nameof(EndCombo), 1);
            }
        }

        public void EndCombo()
        {
            Collidable = false;
            _comboCounter = 0;
            _lastComboEnd = Time.time;
        }
    }
}