using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Game
{
    public class AddCellScript : MonoBehaviour, IPointerClickHandler
    {
        public int index_i;
        public int index_j;

        GameController gameController;

        private void Start()
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            gameController.FieldCellOnClick(transform.gameObject, index_i, index_j);
        }
    }
}

