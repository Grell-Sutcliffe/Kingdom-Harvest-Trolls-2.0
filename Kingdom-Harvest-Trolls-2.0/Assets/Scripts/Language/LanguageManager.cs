using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }
    [SerializeField] private List<LanguageData> _difficulties;
    public List<LanguageData> Difficulties
    {
        get { return _difficulties; }
    }
    public LanguageData Language { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //SetLanguage(_difficulties[SaveLoadSystem.LoadLanguage()]);
    }

    public void SetLanguage(LanguageData Language)
    {
        Language = Language;
    }

    public void SaveLanguage()
    {
        //SaveLoadSystem.SaveLanguage(_difficulties.IndexOf(Language));
    }
}
