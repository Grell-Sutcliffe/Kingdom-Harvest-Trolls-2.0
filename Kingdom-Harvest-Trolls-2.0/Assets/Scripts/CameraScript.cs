using UnityEngine;

namespace Game
{
    public class MapDrag : MonoBehaviour
    {
        public Camera mainCamera;  // Камера, через которую смотрим на карту
        private Vector3 dragOrigin;  // Точка начала перетаскивания
        private bool isDragging = false;  // Флаг перетаскивания

        void Update()
        {
            // Начало перетаскивания при нажатии левой кнопки мыши
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;
            }

            // Завершение перетаскивания при отпускании левой кнопки мыши
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            // Перетаскивание карты
            if (isDragging)
            {
                Vector3 currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 difference = dragOrigin - currentMousePos;

                mainCamera.transform.position += difference;
            }
        }
    }
}


