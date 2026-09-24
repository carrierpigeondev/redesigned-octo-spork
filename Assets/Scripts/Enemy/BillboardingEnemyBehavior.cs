using UnityEngine;
using UnityEngine.UI;

public class BillboardingEnemyBehavior : BaseEnemyBehaviour
{
    [SerializeField] private RawImage billboardImage;

    [SerializeField] private TextureManagerAsset textureManager;

    [SerializeField] private string namePrefix;
    [SerializeField] private float lookAngle; // internal way that it faces
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
        //ChangeSprite(Vector3.Angle(Vector3.forward, target.position - this.transform.position));

        // handle simple rotation, this doesn't need to know anything about the internal look angle
        Vector3 dir = target.position - billboardImage.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        billboardImage.transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        float theta = target.rotation.eulerAngles.y - lookAngle;
        Debug.Log(theta);
        ChangeSprite(theta);

        //billboardImage.transform.rotation = Quaternion.Euler(0f, lookAngle, 0f);

        // doesnt behave properly
        //
        // TODO:
        // should behave as if looking in one direction (which is internal?)
        // but appear always facing the player (which is external, i presume)
        // and change sprite based on angle of player to internal direction (so it looks 3d)
    }

    private void ChangeSprite(float a)
    {
        string count = "0";
        switch (a)
        {
            case float t when t <= 22.5 || t >= 337.5:
                count = "0"; break;

            case float t when t <= 67.5:
                count = "1"; break;

            case float t when t <= 112.5:
                count = "2"; break;

            case float t when t <= 157.5:
                count = "3"; break;

            case float t when t <= 202.5:
                count = "4"; break;

            case float t when t <= 247.5:
                count = "5"; break;

            case float t when t <= 292.5:
                count = "6"; break;

            case float t when t <= 337.5:
                count = "7"; break;
        }
        textureIndex = textureManager[$"{namePrefix}_{count}"];
        billboardImage.texture = textureManager.GetTexture(textureIndex);
    }
}
