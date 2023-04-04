using System.Collections.Generic;
using UnityEngine;
using TowerDefence.GameManagement;

namespace TowerDefence.Entities.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [field: Header("Object References")]
        [field: SerializeField]
        private List<Enemy> EnemyPrefabCollection { get; set; } = new List<Enemy>();
        [field: SerializeField]
        private List<Enemy> SpawnedEnemyCollection { get; set; } = new List<Enemy>();
        [field: SerializeField]
        private Transform SpawnPoint { get; set; }
        [field: SerializeField]
        private Transform EndPoint { get; set; }
        [field: SerializeField]
        private Transform EntitiesContainer { get; set; }
        [field: SerializeField]
        private SettingsData GameSettings { get; set; }

        [field: Header("Settings")]
        [field: SerializeField]
        private float SpawnDelay { get; set; } = 1f;
        [field: SerializeField]
        private int SpawningAmount { get; set; } = 50;
        [field: SerializeField]
        private float MaxSpawnDelay { get; set; }
        [field: SerializeField]
        private float MinSpawnDelay { get; set; }
        [field: SerializeField]
        private float TimeToReachMinSpawnDelay { get; set; }
        [field: SerializeField]
        private bool SpawningEnabled { get; set; }

        private float TimeUntilSpawn { get; set; } = 0f;
        private int SpawningIndex { get; set; } = 0;
        private float CurrentTimeToMinSpawnDelay { get; set; }

        private KeyCode SpawnEnemyKey { get; set; } = KeyCode.K;
        private KeyCode DestroyEnemiesKey { get; set; } = KeyCode.L;

        protected virtual void Start()
        {
            InitializeValues();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameEnd += HandleGameEnd;
            }
        }

        protected virtual void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameEnd -= HandleGameEnd;
            }
        }

        protected virtual void Update()
        {
            if (SpawningEnabled == true)
            {
                ControlSpawner();
                ControlSpawnDelay();

                if (IsSpawnKeyPressed() == true)
                {
                    IncreaseGameManagerEnemyCounter(1);
                    SpawnEnemy();
                }

                if (IsDestroyEnemiesKeyPressed() == true)
                {
                    DestroySpawnedEnemies();
                }
            }
        }

        private bool IsSpawnKeyPressed()
        {
            return Input.GetKeyDown(SpawnEnemyKey) == true;
        }

        private bool IsDestroyEnemiesKeyPressed()
        {
            return Input.GetKeyDown(DestroyEnemiesKey) == true;
        }

        private bool IsSpawnDelayReached()
        {
            return TimeUntilSpawn < SpawnDelay == false;
        }

        private bool IsSpawnLimitReached()
        {
            return SpawningIndex < SpawningAmount == false;
        }

        private void ControlSpawner()
        {
            if (IsSpawnLimitReached() == false)
            {
                if (IsSpawnDelayReached() == true)
                {
                    SpawnEnemy();
                    ResetSpawnDelayTimer();
                }
                else
                {
                    UpdateSpawnDelayTimer();
                }
            }
        }

        private void ControlSpawnDelay()
        {
            if (CurrentTimeToMinSpawnDelay < TimeToReachMinSpawnDelay)
            {
                CurrentTimeToMinSpawnDelay += Time.deltaTime;
            }

            SetNewSpawnDelay();
        }

        private void SetNewSpawnDelay()
        {
            if (SpawnDelay > MinSpawnDelay)
            {
                SpawnDelay = Mathf.Lerp(MinSpawnDelay, MaxSpawnDelay, 1 - CurrentTimeToMinSpawnDelay / TimeToReachMinSpawnDelay);
            }
        }

        private void IncreaseGameManagerEnemyCounter(int amount)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncreaseEnemyCounter(amount);
            }
        }

        private void UpdateSpawnDelayTimer()
        {
            TimeUntilSpawn += Time.deltaTime;
        }

        private void ResetSpawnDelayTimer()
        {
            TimeUntilSpawn = 0f;
        }

        private void InitializeSpawnDelayValue()
        {
            SpawnDelay = MaxSpawnDelay;
        }

        private void SpawnEnemy()
        {
            int randomizedIndex = Random.Range(0, EnemyPrefabCollection.Count);

            Enemy spawnedEnemy = Instantiate(EnemyPrefabCollection[randomizedIndex], SpawnPoint.position, SpawnPoint.rotation, EntitiesContainer);
            spawnedEnemy.Initialize(EndPoint.position);

            SetEnemyDifficulty(spawnedEnemy);
            AddListenerToEnemy(spawnedEnemy);

            SpawnedEnemyCollection.Add(spawnedEnemy);
            SpawningIndex++;
        }

        private void SetEnemyDifficulty(Enemy spawnedEnemy)
        {
            float currentDifficultyMultiplier = GameSettings.GetDifficultyMultiplier();

            spawnedEnemy.HealthAmount = (int)(spawnedEnemy.HealthAmount * currentDifficultyMultiplier);
            spawnedEnemy.DamageGiven = (int)(spawnedEnemy.DamageGiven * currentDifficultyMultiplier);
            spawnedEnemy.CoinsDropped = (int)(spawnedEnemy.CoinsDropped + (spawnedEnemy.CoinsDropped * (1 - currentDifficultyMultiplier)));
        }

        private void SetTimeToReachMinSpawnDelayByDifficulty()
        {
            float currentDifficultyMultiplier = GameSettings.GetDifficultyMultiplier();

            TimeToReachMinSpawnDelay = TimeToReachMinSpawnDelay + (TimeToReachMinSpawnDelay * (1 - currentDifficultyMultiplier));
        }

        private void DestroySpawnedEnemies()
        {
            for (int index = 0; index < SpawnedEnemyCollection.Count; index++)
            {
                Destroy(SpawnedEnemyCollection[index].gameObject);
            }

            SpawnedEnemyCollection.Clear();
        }

        private void RemoveEnemyFromCollection(Enemy enemy)
        {
            RemoveListenerFromEnemy(enemy);

            int indexOfEnemy = ReturnIndexOfEnemy(enemy);

            if (indexOfEnemy >= 0)
            {
                SpawnedEnemyCollection.RemoveAt(indexOfEnemy);
            }
            else
            {
                Debug.Log("This enemy doesn't exist in collection");
            }
        }

        private int ReturnIndexOfEnemy(Enemy enemy)
        {
            for (int index = 0; index < SpawnedEnemyCollection.Count; index++)
            {
                if (enemy == SpawnedEnemyCollection[index])
                {
                    return index;
                }
            }
            return -1;
        }

        private void InitializeValues()
        {
            IncreaseGameManagerEnemyCounter(SpawningAmount);
            SetTimeToReachMinSpawnDelayByDifficulty();
            InitializeSpawnDelayValue();

            SpawningEnabled = true;
        }

        private void HandleGameEnd(int playerHealth)
        {
            DestroySpawnedEnemies();

            SpawningEnabled = false;
        }

        private void AddListenerToEnemy(Enemy enemy)
        {
            enemy.OnTargetReachedOrDead += RemoveEnemyFromCollection;
        }

        private void RemoveListenerFromEnemy(Enemy enemy)
        {
            enemy.OnTargetReachedOrDead -= RemoveEnemyFromCollection;
        }
    }
}