using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColliderScript : MonoBehaviour
{
    public int index_i;
    public int index_j;

    private int is_broking = 0;

    GameController gameController;
    FieldScript fieldScript;
    CellsScript cellsScript;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        fieldScript = GameObject.Find("FieldPanel").GetComponent<FieldScript>();
        cellsScript = GameObject.Find("FieldPanel").GetComponent<CellsScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if ((fieldScript.cells[index_i, index_j].destroyable) && (!fieldScript.cells[index_i, index_j].is_destroyed))
        {
            if (collision.gameObject.tag == "Troll")
            {
                is_broking++;

                Cell cell = fieldScript.cells[index_i, index_j];
                Cell new_cell;

                if (cell.type == "wheat")
                {
                    new_cell = fieldScript.FindCellByType(cell.type, 0, cell.count_of_road, true);
                }
                else
                {
                    new_cell = fieldScript.FindCellByType(cell.type, cell.level, cell.count_of_road, true);
                }

                gameController.UpgrateCellInfo(index_i, index_j, new_cell);

                fieldScript.ChangeCellTag(index_i, index_j, "Untagged");

                if ((cell.type == "castle") && (is_broking > 1))
                {
                    BreakCastle();
                }
                else
                {
                    is_broking = 0;
                }
            }
        }
    }

    private void BreakCastle()
    {
        Cell new_cell = fieldScript.FindCellByType("castle", -1, 1, true);

        gameController.UpgrateCellInfo(index_i, index_j, new_cell);

        gameController.OpenDude("The castle is turning into riuns!");
    }
}
