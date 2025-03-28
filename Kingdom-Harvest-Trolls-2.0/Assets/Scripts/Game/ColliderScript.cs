using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColliderScript : MonoBehaviour
{
    public int index_i;
    public int index_j;
    private float castle_interval = 10f;

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
        //Debug.Log($"SHIT HAPPENS {collision.gameObject.name}");
        if ((collision.gameObject.tag != "Villager") && (collision.gameObject.tag != "Knight"))
        {
            if (collision.gameObject.tag == "Troll")
            {
                if (fieldScript.cells[index_i, index_j].destroyable)
                {
                    Cell cell = fieldScript.cells[index_i, index_j];
                    Cell new_cell;

                    if ((cell.type == "castle") && (cell.is_destroyed == true) && (fieldScript.checks[index_i, index_j].tag == "Village"))
                    {
                        new_cell = fieldScript.FindCellByType("castle", -1, 1, true, -1);

                        gameController.OpenDude("The castle is turning into riuns!");

                        new_cell.rotation = cell.rotation;
                        new_cell = gameController.EditRepairedCell(new_cell);

                        gameController.UpgrateCellInfo(index_i, index_j, new_cell);

                        fieldScript.ChangeCellTag(index_i, index_j, "Untagged");
                    }
                    else if ((cell.is_destroyed == false) && (fieldScript.checks[index_i, index_j].tag == "Village"))
                    {
                        DestroyCell();
                    }
                }
            }
        }
        else
        {
            if ((fieldScript.cells[index_i, index_j].type == "road") && (collision.gameObject.tag == "Villager"))
            {
                fieldScript.cells[index_i, index_j].coin_amount += fieldScript.cells[index_i, index_j].count_of_road;
                fieldScript.ready_to_claim[index_i, index_j] = true;
            }
        }
        gameController.UpdateClaimPanel();
    }

    private void DestroyCell()
    {
        gameController.ChangeCurrentHappinessLevel(-1);

        Cell cell = fieldScript.cells[index_i, index_j];
        Cell new_cell;

        if (cell.type == "wheat")
        {
            new_cell = fieldScript.FindCellByType(cell.type, 0, cell.count_of_road, true, -1);
        }
        else
        {
            new_cell = fieldScript.FindCellByType(cell.type, cell.level, cell.count_of_road, true, cell.index);
        }

        new_cell.rotation = cell.rotation;
        new_cell = gameController.EditRepairedCell(new_cell);

        gameController.UpgrateCellInfo(index_i, index_j, new_cell);

        if ((new_cell.type == "castle") && (new_cell.is_destroyed == true) && (new_cell.level > -1))
        {
            if (new_cell.level != -1)
            {
                //gameController.ChangeCurrentHappinessLevel(1 - new_cell.level);
                //Debug.Log(new_cell.level);
            }
            Invoke("CastleSecondChance", castle_interval);
        }

        fieldScript.ChangeCellTag(index_i, index_j, "Untagged");
    }

    private void CastleSecondChance()
    {
        fieldScript.ChangeCellTag(index_i, index_j, "Village");
    }
}
