using UnityEngine;

public class EnemyPlayerInteractionInterfaceBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTransformSet;
    [SerializeField] private PlayerResources playerResourcesSet;

    private void Awake()
    {
        playerTransform = playerTransformSet;
        playerResources = playerResourcesSet;
    }

    public static Transform playerTransform;
    public static PlayerResources playerResources;
}
