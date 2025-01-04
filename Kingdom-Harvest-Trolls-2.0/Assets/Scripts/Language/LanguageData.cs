using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "NewLanguageData", menuName = "Language/LanguageData")]
    public class LanguageData : ScriptableObject
    {
        public string Name;
        public int WidthMultiplier;
        public int HeightMultiplier;
    }
}