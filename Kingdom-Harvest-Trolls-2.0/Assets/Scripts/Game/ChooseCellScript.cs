using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Game
{
    public class ChooseCellScript : MonoBehaviour, IPointerClickHandler
    {
        GameController gameController;

        private void Start()
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            gameController.NewCellOnClick(transform.gameObject);
        }
    }
}

