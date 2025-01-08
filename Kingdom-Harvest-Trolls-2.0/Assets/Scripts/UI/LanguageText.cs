using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour
{
    public int language_index;
    public string[] text;
    private TextMeshProUGUI textLine;

    void Start()
    {
        language_index = PlayerPrefs.GetInt("language", language_index);
        textLine = GetComponent<TextMeshProUGUI>();
        textLine.text = "" + text[language_index];
    }
}
