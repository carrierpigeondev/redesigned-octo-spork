using Unity.VisualScripting;
using UnityEngine;

public class TurretEnemyBehavior : BillboardingEnemyBehavior
{
    [SerializeField] private const float CAST_LIMIT = 20f;
    [SerializeField] private float distanceAbove = CAST_LIMIT;
    [SerializeField] private float distanceBelow = CAST_LIMIT;
    [SerializeField] private Vector3 beginLocation, endLocation;

    [SerializeField] AnimationCurve stepEase = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    [SerializeField] AnimationCurve stepHeightShape = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepDuration = 0.3f;

    private float stepTime;

    internal override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate();
    }

    internal override void DoEnemyUpdateInRange()
    {
        base.DoEnemyUpdateInRange();
        AdjustHeight();
    }

    internal override void SetResourceDictionary()
    {
        resourceDictionary.Add(Resource.Ruby, 7);
    }

    internal override void SpecificStart()
    {
        beginLocation = billboardImage.transform.position;

        // Checks the bounds to which it can move relative to itself currently
        Debug.Log(distanceAbove);
        RaycastHit hitAbove;
        if (Physics.Raycast(billboardImage.transform.position, Vector3.up, out hitAbove, CAST_LIMIT))
        {
            distanceAbove = hitAbove.distance;
        } // if no hit, stays at 1000, the limit
        Debug.Log(distanceAbove);

        RaycastHit hitBelow;
        if (Physics.Raycast(billboardImage.transform.position, Vector3.down, out hitBelow, CAST_LIMIT))
        {
            distanceBelow = hitBelow.distance;
        } // if no hit, stays at 1000, the limit


        // TEST
        //distanceAbove = 10;

        endLocation = beginLocation;
        endLocation.y += distanceAbove / 2;
    }

    private void AdjustHeight()
    {
        stepTime += Time.deltaTime;
        float progress = stepEase.Evaluate(stepTime / stepDuration);
        ///Debug.Log(progress);

        Vector3 newPos = Vector3.Lerp(beginLocation, endLocation, progress);
        newPos.y += stepHeight * stepHeightShape.Evaluate(progress);

        billboardImage.transform.position = newPos;

        if (progress == 1)
        {
            Vector3 t = endLocation;
            endLocation = beginLocation;
            beginLocation = t;
            stepTime = 0;
        }
    }
}
