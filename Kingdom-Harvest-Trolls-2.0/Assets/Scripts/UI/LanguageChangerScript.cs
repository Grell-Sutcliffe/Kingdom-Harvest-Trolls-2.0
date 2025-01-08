using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageChangerScript : MonoBehaviour
{
    public int language_index;
    public GameObject waitingPanel;

    void Start()
    {
        language_index = PlayerPrefs.GetInt("language", language_index);
    }

    public void EnglishLanguage()
    {
        language_index = 0;
        PlayerPrefs.SetInt("language", language_index);
        waitingPanel.SetActive(true);
        Invoke("LoadMenu", 1.7f);
    }

    public void RussianLanguage()
    {
        language_index = 1;
        PlayerPrefs.SetInt("language", language_index);
        waitingPanel.SetActive(true);
        Invoke("LoadMenu", 1.8f);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
