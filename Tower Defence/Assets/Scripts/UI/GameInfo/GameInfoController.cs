using TMPro;
using UnityEngine;
using TowerDefence.GameManagement;

namespace TowerDefence.UI
{
    public class GameInfoController : MonoBehaviour
    {
        [field: Header("Object References")]
        [field: SerializeField]
        private TextMeshProUGUI HealthAmount { get; set; }
        [field: SerializeField]
        private TextMeshProUGUI CoinsAmount { get; set; }
        [field: SerializeField]
        private TextMeshProUGUI EnemiesCount { get; set; }

        [field: Space]
        [field: SerializeField]
        private float TimeToSwitchColors { get; set; }
        [field: SerializeField]
        private Color32 OriginalColor { get; set; }
        [field: SerializeField]
        private Color32 InsufficientFundsColor { get; set; }

        private float CurrentTimeToSwitchColors { get; set; }
        private bool IsChangingCoinsColor { get; set; }

        protected virtual void Start()
        {
            UpdateValues(GameManager.Instance);

            AddListenerToGameManager();
        }

        protected virtual void OnDisable()
        {
            RemoveListenerFromGameManager();
        }

        protected virtual void Update()
        {
            if (IsChangingCoinsColor == true)
            {
                SwitchBackToOriginalCoinsTextColor();
            }
        }

        private void UpdateValues(GameManager gameManager)
        {
            if (gameManager != null)
            {
                HealthAmount.text = gameManager.PlayerHealth.ToString();
                CoinsAmount.text = gameManager.Coins.ToString();
                EnemiesCount.text = gameManager.EnemiesCount.ToString();
            }
        }

        private void StartChangingCoinsTextColor(GameManager gameManager)
        {
            if (gameManager != null)
            {
                CoinsAmount.color = InsufficientFundsColor;
                IsChangingCoinsColor = true;
                CurrentTimeToSwitchColors = 0f;
            }
        }

        private void SwitchBackToOriginalCoinsTextColor()
        {
            if (CoinsAmount != null)
            {
                if (CurrentTimeToSwitchColors < TimeToSwitchColors)
                {
                    CurrentTimeToSwitchColors += Time.deltaTime;
                    CoinsAmount.color = Color32.Lerp(InsufficientFundsColor, OriginalColor, CurrentTimeToSwitchColors / TimeToSwitchColors);
                }
                else
                {
                    IsChangingCoinsColor = false;
                    CurrentTimeToSwitchColors = 0f;
                }
            }
        }

        private void AddListenerToGameManager()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnValuesChange += UpdateValues;
                GameManager.Instance.OnInsufficientFunds += StartChangingCoinsTextColor;
            }
        }

        private void RemoveListenerFromGameManager()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnValuesChange -= UpdateValues;
                GameManager.Instance.OnInsufficientFunds -= StartChangingCoinsTextColor;
            }
        }
    }
}
