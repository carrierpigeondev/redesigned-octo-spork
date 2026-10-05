using UnityEngine;
using UnityEngine.UI;

public class BillboardingEnemyBehavior : BaseEnemyBehaviour
{
    public float MoveSpeed = 2;

    [SerializeField] internal RawImage billboardImage;

    [SerializeField] internal TextureManagerAsset textureManager;

    [SerializeField] internal string namePrefix;
    [SerializeField] internal float lookAngle; // internal way that it faces
    private byte textureIndex;

    private int previousDirection = -1;

    internal override void OnEnemyUpdate()
    {
        DoEnemyUpdate();

        // the GeneralRange check in question:
        if (Vector3.Distance(billboardImage.transform.position, target.position) < GeneralRange)
        {
            DoEnemyUpdateInRange();
        } else
        {
            DoEnemyUpdateOutOfRange();
        }
    }

    /// <summary>
    /// Method called OnEnemyUpdate prior to GeneralRange check
    /// </summary>
    internal virtual void DoEnemyUpdate()
    {
        LookAt(this.target.position); // must always happen for rendering to work properly
    }

    /// <summary>
    /// Method called OnEnemyUpdate if GeneralRange check passes
    /// </summary>
    internal virtual void DoEnemyUpdateInRange()
    {
        ChangeLookAngleToward(target.position);
    }

    /// <summary>
    /// Method called OnEnemyUpdate if GeneralRange check fails
    /// </summary>
    internal virtual void DoEnemyUpdateOutOfRange() { }

    internal override void SetResourceDictionary()
    {
        resourceDictionary.Add(Resource.Ruby, 5);
    }

    internal override void SetInternalPlayerValues()
    {
        base.SetInternalPlayerValues();
    }

    internal override void LookAt(Vector3 position)
    {
        // handle simple rotation, this doesn't need to know anything about the internal look angle
        Vector3 dir = target.position - billboardImage.transform.position;
        billboardImage.transform.rotation = Quaternion.LookRotation(dir);

        // calculates the difference with camera look (way the player is rotated) and way the bill
        // is facing itself, and use that to determine what sprite to use
        // properly works with non 0 lookAngles too (which is necessary for the internal rotation thing to work)
        float theta = lookAngle - target.rotation.eulerAngles.y;
        if (theta < 0) theta += 360;

        ChangeSprite(theta);
    }

    internal virtual void ChangeLookAngleToward(Vector3 targetVector)
    {
        Vector3 direction = targetVector - billboardImage.transform.position;
        float angle = Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg;
        lookAngle = angle;
    }

    internal virtual Vector3 Forward()
    {
        float lookRadians = lookAngle * Mathf.PI / 180.0f;
        return new Vector3(-Mathf.Sin(lookRadians), 0, -Mathf.Cos(lookRadians));
    }

    private void ChangeSprite(float angle)
    {
        angle = Mathf.Repeat(angle, 360f);
        
        int direction = Mathf.FloorToInt((angle + 22.5f) / 45f) % 8;
        if (direction == previousDirection) return; // save on getting texture when not needed
        previousDirection = direction;
        
        textureIndex = textureManager[$"{namePrefix}_{direction}"];
        billboardImage.texture = textureManager.GetTexture(textureIndex);
    }
}
