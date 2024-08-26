using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI
{
    public class LevelChooser : MonoBehaviour
    {
        [SerializeField] private string choosenLevel;
        public void LoadChosenLevel()
        {
            SceneManager.LoadScene(choosenLevel);
        }
    }
}
