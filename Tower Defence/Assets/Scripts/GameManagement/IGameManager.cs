using TowerDefence.Towers;

namespace TowerDefence.GameManagement
{
    public interface IGameManager
    {
        void TakeDamage(int damageTaken);

        void AddCoins(int amount);
        
        void RemoveCoins(int amount);

        void IncreaseEnemyCounter(int amount);

        void DecreaseEnemyCounter(int amount);

        bool CanAffordBuildingTower(TowerController tower);
    }
}
