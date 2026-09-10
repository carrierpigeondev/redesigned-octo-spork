using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyBehaviour : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    public float GeneralRange;
    [SerializeField] internal Transform target;
    [SerializeField] internal PlayerResources playerResources;
    internal Dictionary<Resource, int> resourceDictionary = new Dictionary<Resource, int>(); // must be overridden in code and not inspector bc not serializeable

    private void Start()
    {
        CurrentHealth = MaxHealth;
        SetResourceDictionary();

        target = EnemyPlayerInteractionInterfaceBehavior.playerTransform;
        playerResources = EnemyPlayerInteractionInterfaceBehavior.playerResources;
    }

    private void Update()
    {
        OnEnemyUpdate();
    }

    internal abstract void OnEnemyUpdate();

    /// <summary>
    /// Should be called outside of enemy, such as by player, to enable/disable
    /// this game object when in/out of range.
    /// 
    /// Handle internal logic within the OnEnemyUpdate method.
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public virtual bool IsWithinGeneralRange(Vector3 position)
    {
        return Vector3.Distance(position, this.transform.position) <= GeneralRange;
    }

    internal virtual void SetResourceDictionary()
    {
        // not necessary to override, which is why not abstract
    }

    internal virtual void LookAt(Vector3 position)
    {
        this.transform.LookAt(position);
    }

    internal virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    internal virtual void Die()
    {
        Destroy(gameObject);
        foreach (KeyValuePair<Resource, int> kvp in resourceDictionary) {
            if (playerResources.WillModifyResultLessThanZero(kvp.Key, kvp.Value))
            {
                Debug.LogWarning($"Player resource {kvp.Key} would go below zero. Not modifying.");
                continue;
            }
            playerResources.ModifyResource(kvp.Key, kvp.Value);
        }
    }
}
