using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseUIController : MonoBehaviour
{
    private Canvas canvas;

    public float cameraSpeed;

    public float minZoom;
    public float maxZoom;

    public float zoom = 1f;

    public GameObject Panel;
    public GameObject ColliderPanel;

    private Vector3 startPosition;
    private Vector3 cursorPosition;
    //private Vector3 lastPanelPosition;
    private Vector3 canvasCenter;

    private void Start()
    {
        startPosition = Panel.transform.position;

        //lastPanelPosition = canvasCenter;

        Debug.Log($"{canvasCenter.x} {canvasCenter.y}");
    }

    private void Update()
    {
        Zoom();
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        //GetComponent<CanvasScaler>().scaleFactor = zoom;
        Panel.GetComponent<RectTransform>().localScale = new Vector3(zoom, zoom, 1);
        //ColliderPanel.GetComponent<RectTransform>().localScale = new Vector3(zoom, zoom, 1);

        startPosition = Input.mousePosition;

        RestrictPanelMovement();
        //lastPanelPosition = Panel.transform.position;
    }

    void Zoom()
    {
        if (Input.GetKeyUp(KeyCode.KeypadMinus))
        {
            zoom -= cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            zoom += cameraSpeed * Time.deltaTime;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoom -= cameraSpeed * Time.deltaTime * 10f;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            zoom += cameraSpeed * Time.deltaTime * 10f;
        }
    }

    public void Move()
    {
        Panel.transform.position = new Vector3(
            Input.mousePosition.x - startPosition.x + Panel.transform.position.x,
            Input.mousePosition.y - startPosition.y + Panel.transform.position.y,
            0
        );
        //ColliderPanel.transform.position = Panel.transform.position;
        startPosition = Input.mousePosition;
    }

    private void RestrictPanelMovement()
    {
        canvas = gameObject.GetComponent<Canvas>();

        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        canvasCenter = new Vector3(canvasRectTransform.rect.width / 2, canvasRectTransform.rect.height / 2, 0);
        // Получаем RectTransform панели и Canvas

        RectTransform panelRectTransform = Panel.GetComponent<RectTransform>();

        // Получаем текущую позицию Panel
        Vector3 panelPosition = Panel.transform.position;

        // Проверяем, выходит ли центр Canvas за пределы Panel
        if (Mathf.Abs(canvasCenter.x - panelPosition.x) > panelRectTransform.rect.width * zoom / 2)
        {
            float new_x = Mathf.Abs(canvasCenter.x - panelPosition.x) - panelRectTransform.rect.width * zoom / 2;
            new_x *= (canvasCenter.x - panelPosition.x > 0) ? (1) : (-1);
            Panel.transform.position = new Vector3(
                Panel.transform.position.x + new_x,
                Panel.transform.position.y,
                0
            );
            //ColliderPanel.transform.position = Panel.transform.position;
        }
        if (Mathf.Abs(canvasCenter.y - panelPosition.y) > panelRectTransform.rect.height * zoom / 2)
        {
            float new_y = Mathf.Abs(canvasCenter.y - panelPosition.y) - panelRectTransform.rect.height * zoom / 2;
            new_y *= (canvasCenter.y - panelPosition.y > 0) ? (1) : (-1);
            Panel.transform.position = new Vector3(
                Panel.transform.position.x,
                Panel.transform.position.y + new_y,
                0
            );
            //ColliderPanel.transform.position = Panel.transform.position;
        }
    }
}
