using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameIsPause = false;
        public GameObject PauseMenuUI;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPause)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            Debug.Log("Continue clicked");
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPause = false;
        }

        public void Pause()
        {
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPause = true;
        }

        public void LoadMenu()
        {
            Debug.Log("loaded menu");
            Time.timeScale = 1f;
            PlayerPrefs.Save();
            SceneManager.LoadScene("MainMenu");
        }
    }
}


