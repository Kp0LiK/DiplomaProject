using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public Vector3 Direction { get; set; }

    public Rigidbody Rigidbody { get; private set; }
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision other)
    {

        Destroy(gameObject, 3);
    }
}