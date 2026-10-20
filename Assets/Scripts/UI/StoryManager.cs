using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public enum StoryState
{
    _00_Beginning,
    _01_GoToShopZone,
    _02_AlienLife,
    _03_ContinueResources,
    _04_Sleep
}

public class StoryManager : MonoBehaviour
{
    public static StoryState CurrentState = StoryState._00_Beginning;
    public static bool HasChangedToDoom;
    
    [SerializeField] private GameObject ParentObject_00_Beginning_Text; // needs to be disabled after _00_Beginning is done
    [SerializeField] private GameObject ParentObject_01_GoToShopZone_Text;
    [SerializeField] private GameObject ParentObject_02_AlienLife_Text;
    [SerializeField] private GameObject ParentObject_03_ContinueResources_Text;
    [SerializeField] private GameObject ParentObject_04_Sleep_Text;

    [SerializeField] private World w;
    
    [SerializeField] private Material doomSkybox;
    [SerializeField] private TMP_Text WarningText;
    
    [SerializeField] private float doomTimer;
        
    void Start()
    {
        CurrentState = StoryState._00_Beginning; // enforce beginning,
    }

    void Update()
    {
        ParentObject_00_Beginning_Text.SetActive(CurrentState == StoryState._00_Beginning);
        ParentObject_01_GoToShopZone_Text.SetActive(CurrentState == StoryState._01_GoToShopZone);
        ParentObject_02_AlienLife_Text.SetActive(CurrentState == StoryState._02_AlienLife);
        ParentObject_03_ContinueResources_Text.SetActive(CurrentState == StoryState._03_ContinueResources);
        ParentObject_04_Sleep_Text.SetActive(CurrentState == StoryState._04_Sleep);

        if (CurrentState == StoryState._04_Sleep && !HasChangedToDoom)
        {
            doomTimer += Time.deltaTime;
        }

        if (doomTimer > 30 && !HasChangedToDoom)
        {
            ChangeSkyboxDoom();
            HasChangedToDoom = true;
            w.SpawnEnemies(10, 20);
        }
    }

    void ChangeSkyboxDoom()
    {
        RenderSettings.skybox = doomSkybox;
        DynamicGI.UpdateEnvironment();

        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = Color.black;
        RenderSettings.ambientIntensity = 0f;

        RenderSettings.reflectionIntensity = 0f;
        
        WarningText.SetText("Planetary Anomaly Bounds Will No Longer Phase You To Surface.");
    }
}
