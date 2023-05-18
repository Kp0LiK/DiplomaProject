using UnityEngine;

namespace Client.Scripts.Data.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData/Create Enemy Data", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; set; }
        [field: SerializeField] public float StopDistance { get; private set; }

        [field: SerializeField] public float Health { get; set; }
        [field: SerializeField] public float Damage { get; set; }

        [field: SerializeField] public bool IsDied { get; set; }


        public void DeathState(float damage)
        { 
            Damage = damage;
        }
    }
}