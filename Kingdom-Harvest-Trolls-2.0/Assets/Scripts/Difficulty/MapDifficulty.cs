using UnityEngine;

namespace Game
{
    public class PlayerDifficultySetter : MonoBehaviour
    {
        private FieldScript fieldScript;

        private void Awake()
        {
            fieldScript = GetComponent<FieldScript>();
        }

        private void Start()
        {
            fieldScript.SetWidth(DifficultyManager.Instance.Difficulty.WidthMultiplier);
            fieldScript.SetHeight(DifficultyManager.Instance.Difficulty.HeightMultiplier);

        }
    }
}
