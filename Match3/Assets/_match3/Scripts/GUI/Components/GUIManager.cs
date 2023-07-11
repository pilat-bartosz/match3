using System.Collections.Generic;
using _match3.Game;
using Unity.Collections;
using Unity.Entities;
using Unity.Logging;
using UnityEngine;
using UnityEngine.UI;

namespace _match3.GUI
{
    public class GUIManager : MonoBehaviour, IComponentData
    {
        [Header("Menu")]
        [SerializeField] private GameObject _menuUI;
        [SerializeField] private List<Button> _menuButtons;

        private int _lastButtonClicked;
        private bool _wasButtonClicked;
    
        [Header("Game")]
        [SerializeField] private GameObject _gameUI;
        [SerializeField] private List<Image> _scoreImages;
    
    
        [Header("Congratulations")]
        [SerializeField] private GameObject _congratulationsUI;

        public void Initialize()
        {
            FixCanvasScaleToFitResolution();

            for (var i = 0; i < _menuButtons.Count; i++)
            {
                var menuButton = _menuButtons[i];
                var buttonID = i;
            
                menuButton.onClick.RemoveAllListeners();
                menuButton.onClick.AddListener(() =>
                {
                    MenuButtonClicked(buttonID);
                });
            }

            _wasButtonClicked = false;
            _lastButtonClicked = -1;
        }

        public void FixCanvasScaleToFitResolution()
        {
            Debug.Log("Screen width - " + Camera.main.pixelWidth);
            var canvasScaler = GetComponentInChildren<CanvasScaler>();
            canvasScaler.matchWidthOrHeight = 1 - (float)Camera.main.pixelWidth / 1280;
        }
    
        public void SwitchToUI(GameState uiState, bool activeState = true)
        {
            Log.Debug($"SwitchToUI {uiState}");
            _menuUI.SetActive(activeState && uiState == GameState.Menu);
            _gameUI.SetActive(activeState && uiState == GameState.Game);
            _congratulationsUI.SetActive(activeState && uiState == GameState.Congratulations);
        }
        
        public void UpdateScore(NativeArray<int> scores, NativeArray<int> maxScore)
        {
            for (var i = 0; i < _scoreImages.Count; i++)
            {
                _scoreImages[i].fillAmount = (float) scores[i] / maxScore[i];
            }
        }
        
        private void MenuButtonClicked(int buttonID)
        {
            SetMenuButtons(buttonID, true, false);
            Log.Debug($"Menu Button Clicked {buttonID}");
        }
        
        public bool BeginMenuButtonHandling()
        {
            return _wasButtonClicked;
        }
        
        public int GetLastButtonClicked()
        {
            return _lastButtonClicked;
        }
        
        public void EndMenuButtonHandling()
        {
            SetMenuButtons(-1, false, true);
        }

        private void SetMenuButtons(int value,bool wasButtonClicked, bool buttonsState)
        {
            foreach (var menuButton in _menuButtons)
            {
                menuButton.interactable = buttonsState;
            }

            _wasButtonClicked = wasButtonClicked;
            _lastButtonClicked = value;
        }
    }
}