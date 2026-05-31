using UnityEngine;
using System;

namespace TargetStrike.Player
{
    public class PlayerAmmo : MonoBehaviour
    {
        [Header("Ammo Settings")]
        [SerializeField] private int maxMagazineAmmo = 30;
        [SerializeField] private int currentMagazineAmmo;
        [SerializeField] private int totalReserveAmmo = 90;

        public event Action OnAmmoChanged;
        public event Action OnOutOfAmmo;

        public int CurrentMagazineAmmo => currentMagazineAmmo;
        public int TotalReserveAmmo => totalReserveAmmo;

        private void Awake()
        {
            currentMagazineAmmo = maxMagazineAmmo;
        }

        public bool CanFire()
        {
            return currentMagazineAmmo > 0;
        }

        public void ConsumeAmmo()
        {
            currentMagazineAmmo--;
            OnAmmoChanged?.Invoke();

            if (currentMagazineAmmo <= 0 && totalReserveAmmo <= 0)
            {
                OnOutOfAmmo?.Invoke();
            }
        }

        public void Reload()
        {
            if (currentMagazineAmmo == maxMagazineAmmo || totalReserveAmmo <= 0) return;

            int ammoNeeded = maxMagazineAmmo - currentMagazineAmmo;
            int ammoToReload = Mathf.Min(ammoNeeded, totalReserveAmmo);

            currentMagazineAmmo += ammoToReload;
            totalReserveAmmo -= ammoToReload;

            OnAmmoChanged?.Invoke();
        }
    }
}
