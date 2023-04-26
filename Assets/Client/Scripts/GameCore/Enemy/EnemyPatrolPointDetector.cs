using System.Collections;
using System.Collections.Generic;
using Client;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPatrolPointDetector : MonoBehaviour
{
    public event UnityAction<PatrolPoint> Entered;
    public event UnityAction<PatrolPoint> DetectExited;
    
    public PatrolPoint PatrolPoint { get; private set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PatrolPoint patrolPoint)) return;
        PatrolPoint = patrolPoint;
        Entered?.Invoke(patrolPoint);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out PatrolPoint patrolPoint)) return;
        DetectExited?.Invoke(patrolPoint);
        PatrolPoint = null;
    }
}
