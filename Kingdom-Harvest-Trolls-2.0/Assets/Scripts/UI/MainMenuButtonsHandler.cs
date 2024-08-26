using UnityEngine;

namespace Game.UI
{
    public class MainMenuButtonsHandler : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private GameObject _optionsMenu;

        public void ShowOptions() => _optionsMenu.SetActive(true);
        public void Quit() => Application.Quit();
    }
}
