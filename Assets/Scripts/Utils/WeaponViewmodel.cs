using UnityEngine;

namespace TargetStrike.Utils
{
    public class WeaponViewmodel : MonoBehaviour
    {
        [Header("Positioning")]
        [SerializeField] private Vector3 localPosition = new Vector3(0.3f, -0.4f, 0.5f);
        [SerializeField] private Vector3 localRotation = Vector3.zero;

        private void Start()
        {
            // Force correct positioning in first person view
            transform.localPosition = localPosition;
            transform.localRotation = Quaternion.Euler(localRotation);
        }
    }
}
