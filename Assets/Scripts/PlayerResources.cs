using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    private Dictionary<Resource, int> resourceDictionary = new Dictionary<Resource, int>();

    private void Start()
    {
        // populate dictionary with all Resource types at start
        foreach (Resource resource in Enum.GetValues(typeof(Resource)))
            resourceDictionary.Add(resource, 0);
    }

    private void Update()
    {
        // because Dictionary isn't serializable, debug info goes here :D
        // TODO: do that
    }

    public int GetResource(Resource resource)
    {
        return resourceDictionary[resource];
    }

    public bool WillModifyResultLessThanZero(Resource resource, int amount)
    {
        int resourceAmount = GetResource(resource);
        if (resourceAmount + amount < 0) { return false; }
        return true;
    }

    public void ModifyResource(Resource resource, int amount)
    {
        resourceDictionary[resource] += amount;
    }
}
