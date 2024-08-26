using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "NewDifficultyData", menuName = "Difficulty/DifficultyData")]
    public class DifficultyData : ScriptableObject
    {
        public string Name;
        public int WidthMultiplier;
        public int HeightMultiplier;
    }
}