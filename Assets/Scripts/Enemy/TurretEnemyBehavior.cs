using UnityEngine;

public class TurretEnemyBehavior : BaseEnemyBehaviour
{
    //public Transform Pivot;
    public Transform TurretHead;

    internal override void OnEnemyUpdate()
    {
        // aim at player
        LookAt(this.target.position);
    }

    internal override void SetResourceDictionary()
    {
        resourceDictionary.Add(Resource.Ruby, 5);
    }

    internal override void LookAt(Vector3 position)
    {
        // normal rotation doesnt work the same for a turret
        // only want to rotate the head of the turret, and limit it

        //TurretHead.RotateAround(Pivot.position, Vector3.up, 45f * Time.deltaTime);
        TurretHead.LookAt(position);
    }
}
