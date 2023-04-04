using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence.SceneManagement
{
    public class SceneController : SingletonMonoBehaviour<SceneController>
    {
        public event Action<string> OnSceneChange = delegate { };

        private AsyncOperation SceneLoader { get; set; }

        public void LoadMainMenu()
        {
            SceneLoader = SceneManager.LoadSceneAsync(SceneNames.MAIN_MENU, LoadSceneMode.Single);
            SceneLoader.completed += NotifyOnSceneChange;
        }

        public void LoadSceneSelector()
        {
            SceneLoader = SceneManager.LoadSceneAsync(SceneNames.SCENE_SELECTOR, LoadSceneMode.Single);
        }

        public void LoadMineEntranceMap()
        {
            SceneLoader = SceneManager.LoadSceneAsync(SceneNames.MINE_ENTRANCE_MAP, LoadSceneMode.Single);
            SceneLoader.completed += NotifyOnSceneChange;
        }

        public void LoadMineMap()
        {
            SceneLoader = SceneManager.LoadSceneAsync(SceneNames.MINE_MAP, LoadSceneMode.Single);
            SceneLoader.completed += NotifyOnSceneChange;
        }

        public void RestartLevel()
        {
            SceneLoader = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        protected virtual void Start()
        {
            DontDestroyOnLoad(gameObject);

            LoadMainMenu();
        }

        private void NotifyOnSceneChange(AsyncOperation operation)
        {
            OnSceneChange.Invoke(SceneManager.GetActiveScene().name);
        }
    }
}
