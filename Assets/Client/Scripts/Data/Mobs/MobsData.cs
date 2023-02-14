using UnityEngine;

namespace Client.Scripts.Data.Mobs
{
    [CreateAssetMenu(fileName = "Mobsdata", menuName = "MobsData/Mobs Data", order = 0)]
    public class MobsData : ScriptableObject
    {
        [field: SerializeField] public int Speed { get; private set; }
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        
        [field: SerializeField] public bool IsDied { get; private set; }
    }
}