using UnityEngine;

namespace Client.Scripts.Data.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Create Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public float WalkSpeed { get; private set; }
        [field: SerializeField] public float RunSpeed { get; private set; }
        [field: SerializeField] public float Health { get; set; }
        [field: SerializeField] public float Stamina { get; private set; }
        [field: SerializeField] public float Mana { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        
        [field: SerializeField] public bool IsDied { get; set; }
    }
}