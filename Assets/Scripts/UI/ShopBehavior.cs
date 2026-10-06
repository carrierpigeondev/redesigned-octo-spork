using UnityEngine;

public class ShopBehavior : MonoBehaviour
{
    [SerializeField] private GameObject ParentObjectMenu;
    [SerializeField] private Transform target;
    [SerializeField] private float usageRange;
    public float DistanceToPlayer;
    public bool CanBeInteractedWith;

    // called externally
    public void ToggleMenu()
    {
        ParentObjectMenu.SetActive(!ParentObjectMenu.activeSelf);
    }
    public void ToggleMenu(bool state)
    {
        ParentObjectMenu.SetActive(state);
    }

    private void CloseIfNotInteractable()
    {
        if (!CanBeInteractedWith && ParentObjectMenu.activeSelf)
        {
            ParentObjectMenu.SetActive(false);
        }
    }

    private void Update()
    {
        // check distance from player
        DistanceToPlayer = Vector3.Distance(target.position, transform.position);

        // only able to be interacted with if in range && the player is above the y level of the shop
        CanBeInteractedWith = (DistanceToPlayer <= usageRange) && (target.position.y >= transform.position.y);
        CloseIfNotInteractable();
    }
}
