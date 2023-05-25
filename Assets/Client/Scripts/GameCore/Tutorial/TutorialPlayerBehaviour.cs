using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Data.Player;
using UnityEngine;

namespace Client
{
    public class TutorialPlayerBehaviour : PlayerBehaviour
    {
        private bool _canMove;
        private bool _canAttack;
        private bool _canInteract;

        protected override void OnEnable()
        {
            base.OnEnable();
            TutorialSystem.SetPlayerMovement += SetMovement;
            TutorialSystem.SetPlayerAttack += SetAttack;
            TutorialSystem.SetPlayerInteraction += SetInteraction;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            TutorialSystem.SetPlayerMovement -= SetMovement;
            TutorialSystem.SetPlayerAttack -= SetAttack;
            TutorialSystem.SetPlayerInteraction -= SetInteraction;
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && _currentNPC && _canInteract)
            {
                if (_currentNPC.Interactable)
                {
                    _currentNPC.Interacted?.Invoke();
                }
            }

            Move();

            if (!_canAttack) return;
            
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
            }

            if (Input.GetMouseButtonUp(1) && _isKobyz)
            {
                _isAim = false;
                Animator.SetBool(IsAim, false);
                _aimCamera.gameObject.SetActive(false);
                _aimTarget.gameObject.SetActive(false);
            }

            if (Input.GetKey(KeyCode.Alpha1))
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
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                _isBow = true;
                _isSword = false;
                _isKobyz = false;
                _bow.gameObject.SetActive(true);
                _sword.gameObject.SetActive(false);
                _kobyz.gameObject.SetActive(false);
                _kobyz2.gameObject.SetActive(true);
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                _isKobyz = true;
                _isBow = false;
                _isSword = false;
                _kobyz.gameObject.SetActive(true);
                _kobyz2.gameObject.SetActive(false);

                _bow.gameObject.SetActive(false);
                _sword.gameObject.SetActive(false);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
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

        protected override void Move()
        {
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            if (_canMove)
            {
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
                            var eulerAngles = _mainCamera.transform.eulerAngles;
                            var targetAngleforAim = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                                    eulerAngles.y;
                            transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
                            var moveDirForAim = Quaternion.Euler(0f, targetAngleforAim, 0f) * Vector3.forward;
                            _characterController.Move(moveDirForAim.normalized * _speed * Time.deltaTime);
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
                            _characterController.Move(moveDir.normalized * _speed * Time.deltaTime);
                        }
                    }
                }
                else
                {
                    //Look for camera, when Player AimIdle
                    if (_isAim)
                    {
                        if (_mainCamera != null)
                        {
                            var lookPos = _mainCamera.transform.position - transform.position;
                            lookPos.y = 0;
                            var rotation = Quaternion.LookRotation(-lookPos);
                            // поворачиваем по оси Y
                            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                                rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                        }
                    }
                }
            }

            _velocity.y += _gravity * Time.deltaTime;

            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void SetMovement(bool canMove)
        {
            _canMove = canMove;
            
            if (!canMove) Idle();
        }

        private void SetAttack(bool canAttack)
        {
            _canAttack = canAttack;

            if (!canAttack)
            {
                Animator.SetBool(IsAim, false);
                _aimCamera.gameObject.SetActive(false);
                _aimTarget.gameObject.SetActive(false);
            }
        }

        private void SetInteraction(bool canInteract)
        {
            _canInteract = canInteract;
        }
    }
}