using TMPro;
using TowerDefence.SoundManagement;
using UnityEngine;

namespace TowerDefence.Towers.Buttons
{
    public class TowerButtonController : MonoBehaviour
    {
        [field: SerializeField]
        private TowerController TowerPrefab { get; set; }
        [field: SerializeField]
        private TextMeshProUGUI TowerCostText { get; set; }
        [field: SerializeField]
        private ObjectSoundPlayer TowerButtonSoundPlayer { get; set; }

        protected virtual void Awake()
        {
            SetTowerValueText();
        }

        public void SelectTowerForBuilding()
        {
            TowerButtonSoundPlayer.PlaySound();

            if (TowerPrefab != null)
            {
                TowerManager.Instance.StartPlacingTower(TowerPrefab);
            }
        }

        private void SetTowerValueText()
        {
            TowerCostText.text = TowerPrefab.TowerCost.ToString();
        }
    }
}
