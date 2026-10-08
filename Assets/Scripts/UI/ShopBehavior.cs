using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBehavior : MonoBehaviour
{
    public GameObject ParentObjectMenu;
    [SerializeField] private Transform target;
    [SerializeField] private PlayerResources playerResources;
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private Button[] buttons;
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

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            foreach (Resource resource in Enum.GetValues(typeof(Resource)))
            {
                buttons[i].gameObject.GetComponent<ResourceTextHolder>().ResourceTexts[(int)resource].text = ((Weapon)i+1).CreatePurchaseInformation().ResourceDictionary[resource].ToString();
            }
        }
    }

    private void Update()
    {
        // check distance from player
        // ignore Y here since Y check is handled CanBeInteractedWith set
        Vector2 targetPosition = new(target.position.x, target.position.z);
        Vector2 transformPosition = new(transform.position.x, transform.position.z);
        
        DistanceToPlayer = Vector2.Distance(targetPosition, transformPosition);

        // only able to be interacted with if in range && the player is above the y level of the shop
        CanBeInteractedWith = (DistanceToPlayer <= usageRange) && (target.position.y >= transform.position.y);
        CloseIfNotInteractable();
    }

    public void TryPurchaseWeapon(int weaponAsInt)
    {
        Weapon weapon = (Weapon)weaponAsInt;
        WeaponPurchaseInformation wpi = weapon.CreatePurchaseInformation();

        foreach (KeyValuePair<Resource, int> r in wpi.ResourceDictionary)
        {
            if (playerResources.WillModifyResultLessThanZero(r.Key, -r.Value))
            {
                Debug.Log("Cannot afford");
                return;
            }
        }
        
        foreach (KeyValuePair<Resource, int> r in wpi.ResourceDictionary)
        {
            playerResources.ModifyResource(r.Key, -r.Value);
        }
        
        playerWeapons.OwnedWeapons.Add(weapon);
        buttons[weaponAsInt - 1].interactable = false;
        buttons[weaponAsInt - 1].gameObject.GetComponent<Image>().color = Color.gray;

        if (wpi.IsAdvanced)
        {
            playerWeapons.OwnedWeapons.Add((Weapon)weaponAsInt-2);
            buttons[weaponAsInt - 3].interactable = false;
            buttons[weaponAsInt - 3].gameObject.GetComponent<Image>().color = Color.gray;
        }
        
        playerWeapons.UpdateToolbar();
    }
}
