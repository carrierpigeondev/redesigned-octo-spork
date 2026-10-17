using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TeleportationArc : MonoBehaviour
{
    [SerializeField] private Transform view;
    [SerializeField] private float launchSpeed = 12f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private int segments = 50;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
    }

    private void LateUpdate()
    {
        line.positionCount = segments + 1;

        Vector3 origin = view.position;
        Vector3 velocity = view.forward * launchSpeed;

        for (int i = 0; i <= segments; i++)
        {
            float t = duration * i / segments;

            Vector3 position =
                origin +
                velocity * t +
                0.5f * gravity * t * t * Vector3.down;

            line.SetPosition(i, position);
        }
    }
}