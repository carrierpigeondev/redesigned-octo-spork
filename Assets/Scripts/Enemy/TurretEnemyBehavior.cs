using System;
using Unity.VisualScripting;
using UnityEngine;

public class TurretEnemyBehavior : BillboardingEnemyBehavior
{
    [SerializeField] private float fireCooldown;
    [SerializeField] private float fireTimer;
    [SerializeField] private float fireRange;
    [SerializeField] private float rotSpeed;
    
    internal override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate(); // only handles billboarding
    }

    internal override void DoEnemyUpdateInRange()
    {
        AimAtPlayer();
        fireTimer -= Time.deltaTime;
        
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);
        if (distanceToPlayer <= fireRange && fireTimer <= 0 && CanSeePlayer())
        {
            Fire();
            fireTimer = fireCooldown;
        }
    }

    internal override void SetResourceDictionary()
    {
        resourceDictionary.Add(Resource.Ruby, 7);
    }

    internal override void SpecificStart()
    {
        
    }

    private void AimAtPlayer()
    {
        Vector3 direction = target.position - transform.position;

        float angle = Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg;
        lookAngle = Mathf.MoveTowardsAngle(lookAngle, angle, rotSpeed * Time.deltaTime);
    }

    private bool CanSeePlayer()
    {
        Vector3 direction = target.position - transform.position;
        float distanceToPlayer = direction.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, distanceToPlayer))
        {
            return hit.transform == target;
        }

        return false;
    }

    private void Fire()
    {
        
        
    }
}
