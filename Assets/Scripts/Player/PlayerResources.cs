using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private ResourceUIParentBehaviour resourceUi;
    private Dictionary<Resource, int> resourceDictionary = new Dictionary<Resource, int>();

    private void Start()
    {
        // populate dictionary with all Resource types at start
        foreach (Resource resource in Enum.GetValues(typeof(Resource)))
        {
            resourceDictionary.Add(resource, 0);
            resourceUi.ResourceTexts[(int)resource].text = $"{resourceDictionary[resource]}";
        }
    }

    private void Update()
    {
        // because Dictionary isn't serializable, debug info goes here :D
        //if (DebugToggle.DebugToggleEnabled)
            //foreach (KeyValuePair<Resource, int> kvp in resourceDictionary)
            //    Debug.Log($"{kvp.Key} :: {kvp.Value}");
    }

    public int GetResource(Resource resource)
    {
        return resourceDictionary[resource];
    }

    public bool WillModifyResultLessThanZero(Resource resource, int amount)
    {
        int resourceAmount = GetResource(resource);
        if (resourceAmount + amount < 0) { return true; }
        return false;
    }

    public void ModifyResource(Resource resource, int amount)
    {
        resourceDictionary[resource] += amount;
        resourceUi.ResourceTexts[(int)resource].text = $"{resourceDictionary[resource]}";
    }
}
