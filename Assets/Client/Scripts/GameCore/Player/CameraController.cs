using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;

    void Update()
    {
        // Перемещение камеры
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical);
        transform.position += transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;

        // Вращение камеры
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, rotateHorizontal * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, -rotateVertical * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
