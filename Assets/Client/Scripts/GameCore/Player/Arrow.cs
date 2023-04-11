using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arrow : MonoBehaviour
{
    public float speed = 10f;  // скорость полета стрелы
    public float lifetime = 5f;  // время жизни стрелы в секундах

    private float spawnTime;  // время создания стрелы

    void Start()
    {
        spawnTime = Time.time;  // запоминаем время создания стрелы
    }

    void Update()
    {
        
    }

    public void Shoot(Vector2 direction)
    {
        // перемещаем стрелу вперед со скоростью speed
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // если стрела прожила больше lifetime секунд, то уничтожаем ее
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
