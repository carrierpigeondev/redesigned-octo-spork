using UnityEngine;

public class FloaterEnemyBehavior : BillboardingEnemyBehavior
{
    internal override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate();
    }

    internal override void DoEnemyUpdateInRange()
    {
        base.DoEnemyUpdateInRange();

        transform.Translate(MoveSpeed * Time.deltaTime * Forward());

        // floater want to intelligently adjust height
        AdjustHeight();
    }


    internal override void SetResourceDictionary()
    {
        resourceDictionary.Add(Resource.Ruby, 8);
    }

    private void AdjustHeight()
    {
        float playerHeight = target.transform.position.y;
        float offset = 10; // be level with the player, for now
        float newHeightTarget = playerHeight + offset;

        float limit = 1000;
        float bufferFromGround = 5f;
        RaycastHit hit;
        
        Vector3 direction = Vector3.zero;

        float distance = limit;

        if (billboardImage.transform.position.y > newHeightTarget)
        {
            // must move down
            direction = Vector3.down;
        } else if (billboardImage.transform.position.y < newHeightTarget)
        {
            // must move up
            direction = Vector3.up;
        } // also can keep at zeros for no movement

        if (Physics.Raycast(transform.position, direction, out hit, limit))
        {
            distance = hit.distance;
        } // if no hit, stays at 1000, the limit

        if (distance < bufferFromGround)
        {
            newHeightTarget += (bufferFromGround - distance) * -direction.y;
        }

        Debug.Log(newHeightTarget);
        Vector3 a = billboardImage.transform.position;
        a.y = newHeightTarget;
        billboardImage.transform.position = a;
    }
}
