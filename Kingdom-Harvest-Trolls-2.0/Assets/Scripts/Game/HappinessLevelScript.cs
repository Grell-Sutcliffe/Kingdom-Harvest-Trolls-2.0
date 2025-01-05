using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HappinessLevelScript : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] GameObject green_line;

    private float max_width;
    private float current_width;

    private float max_level;
    private float current_level;

    public string color;
    public string green = "38C234";
    public string yellow = "E9E41F";
    public string red = "C81B1B";

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        max_width = green_line.transform.localScale.x;
        current_width = max_width;

        current_level = gameController.repaired_cells;
        max_level = current_level + 2; // castle + 2

        SetCurrentWidth();
    }

    public void ChangeMaxHappinessLevel(int amount, int current_amount)
    {
        max_level += amount;
        ChangeCurrentHappinessLevel(current_amount);
    }

    public void ChangeCurrentHappinessLevel(int amount)
    {
        current_level += amount;
        SetCurrentWidth();
    }

    private void SetCurrentWidth()
    {
        current_width = max_width * current_level / max_level;
        if (current_width < 0)
        {
            current_width = 0;
        }

        if (current_level / max_level > 2 / 3)
        {
            green_line.GetComponent<Image>().color = Color.green;
        }
        else if (current_level / max_level > 1 / 3)
        {
            green_line.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            green_line.GetComponent<Image>().color = Color.red;
        }

        green_line.transform.localScale = new Vector3(current_width, green_line.transform.localScale.y, 0);
        Debug.Log($"Happiness level: max = {max_level}, cur = {current_level}");
    }
}
