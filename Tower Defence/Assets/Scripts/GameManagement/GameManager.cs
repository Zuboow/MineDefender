using UnityEngine;
using TowerDefence.Towers;
using System;

namespace TowerDefence.GameManagement
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public event Action<GameManager> OnValuesChange = delegate { };
        public event Action<GameManager> OnInsufficientFunds = delegate { };

        public event Action OnGameStart = delegate { };
        public event Action<int> OnGameEnd = delegate { };

        [field: Header("Settings")]
        [field: SerializeField]
        public int PlayerHealth { get; private set; }
        [field: SerializeField]
        public int Coins { get; private set; }
        [field: SerializeField]
        public int EnemiesCount { get; private set; }

        public void TakeDamage(int damageTaken)
        {
            PlayerHealth -= damageTaken;

            if (PlayerHealth <= 0)
            {
                PlayerHealth = 0;

                NotifyOnGameEnd();
            }

            NotifyOnValuesChange();
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            NotifyOnValuesChange();
        }

        public void RemoveCoins(int amount)
        {
            Coins -= amount;
            NotifyOnValuesChange();
        }

        public void IncreaseEnemyCounter(int amount)
        {
            EnemiesCount += amount;
            NotifyOnValuesChange();
        }

        public void DecreaseEnemyCounter(int amount)
        {
            EnemiesCount -= amount;
            NotifyOnValuesChange();

            if (EnemiesCount == 0)
            {
                NotifyOnGameEnd();
            }
        }

        public bool CanAffordBuildingTower(TowerController tower)
        {
            if (tower.TowerCost <= Coins)
            {
                return true;
            }
            else
            {
                NotifyOnInsufficientFunds();
                return false;
            }
        }

        protected virtual void Start()
        {
            NotifyOnValuesChange();
            NotifyOnGameStart();
        }

        private void NotifyOnValuesChange()
        {
            OnValuesChange.Invoke(Instance);
        }

        private void NotifyOnInsufficientFunds()
        {
            OnInsufficientFunds.Invoke(Instance);
        }

        private void NotifyOnGameStart()
        {
            OnGameStart.Invoke();
        }

        private void NotifyOnGameEnd()
        {
            OnGameEnd.Invoke(PlayerHealth);
        }
    }
}