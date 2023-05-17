using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverForVideo : MonoBehaviour
{
    public float speed = 5f; // скорость движения
        public Animator animator; // ссылка на компонент Animator
    
        private void Update()
        {
            // Движение NPC вперед
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    
            // Устанавливаем параметр "Speed" в значение скорости движения, чтобы переключить анимацию
            animator.SetFloat("Run", 0.6f);
        }
}
