using UnityEngine;

public class EnemyPlayerInteractionInterfaceBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTransformSet;
    [SerializeField] private PlayerResources playerResourcesSet;
    [SerializeField] private Camera playerCameraSet;

    private void Awake()
    {
        playerTransform = playerTransformSet;
        playerResources = playerResourcesSet;
        playerCamera = playerCameraSet;
    }

    public static Transform playerTransform;
    public static PlayerResources playerResources;
    public static Camera playerCamera;
}
