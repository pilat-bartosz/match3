using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace _match3.GUI
{
    public class GUIManager : MonoBehaviour, IComponentData
    {
        public enum GUIState
        {
            Uninitialized,
            Menu,
            Game,
            Congratulations,
        }

        [Header("Menu")]
        [SerializeField] private GameObject _menuUI;
        [SerializeField] private List<Button> _menuButtons;

        private int _lastButtonClicked;
        private bool _wasMenuButtonClicked;

        [Header("Game")]
        [SerializeField] private GameObject _gameUI;
        [SerializeField] private Button _returnToMenuButton;
        [SerializeField] private List<Image> _scoreImages;

        public bool WasReturnToMenuButtonClicked { get; set; }

        [Header("Congratulations")]
        [SerializeField] private GameObject _congratulationsUI;

        private GUIState internalUIState;

        public void Initialize(GUIState firstState = GUIState.Uninitialized)
        {
            FixCanvasScaleToFitResolution();

            internalUIState = firstState;
            SwitchToUI(firstState);
            
            //Initialize Menu
            for (var i = 0; i < _menuButtons.Count; i++)
            {
                var menuButton = _menuButtons[i];
                var buttonID = i;

                menuButton.onClick.RemoveAllListeners();
                menuButton.onClick.AddListener(() => { MenuButtonClicked(buttonID); });
            }

            _wasMenuButtonClicked = false;
            _lastButtonClicked = -1;

            //Initialize Game gui
            _returnToMenuButton.onClick.RemoveAllListeners();
            _returnToMenuButton.onClick.AddListener(() => { WasReturnToMenuButtonClicked = true; });
            WasReturnToMenuButtonClicked = false;
        }

        public void FixCanvasScaleToFitResolution()
        {
            var canvasScaler = GetComponentInChildren<CanvasScaler>();
            canvasScaler.matchWidthOrHeight = 1 - (float)Camera.main.pixelWidth / 1280;

            var canvas = GetComponentInChildren<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 11f;
        }

        public void UpdateUI(GUIState newState, bool force = false)
        {
            if(internalUIState == newState && !force) return;

            SwitchToUI(newState);
            internalUIState = newState;
        }

        private void SwitchToUI(GUIState uiState)
        {
            _menuUI.SetActive(uiState == GUIState.Menu);
            _gameUI.SetActive(uiState == GUIState.Game);
            _congratulationsUI.SetActive(uiState == GUIState.Congratulations);
        }

        public void UpdateScore(NativeArray<int> scores, NativeArray<int> maxScore)
        {
            for (var i = 0; i < _scoreImages.Count; i++)
            {
                _scoreImages[i].fillAmount = (float)scores[i] / maxScore[i];
            }
        }

        private void MenuButtonClicked(int buttonID)
        {
            SetMenuButtons(buttonID, true, false);
        }

        public bool BeginMenuButtonHandling()
        {
            return _wasMenuButtonClicked;
        }

        public int GetLastButtonClicked()
        {
            return _lastButtonClicked;
        }

        public void EndMenuButtonHandling()
        {
            SetMenuButtons(-1, false, true);
        }

        private void SetMenuButtons(int value, bool wasButtonClicked, bool buttonsState)
        {
            foreach (var menuButton in _menuButtons)
            {
                menuButton.interactable = buttonsState;
            }

            _wasMenuButtonClicked = wasButtonClicked;
            _lastButtonClicked = value;
        }
    }
}