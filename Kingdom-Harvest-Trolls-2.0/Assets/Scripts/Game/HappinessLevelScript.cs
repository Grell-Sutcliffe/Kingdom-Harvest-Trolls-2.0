using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HappinessLevelScript : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] GameObject green_line;

    private float max_width;
    private float current_width;

    private float max_level;
    private float current_level;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        max_width = green_line.transform.localScale.x;
        current_width = max_width;

        max_level = gameController.repaired_cells;
        current_level = max_level;
    }

    public void ChangeMaxHappinessLevel(int amount)
    {
        max_level += amount;
        ChangeCurrentHappinessLevel(amount);
    }

    public void ChangeCurrentHappinessLevel(int amount)
    {
        current_level += amount;
        SetCurrentWidth();
    }

    private void SetCurrentWidth()
    {
        current_width = max_width * current_level / max_level;
        green_line.transform.localScale = new Vector3(current_width, green_line.transform.localScale.y, 0);
    }
}
