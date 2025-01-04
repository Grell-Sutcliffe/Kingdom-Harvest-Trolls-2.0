using Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LanguageChanger : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _languageDropdown;
    private List<LanguageData> _difficulties => LanguageManager.Instance.Difficulties;

    private void Start()
    {
        IEnumerable<string> languageNames =
            from language in _difficulties
            select language.Name;
        _languageDropdown.ClearOptions();
        _languageDropdown.AddOptions(languageNames.ToList());
        _languageDropdown.RefreshShownValue();

        _languageDropdown.value = SaveLoadSystem.Loadlanguage();
        languageManager.Instance.Setlanguage(_difficulties[_languageDropdown.value]);
    }

    private void Awake()
    {
        _languageDropdown.value = SaveLoadSystem.Loadlanguage();
    }

    public void Savelanguage()
    {
        languageManager.Instance.Setlanguage(_difficulties[_languageDropdown.value]);
        SaveLoadSystem.Savelanguage(_languageDropdown.value);
        Debug.Log($"{_languageDropdown.value}");
    }
}
