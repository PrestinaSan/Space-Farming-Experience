using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform target;

    [Header("Attributes")]
    private Vector3 vel = Vector3.zero;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    private void FixedUpdate()
    {
        Vector3 targetPos = target.position + offset;
        targetPos.z = transform.position.z;
            
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, damping);
    }

}
