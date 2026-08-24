using UnityEngine;

public class ResourceTestAdder : MonoBehaviour
{
    float tick;
    public PlayerResources tempReferenceToPlayerResources;

    private void Update()
    {
        tick += Time.deltaTime;
        if (tick > 1)
        {
            _ = TryAddFleshToPlayer(tempReferenceToPlayerResources);
            tick -= 1;
        }
    }

    public bool TryAddFleshToPlayer(PlayerResources pr)
    {
        if (tempReferenceToPlayerResources == null)
            return false;


        pr.ModifyResource(Resource.Flesh, 10);
        return true;
    } 
}
