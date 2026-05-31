using UnityEngine;
using TargetStrike.Player;

namespace TargetStrike.Utils
{
    public class WeaponSetup : MonoBehaviour
    {
        [Header("Weapon Visuals")]
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private Transform weaponParent;
        
        [Header("Weapon Effects")]
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private AudioClip fireSound;
        
        public void Setup(PlayerShooting shooting)
        {
            if (shooting == null) return;

            // Ensure we have a weapon visible
            if (weaponParent != null && weaponParent.childCount == 0 && weaponPrefab != null)
            {
                GameObject weapon = Instantiate(weaponPrefab, weaponParent);
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;
                
                // Try to find muzzle flash in the instantiated weapon
                if (muzzleFlash == null)
                    muzzleFlash = weapon.GetComponentInChildren<ParticleSystem>();
            }

            // Inject references into PlayerShooting
            // We use reflection because fields are private, or we could make them public/serialized
            // For now, let's assume we use a public Initialize method we're about to add to PlayerShooting
            shooting.InitializeWeapon(muzzleFlash, fireSound);
        }
    }
}
