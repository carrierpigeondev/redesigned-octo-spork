using UnityEngine;

public abstract class BaseEnemyBehaviour : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    public float GeneralRange;
    public Transform target;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        OnEnemyUpdate();
    }

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

    internal abstract void OnEnemyUpdate();

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
    }
}
