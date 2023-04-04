using System;
using System.Collections.Generic;
using TowerDefence.GameManagement;
using TowerDefence.SoundManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefence.UI
{
    public class CanvasPanelController : SingletonMonoBehaviour<CanvasPanelController>
    {
        public event Action OnMenuPopup = delegate { };

        [field: Header("Object References")]
        [field: SerializeField]
        public GraphicRaycaster Raycaster { get; private set; }
        [field: SerializeField]
        public EventSystem Events { get; private set; }
        [field: SerializeField]
        public AudioClip VictorySound { get; private set; }
        [field: SerializeField]
        public AudioClip DefeatSound { get; private set; }
        [field: SerializeField]
        private BoolSharedVariable IsRaycastingUI { get; set; }
        [field: SerializeField]
        private AudioClipSharedVariable SharedAudioClip { get; set; }

        [field: Header("UI References")]
        [field: SerializeField]
        private Transform GameplayInfoUI { get; set; }
        [field: SerializeField]
        private Transform TowerButtonsUI { get; set; }
        [field: SerializeField]
        private Transform GameSummaryUI { get; set; }
        [field: SerializeField]
        private Transform GameMenuUI { get; set; }
        [field: SerializeField]
        private Transform VictoryText { get; set; }
        [field: SerializeField]
        private Transform DefeatText { get; set; }

        private PointerEventData PointerData { get; set; }
        private List<RaycastResult> RaycastResults { get; set; }

        public void ShowMenuUI()
        {
            GameplayInfoUI.gameObject.SetActive(false);
            TowerButtonsUI.gameObject.SetActive(false);

            GameMenuUI.gameObject.SetActive(true);

            NotifyOnMenuOpen();
        }

        public void ShowGameplayUI()
        {
            GameplayInfoUI.gameObject.SetActive(true);
            TowerButtonsUI.gameObject.SetActive(true);

            GameMenuUI.gameObject.SetActive(false);
        }

        protected virtual void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += ShowGameplayUI;
                GameManager.Instance.OnGameEnd += ShowSummary;
            }

            InitializeObjects();
        }

        protected virtual void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart -= ShowGameplayUI;
                GameManager.Instance.OnGameEnd -= ShowSummary;
            }
        }

        protected virtual void Update()
        {
            CheckIfIsRaycastingUI();
        }

        private void CheckIfIsRaycastingUI()
        {
            PointerData.position = Input.mousePosition;
            RaycastResults = new List<RaycastResult>();

            Raycaster.Raycast(PointerData, RaycastResults);

            if (RaycastResults.Count > 0)
            {
                IsRaycastingUI.CurrentValue = true;
            }
            else
            {
                IsRaycastingUI.CurrentValue = false;
            }
        }

        private void ShowSummary(int playerHealth)
        {
            GameplayInfoUI.gameObject.SetActive(false);
            TowerButtonsUI.gameObject.SetActive(false);
            GameMenuUI.gameObject.SetActive(false);

            GameSummaryUI.gameObject.SetActive(true);

            NotifyOnMenuOpen();

            if (playerHealth > 0)
            {
                DefeatText.gameObject.SetActive(false);
                VictoryText.gameObject.SetActive(true);

                SharedAudioClip.CurrentValue = VictorySound;
            }
            else
            {
                DefeatText.gameObject.SetActive(true);
                VictoryText.gameObject.SetActive(false);

                SharedAudioClip.CurrentValue = DefeatSound;
            }
        }

        private void InitializeObjects()
        {
            PointerData = new PointerEventData(Events);
        }

        private void NotifyOnMenuOpen()
        {
            OnMenuPopup.Invoke();
        }
    }
}