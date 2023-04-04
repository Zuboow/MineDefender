using System;
using System.Collections.Generic;
using TowerDefence.GameManagement;
using TowerDefence.Skills;
using TowerDefence.SoundManagement;
using TowerDefence.UI;
using UnityEngine;

namespace TowerDefence.Towers
{
    public class TowerManager : SingletonMonoBehaviour<TowerManager>
    {
        public event Action OnTowerSelection = delegate { };

        [field: Header("ObjectReferences")]
        [field: SerializeField]
        private TowerController CurrentTowerBlueprint { get; set; }
        [field: SerializeField]
        private List<TowerController> TowerCollection { get; set; } = new List<TowerController>();
        [field: SerializeField]
        private Transform SpawnedTowerContainer { get; set; }
        [field: SerializeField]
        private ObjectSoundPlayer TowerManagerSoundPlayer { get; set; }
        [field: SerializeField]
        private BoolSharedVariable IsRaycastingUI { get; set; }

        [field: SerializeField]
        private int TowerSelectionMouseButtonIndex { get; set; }

        private KeyCode PlaceCancellingKey { get; set; } = KeyCode.Escape;
        private TowerController CurrentTowerPrefab { get; set; }

        public void StartPlacingTower(TowerController towerPrefab)
        {
            NotifyOnTowerSelection();

            if (IsTowerBuildingModeEnabled() == true)
            {
                RemoveTowerBlueprint();
            }

            PrepareTowerBlueprint(towerPrefab);
        }

        public void RemoveTowerBlueprint()
        {
            if (CurrentTowerBlueprint != null)
            {
                Destroy(CurrentTowerBlueprint.gameObject);
            }

            RemoveTowerReferences();
        }

        public bool IsTowerBuildingModeEnabled()
        {
            return CurrentTowerBlueprint != null;
        }

        protected virtual void Start()
        {
            AttachToEvents();
        }

        protected virtual void OnDestroy()
        {
            DetachFromEvents();
        }

        protected virtual void Update()
        {
            if (IsBuildCancellationKeyDown() == true && IsTowerBuildingModeEnabled() == true)
            {
                RemoveTowerBlueprint();
            }

            if (IsTowerPlacingButtonDown() == true && IsTowerReadyToPlace() == true && IsRaycastingUI.CurrentValue == false)
            {
                if (GameManager.Instance.CanAffordBuildingTower(CurrentTowerPrefab) == true)
                {
                    PlaceTower();
                    PayForPlacingTower(CurrentTowerPrefab.TowerCost);
                    TowerManagerSoundPlayer.PlaySound();

                    RemoveTowerReferences();
                }
            }
        }

        private bool IsTowerReadyToPlace()
        {
            return (IsTowerBuildingModeEnabled() == true && CurrentTowerBlueprint.CanTowerBePlacedInCurrentLocation() == true);
        }

        private bool IsTowerPlacingButtonDown()
        {
            return Input.GetMouseButtonDown(TowerSelectionMouseButtonIndex) == true;
        }

        private bool IsBuildCancellationKeyDown()
        {
            return Input.GetKeyDown(PlaceCancellingKey) == true;
        }

        private void PrepareTowerBlueprint(TowerController towerPrefab)
        {
            CurrentTowerBlueprint = Instantiate(towerPrefab, SpawnedTowerContainer);
            CurrentTowerPrefab = towerPrefab;

            SetCurrentTowerAsBlueprint();
        }

        private void PlaceTower()
        {
            SetCurrentTowerAsPlaced();
            CurrentTowerBlueprint.PreparePlacedTower(CurrentTowerPrefab);

            TowerCollection.Add(CurrentTowerBlueprint);
        }

        private void PayForPlacingTower(int amount)
        {
            GameManager.Instance.RemoveCoins(amount);
        }

        private void SetCurrentTowerAsBlueprint()
        {
            CurrentTowerBlueprint.MarkAsBlueprint();
        }

        private void SetCurrentTowerAsPlaced()
        {
            CurrentTowerBlueprint.MarkAsPlaced();
        }

        private void RemoveTowerReferences()
        {
            CurrentTowerBlueprint = null;
            CurrentTowerPrefab = null;
        }

        private void RemoveAllTowers()
        {
            for (int index = 0; index < TowerCollection.Count; index++)
            {
                Destroy(TowerCollection[index].gameObject);
            }
        }

        private void HandleGameEnd(int playerHealth)
        {
            if (IsTowerBuildingModeEnabled() == true)
            {
                RemoveTowerBlueprint();
            }

            RemoveAllTowers();
        }

        private void NotifyOnTowerSelection()
        {
            OnTowerSelection.Invoke();
        }

        private void AttachToEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameEnd += HandleGameEnd;
            }

            if (CanvasPanelController.Instance != null)
            {
                CanvasPanelController.Instance.OnMenuPopup += RemoveTowerBlueprint;
            }

            if (SkillsManager.Instance != null)
            {
                SkillsManager.Instance.OnSkillSelection += RemoveTowerBlueprint;
            }
        }

        private void DetachFromEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameEnd -= HandleGameEnd;
            }

            if (CanvasPanelController.Instance != null)
            {
                CanvasPanelController.Instance.OnMenuPopup -= RemoveTowerBlueprint;
            }

            if (SkillsManager.Instance != null)
            {
                SkillsManager.Instance.OnSkillSelection -= RemoveTowerBlueprint;
            }
        }
    }
}