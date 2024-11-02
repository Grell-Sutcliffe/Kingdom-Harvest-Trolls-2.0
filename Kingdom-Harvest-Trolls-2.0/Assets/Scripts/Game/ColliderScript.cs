using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColliderScript : MonoBehaviour
{
    public int index_i;
    public int index_j;

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
        if (collision.gameObject.tag != "Villager")
        {
            if ((fieldScript.cells[index_i, index_j].destroyable) && (fieldScript.cells[index_i, index_j].is_destroyed == false))
            {
                if (collision.gameObject.tag == "Troll")
                {
                    DestroyCell();
                }
            }
        }
        else
        {
            if (fieldScript.cells[index_i, index_j].type == "road")
            {
                fieldScript.cells[index_i, index_j].coin_amount += fieldScript.cells[index_i, index_j].count_of_road;
            }
        }
        gameController.UpdateClaimPanel();
    }

    private void DestroyCell()
    {
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

        new_cell.rotation = cell.rotation;
        new_cell = gameController.EditRepairedCell(new_cell);

        gameController.UpgrateCellInfo(index_i, index_j, new_cell);

        fieldScript.ChangeCellTag(index_i, index_j, "Untagged");

        if ((new_cell.type == "castle") && (new_cell.is_destroyed == true) && (new_cell.level > -1))
        {
            Invoke("CastleSecondChance", 3f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Villager")
        {
            if (fieldScript.cells[index_i, index_j].destroyable)
            {
                if (collision.gameObject.tag == "Troll")
                {
                    Cell cell = fieldScript.cells[index_i, index_j];
                    Cell new_cell;

                    if ((cell.type == "castle") && (cell.is_destroyed == true) && (fieldScript.checks[index_i, index_j].tag == "Knight"))
                    {
                        new_cell = fieldScript.FindCellByType("castle", -1, 1, true);

                        gameController.OpenDude("The castle is turning into riuns!");

                        new_cell.rotation = cell.rotation;
                        new_cell = gameController.EditRepairedCell(new_cell);

                        gameController.UpgrateCellInfo(index_i, index_j, new_cell);

                        fieldScript.ChangeCellTag(index_i, index_j, "Untagged");
                    }
                    else if ((cell.is_destroyed == false) && (fieldScript.checks[index_i, index_j].tag == "Knight"))
                    {
                        DestroyCell();
                    }
                }
            }
        }

        gameController.UpdateClaimPanel();
    }

    private void CastleSecondChance()
    {
        fieldScript.ChangeCellTag(index_i, index_j, "Knight");
    }
}
