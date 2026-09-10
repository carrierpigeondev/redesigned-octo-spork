using UnityEngine;

public class BillboardingEnemyBehavior : BaseEnemyBehaviour
{
    [SerializeField] private Transform billboardSpriteTransform;

    internal override void OnEnemyUpdate()
    {
        LookAt(this.target.position);
    }

    internal override void SetResourceDictionary()
    {
        resourceDictionary.Add(Resource.Ruby, 5);
    }

    internal override void SetInternalPlayerValues()
    {
        base.SetInternalPlayerValues();
        //playerCamera = EnemyPlayerInteractionInterfaceBehavior.playerCamera;
    }

    internal override void LookAt(Vector3 position)
    {
        Debug.Log(target.eulerAngles.y); // need to check ranges, then change the sprite based on the direction
    }
}
