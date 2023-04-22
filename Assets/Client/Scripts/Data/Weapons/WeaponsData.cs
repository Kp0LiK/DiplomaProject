using UnityEngine;

namespace Client.Scripts.Data.Player
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData/Create Weapon Data", order = 0)]
    public class WeaponsData : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float Stamina { get; private set; }
    }
}