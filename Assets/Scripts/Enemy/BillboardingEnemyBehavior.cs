using UnityEngine;
using UnityEngine.UI;

public class BillboardingEnemyBehavior : BaseEnemyBehaviour
{
    [SerializeField] private RawImage billboardImage;

    [SerializeField] private TextureManagerAsset textureManager;
    private byte textureIndex;

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
        switch (target.eulerAngles.y)
        {
            case float t when t <= 45 || t >= 315:
                textureIndex = textureManager["bill_front"];
                break;

            case float t when t <= 135:
                textureIndex = textureManager["bill_side_left"];
                break;

            case float t when t <= 225:
                textureIndex = textureManager["bill_back"];
                break;

            case float t when t < 315:
                textureIndex = textureManager["bill_side_right"];
                break;
        }

        billboardImage.texture = textureManager.GetTexture(textureIndex);
    }
}
