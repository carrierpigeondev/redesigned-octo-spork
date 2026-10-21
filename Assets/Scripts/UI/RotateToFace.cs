using UnityEngine;

public class RotateToFace : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool constrain;

    void Update()
    {
        if (!constrain)
        {
            transform.LookAt(target);
            return;
        } 
        
        Vector3 direction = transform.position - target.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion yRotation = Quaternion.LookRotation(direction);

        transform.rotation = yRotation * Quaternion.Euler(transform.rotation.x, 0f, 0f);
    }
}
