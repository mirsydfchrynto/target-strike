using UnityEngine;
using InfimaGames.LowPolyShooterPack;

namespace TargetStrike.Utils
{
    /// <summary>
    /// This script fixes the NullReferenceException in Infima's Weapon.cs 
    /// by providing dummy services that it expects.
    /// </summary>
    public class InfimaBridge : MonoBehaviour
    {
        private void Awake()
        {
            // If we are not using Infima's full system, their Weapon script will crash.
            // We can either provide a dummy service or just disable the script if we use our own.
            
            Weapon weapon = GetComponentInChildren<Weapon>();
            if (weapon != null)
            {
                Debug.Log("[INFIMA BRIDGE] Infima Weapon script detected. Disabling it to prevent service-related crashes, using our custom PlayerShooting instead.");
                weapon.enabled = false;
            }
        }
    }
}
