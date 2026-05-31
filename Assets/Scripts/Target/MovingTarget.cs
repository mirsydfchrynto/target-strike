using UnityEngine;

namespace TargetStrike.Targets
{
    public class MovingTarget : Target
    {
        [Header("Movement Settings")]
        [SerializeField] private float speed = 2f;
        [SerializeField] private float distance = 3f;
        [SerializeField] private Vector3 direction = Vector3.right;

        private Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            float offset = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
            transform.position = startPosition + direction.normalized * offset;
        }
    }
}
