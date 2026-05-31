using UnityEngine;

namespace TargetStrike.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [Header("Settings")]
        public float range = 100f;
        public float damage = 1f;
        public LayerMask hitMask = -1;
        public float fireRate = 0.15f;
        private float nextFireTime;

        [Header("Status Diagnostik (Auto-find)")]
        public PlayerInputHandler inputHandler;
        public Camera mainCamera;
        public Animator weaponAnimator;
        public ParticleSystem muzzleFlash;
        public AudioSource audioSource;
        public AudioClip fireSound;

        public event System.Action OnHitTarget;

        private void Start()
        {
            RefreshReferences();
        }

        public void RefreshReferences()
        {
            // 1. Find Input Handler
            if (inputHandler == null) inputHandler = GetComponent<PlayerInputHandler>();
            if (inputHandler == null) inputHandler = GetComponentInParent<PlayerInputHandler>();
            if (inputHandler == null) inputHandler = Object.FindAnyObjectByType<PlayerInputHandler>();

            // 2. Find Camera
            if (mainCamera == null) mainCamera = GetComponentInChildren<Camera>();
            if (mainCamera == null) mainCamera = Camera.main;

            // 3. Audio Setup
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            
            // Auto-fetch fire sound
            if (fireSound == null)
            {
                #if UNITY_EDITOR
                string[] guids = UnityEditor.AssetDatabase.FindAssets("S_WEP_Fire_002 t:AudioClip");
                if (guids.Length > 0)
                {
                    fireSound = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]));
                }
                #endif
            }

            // 4. Weapon Visuals
            AutoLinkWeapon();
        }

        private void AutoLinkWeapon()
        {
            if (weaponAnimator == null) weaponAnimator = GetComponentInChildren<Animator>();
            
            if (weaponAnimator != null)
            {
                if (muzzleFlash == null)
                {
                    muzzleFlash = weaponAnimator.gameObject.GetComponentInChildren<ParticleSystem>();
                    
                    if (muzzleFlash == null)
                    {
                        foreach (var ps in weaponAnimator.GetComponentsInChildren<ParticleSystem>(true))
                        {
                            if (ps.name.ToLower().Contains("muzzle")) { muzzleFlash = ps; break; }
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (inputHandler == null) return;

            // Direct Mouse Check (Bulletproof)
            bool isFiring = UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
            if (inputHandler.FireTriggered) isFiring = true;

            if (isFiring)
            {
                if (Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + fireRate;
                    Shoot();
                }
            }

            // Reload Check
            bool isReloading = UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame;
            if (inputHandler.ReloadTriggered) isReloading = true;

            if (isReloading) Reload();
        }

        private void Shoot()
        {
            Debug.Log("<color=cyan>[PLAYER SHOOTING]</color> EKSEKUSI TEMBAKAN DIMULAI!");

            // 1. Visual Animasi
            if (weaponAnimator != null) 
            {
                weaponAnimator.SetTrigger("Fire");
                Debug.Log("-> Animasi 'Fire' dijalankan.");
            }

            // 2. Visual Api
            if (muzzleFlash != null) 
            {
                muzzleFlash.Play();
                Debug.Log("-> Efek Api (Muzzle Flash) dimainkan.");
            }

            // 3. Suara
            if (audioSource != null && fireSound != null) 
            {
                audioSource.PlayOneShot(fireSound);
                Debug.Log("-> Suara tembakan dimainkan.");
            }

            // 4. Raycast
            if (mainCamera == null) return;
            
            Ray ray = mainCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask))
            {
                Debug.Log($"<color=white>[HIT]</color> Mengenai: {hit.transform.name}");
                var target = hit.transform.GetComponent<TargetStrike.Targets.Target>();
                if (target == null) target = hit.transform.GetComponentInParent<TargetStrike.Targets.Target>();
                
                if (target != null)
                {
                    target.TakeDamage(damage);
                    OnHitTarget?.Invoke();
                }
            }
        }

        private void Reload()
        {
            Debug.Log("<color=yellow>[PLAYER SHOOTING]</color> RELOAD!");
            if (weaponAnimator != null) weaponAnimator.SetTrigger("Reload");
        }

        // Compatibility methods
        public void InitializeWeapon(ParticleSystem muzzle, AudioClip sound) { muzzleFlash = muzzle; fireSound = sound; }
        public void SetupWeapon(GameObject weaponObj) { RefreshReferences(); }
    }
}
