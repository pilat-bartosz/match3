using _match3.Managers;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour, IComponentData
{
    [Header("Menu")]
    [SerializeField] private GameObject MenuUI;
    
    [Header("Game")]
    [SerializeField] private GameObject GameUI;
    
    [Header("Congratulations")]
    [SerializeField] private GameObject CongratulationsUI;

    private Entity guiEntity;

    private void Awake()
    {
        var entityManger = World.DefaultGameObjectInjectionWorld.EntityManager;
        guiEntity = entityManger.CreateEntity();
        entityManger.AddComponentObject(guiEntity, this);

        Initialize();
    }

    private void Initialize()
    {
        FixCanvasScaleToFitResolution();
    }

    private void OnDestroy()
    {
        //World.DefaultGameObjectInjectionWorld?.EntityManager.DestroyEntity(guiEntity);
    }

    public void FixCanvasScaleToFitResolution()
    {
        Debug.Log("Screen width - " + Camera.main.pixelWidth);
        var canvasScaler = GetComponentInChildren<CanvasScaler>();
        canvasScaler.matchWidthOrHeight = 1 - (float)Camera.main.pixelWidth / 1280;
    }
    
    public void SwitchToUI(GameState uiState, bool activeState = true)
    {
        MenuUI.SetActive(activeState && uiState == GameState.Menu);
        GameUI.SetActive(activeState && uiState == GameState.Game);
        CongratulationsUI.SetActive(activeState && uiState == GameState.Congratulations);
    }

    public void MenuButtonClicked(int lenght)
    {
        
    }
}