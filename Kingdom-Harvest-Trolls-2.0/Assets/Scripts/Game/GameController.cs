using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int coin_amount;
    public int wheat_amount;
    public int knight_amount;

    public int cost_of_new_cell = 200;

    public TextMeshProUGUI coin_amount_text;
    public TextMeshProUGUI wheat_amount_text;
    public TextMeshProUGUI knight_amount_text;

    [SerializeField] GameObject colliderPanel;

    FieldScript fieldScript;
    CellsScript cellsScript;
    public GameObject Field;
    public GameObject Zoom;

    Sprite sprite = null;
    public Sprite empty_sprite;

    public GameObject optionPanel;
    public GameObject cellPressedPanel;

    public Button buyNewCell;
    public TextMeshProUGUI costOfCell;
    public GameObject costOfCellPanel;

    public GameObject OkayPanel;
    public GameObject CastleOkayPanel;
    public GameObject NotOkayPanel;
    public GameObject UpgrateCastlePanel;
    public GameObject BuyKnightsPanel;

    public GameObject WinPanel;
    public GameObject LosePanel;

    public GameObject Dude;
    public TextMeshProUGUI dudesText;

    public Image image;
    public Cell new_cell;

    private int x, y;

    private string not_enougth = "My lord, you do not have enougth of materials to do the action.";

    private string greeting1 = "Greetings, my king! I'm your Sycophant and I will assist you! You, my king, must defend the kingdom from trolls and expand it!";
    private string greeting2 = "To the right of the map, there is a panel displating the kingdom's resources. You can choose multipple cards for your kingdom and rotate it for the best match..!.";
    private string greeting3 = "Castles bring you gold and with this gold and wheat you can hire knight. So, be wise, my king! Fill every cell on the map to win.";

    private string[] greetings = new string[3];
    private int current_greeting = 0;

    private bool is_win = false;
    private bool is_lose = false;

    public int free_cells_amount;

    private void Start()
    {
        fieldScript = Field.GetComponent<FieldScript>();
        cellsScript = Field.GetComponent<CellsScript>();

        free_cells_amount = fieldScript.width * fieldScript.height - 4;

        greetings[0] = greeting1;
        greetings[1] = greeting2;
        greetings[2] = greeting3;

        CloseBuyKnightsPanel();
        CloseCastleOkayPanel();
        CloseCellPressedPanel();
        CloseNotOkayPanel();
        CloseOkayPanel();
        CloseOptionPanel();
        CloseUpgrateCastlePanel();
        CloseDude();

        WinPanel.gameObject.SetActive(false);
        LosePanel.gameObject.SetActive(false);

        ClosingOfBuyingANewCell(true);
        costOfCell.text = cost_of_new_cell.ToString();

        IncreaseCoinAmount(0);
        IncreaseWheatAmount(0);
        IncreaseKnightAmount(0);

        InvokeRepeating("Timer", 1f, 1f);
    }

    private void Update()
    {
        if ((Dude.active == false) && (current_greeting < greetings.Length))
        {
            OpenDude(greetings[current_greeting]);
            current_greeting++;
        }
    }

    public void NewCellOnClick(GameObject cell)
    {
        sprite = cell.GetComponent<Image>().sprite;
        image.sprite = sprite;

        for (int i = 0; i < cellsScript.all_cells.Length; i++)
        {
            if (cellsScript.all_cells[i].sprite == sprite)
            {
                new_cell = cellsScript.all_cells[i];

                break;
            }
        }
    }

    public void BuyANewCell()
    {
        if (coin_amount - cost_of_new_cell < 0)
        {
            OpenDude(not_enougth);
        }
        else
        {
            IncreaseCoinAmount(-cost_of_new_cell);

            ClosingOfBuyingANewCell(false);
        }
    }

    public void ClosingOfBuyingANewCell(bool IF)
    {
        buyNewCell.gameObject.SetActive(IF);
        costOfCellPanel.gameObject.SetActive(IF);
    }

    public void FieldCellOnClick(GameObject cell, int index_i, int index_j)
    {
        x = index_i;
        y = index_j;
        if ((sprite != null) && (fieldScript.cells[index_i, index_j].title == null))
        {
            free_cells_amount--;

            Debug.Log("CLICK");
            cell.GetComponent<Image>().sprite = sprite;
            cell.GetComponent<Image>().transform.localEulerAngles = new Vector3(0, 0, 90 * new_cell.rotation);

            EditCell();

            fieldScript.dark_cells[index_i, index_j] = cell;
            fieldScript.cells[index_i, index_j] = new_cell;
            if (fieldScript.cells[index_i, index_j].destroyable)
            {
                fieldScript.ChangeCellTag(index_i, index_j, "Knight");
            }

            sprite = null;
            new_cell.rotation = 0;
            image.sprite = empty_sprite;
            image.transform.localEulerAngles = new Vector3(0, 0, 0);

            ClosingOfBuyingANewCell(true);

            cellsScript.RandomCell();

            if (free_cells_amount == 0)
            {
                ShowWinPanel();
            }
        }
        else if (sprite == null)
        {
            fieldScript.OnCellClick(index_i, index_j);
            if (fieldScript.cells[index_i, index_j].destroyable)
            {
                OpenCellPressedPanel(index_i, index_j);
                UpdateClaimPanel();
            }
            else
            {
                CloseCellPressedPanel();
            }
        }
    }

    public void EditCell()
    {
        while (new_cell.rotation > 0)
        {
            string temp = new_cell.down;
            new_cell.down = new_cell.left;
            new_cell.left = new_cell.up;
            new_cell.up = new_cell.right;
            new_cell.right = temp;

            new_cell.rotation--;
        }
    }

    public void SpriteOnClick()
    {
        new_cell.rotation = (new_cell.rotation + 1) % 4;
        image.transform.localEulerAngles = new Vector3(0, 0, 90 * new_cell.rotation);
    }

    public void IncreaseAmount()
    {
        if (fieldScript.cells[x, y].coin_per_time > 0)
        {
            IncreaseCoinAmount(fieldScript.cells[x, y].coin_amount);
            fieldScript.cells[x, y].coin_amount = 0;
        }
        if (fieldScript.cells[x, y].wheat_per_time > 0)
        {
            IncreaseWheatAmount(fieldScript.cells[x, y].wheat_amount);

            if (fieldScript.cells[x, y].wheat_amount > 0)
            {
                Cell new_wheat = fieldScript.FindCellByType("wheat", 0, 0, false);
                UpgrateCellInfo(x, y, new_wheat);
                fieldScript.cells[x, y].wheat_amount = 0;
                fieldScript.cells[x, y].time_for_peak = 60;
            }
        }
        //fieldScript.cells[x, y].time_for_peak = 60;

        UpdateClaimPanel();
    }

    public void UpdateClaimPanel()
    {
        cellPressedPanel.gameObject.GetComponent<CellPressedPanelScript>().ChangeTitle(fieldScript.cells[x, y]);

        Cell cell = fieldScript.cells[x, y];

        CastleOkayPanel.GetComponent<TimerScript>().timer_text.text = cell.time_for_peak.ToString();
        OkayPanel.GetComponent<TimerScript>().timer_text.text = cell.time_for_peak.ToString();
    }

    public void IncreaseCoinAmount(int amount)
    {
        coin_amount += amount;

        if (coin_amount < 0)
        {
            coin_amount = 0;
            OpenDude(not_enougth);
        }
        else
        {
            coin_amount_text.text = coin_amount.ToString();
        }
    }

    public void IncreaseWheatAmount(int amount)
    {
        wheat_amount += amount;
        wheat_amount_text.text = wheat_amount.ToString();
    }

    public void IncreaseKnightAmount(int amount)
    {
        knight_amount += amount;
        knight_amount_text.text = knight_amount.ToString();
    }

    public void OpenCellPressedPanel(int index_i, int index_j)
    {
        cellPressedPanel.gameObject.SetActive(true);

        if (fieldScript.cells[index_i, index_j].is_destroyed == true)
        {
            cellPressedPanel.GetComponent<CellPressedPanelScript>().claim_button.gameObject.SetActive(true);
            cellPressedPanel.GetComponent<CellPressedPanelScript>().ShowImage(false);
            OpenNotOkayPanel();
        }
        else
        {
            cellPressedPanel.GetComponent<CellPressedPanelScript>().ShowImage(true);
            if (fieldScript.cells[index_i, index_j].type == "castle")
            {
                cellPressedPanel.GetComponent<CellPressedPanelScript>().claim_button.gameObject.SetActive(false);
                OpenCastleOkayPanel();
            }
            else
            {
                cellPressedPanel.GetComponent<CellPressedPanelScript>().claim_button.gameObject.SetActive(true);
                OpenOkayPanel();
            }
        }
    }

    public void BuyKnights()
    {
        BuyKnightsPanelScript knights = BuyKnightsPanel.GetComponent<BuyKnightsPanelScript>();

        if ((coin_amount - knights.cost_coins < 0) || (wheat_amount - knights.cost_wheat < 0))
        {
            OpenDude(not_enougth);
        }
        else
        {
            IncreaseCoinAmount(-knights.cost_coins);
            IncreaseWheatAmount(-knights.cost_wheat);

            IncreaseKnightAmount(knights.knights_amount);

            for (int i = 0; i < knights.knights_amount; i++)
            {
                Zoom.GetComponent<EnemySpawner>().KnightSpawn();
            }
        }
    }

    public void OpenOptionPanel()
    {
        optionPanel.gameObject.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        optionPanel.gameObject.SetActive(false);
    }

    public void CloseCellPressedPanel()
    {
        CloseBuyKnightsPanel();
        CloseCastleOkayPanel();
        CloseNotOkayPanel();
        CloseOkayPanel();
        CloseUpgrateCastlePanel();

        cellPressedPanel.gameObject.SetActive(false);
    }

    public void OpenOkayPanel()
    {
        CloseCastleOkayPanel();
        CloseNotOkayPanel();

        OkayPanel.gameObject.SetActive(true);

        UpgrateTimer();
    }

    public void CloseOkayPanel()
    {
        OkayPanel.gameObject.SetActive(false);
    }

    public void OpenCastleOkayPanel()
    {
        CloseNotOkayPanel();
        CloseOkayPanel();

        CastleOkayPanel.gameObject.SetActive(true);

        UpgrateTimer();
    }

    public void UpgrateTimer()
    {
        Cell cell = fieldScript.cells[x, y];

        CastleOkayPanel.GetComponent<TimerScript>().timer_text.text = cell.time_for_peak.ToString();
    }

    public void CloseCastleOkayPanel()
    {
        CastleOkayPanel.gameObject.SetActive(false);
    }

    public void OpenNotOkayPanel()
    {
        CloseCastleOkayPanel();

        NotOkayPanel.gameObject.SetActive(true);

        Cell cell = fieldScript.cells[x, y];

        NotOkayPanel.GetComponent<RepairScript>().repair_cost_text.text = cell.cost_of_upgrate.ToString();
    }

    public void CloseNotOkayPanel()
    {
        NotOkayPanel.gameObject.SetActive(false);
    }

    public void OpenBuyKnightsPanel()
    {
        BuyKnightsPanel.gameObject.SetActive(true);
    }

    public void CloseBuyKnightsPanel()
    {
        BuyKnightsPanel.gameObject.SetActive(false);
    }

    public void OpenUpgrateCastlePanel()
    {
        UpgrateCastlePanel.gameObject.SetActive(true);

        UpdateUpgratePanelInfo();
    }

    public void RepairCell()
    {
        Cell cell = fieldScript.cells[x, y];
        Cell new_cell = fieldScript.FindCellByType(cell.type, cell.level + (cell.level == -1 ? 1 : 0), cell.count_of_road, false);

        if (coin_amount - new_cell.cost_of_upgrate < 0)
        {
            OpenDude(not_enougth);
        }
        else
        {
            IncreaseCoinAmount(-new_cell.cost_of_upgrate);
            UpgrateCellInfo(x, y, new_cell);

            fieldScript.ChangeCellTag(x, y, "Knight");

            UpdateUpgratePanelInfo();

            CloseNotOkayPanel();
            OpenCellPressedPanel(x, y);
        }
    }

    public void UpgrateCastle()
    {
        int next_lvl = fieldScript.cells[x, y].level + 1;
        Cell new_castle = fieldScript.FindCellByType("castle", next_lvl, 1, false);

        if (coin_amount - new_castle.cost_of_upgrate < 0)
        {
            OpenDude(not_enougth);
        }
        else
        {
            IncreaseCoinAmount(-new_castle.cost_of_upgrate);
            UpgrateCellInfo(x, y, new_castle);

            UpdateUpgratePanelInfo();
        }
    }

    public void UpdateUpgratePanelInfo()
    {
        UpgrateCastlePanelScript script = UpgrateCastlePanel.GetComponent<UpgrateCastlePanelScript>();

        int castle_current_lvl = fieldScript.cells[x, y].level + 1;
        int current_coin_per_time = fieldScript.cells[x, y].coin_per_time;
        int current_knight_amount = fieldScript.cells[x, y].knight_amount;
        Cell new_castle = fieldScript.FindCellByType("castle", castle_current_lvl, 1, false);
        int cost = new_castle.cost_of_upgrate;

        if (fieldScript.cells[x, y].level < 2)
        {
            script.level_upgrate.text = castle_current_lvl.ToString() + " -> " + (castle_current_lvl + 1).ToString();

            int next_coin_per_time = new_castle.coin_per_time;
            script.coin_upgrate.text = current_coin_per_time.ToString() + " -> " + next_coin_per_time.ToString();

            int knight_amount = new_castle.knight_amount;
            script.knight_upgrate.text = current_knight_amount.ToString() + " -> " + knight_amount.ToString();

            script.cost.text = cost.ToString();

            script.upgrate_button.gameObject.SetActive(true);
        }
        else
        {
            script.level_upgrate.text = castle_current_lvl.ToString();
            script.coin_upgrate.text = current_coin_per_time.ToString();
            script.knight_upgrate.text = current_knight_amount.ToString();

            script.cost.text = cost.ToString();

            script.upgrate_button.gameObject.SetActive(false);
        }
    }

    public void CloseUpgrateCastlePanel()
    {
        UpgrateCastlePanel.gameObject.SetActive(false);
    }

    public void UpgrateCellInfo(int index_i, int index_j, Cell new_cell)
    {
        fieldScript.cells[index_i, index_j] = new_cell;
        fieldScript.dark_cells[index_i, index_j].GetComponent<Image>().sprite = new_cell.sprite;
    }

    public void OpenDude(string text)
    {
        dudesText.text = text;
        Dude.gameObject.SetActive(true);
    }

    public void CloseDude()
    {
        Dude.gameObject.SetActive(false);
        dudesText.text = "";
    }

    public void ShowWinPanel()
    {
        WinPanel.gameObject.SetActive(true);
    }

    public void ShowLosePanel()
    {
        LosePanel.gameObject.SetActive(true);
    }

    private void Timer()
    {
        fieldScript.IncreaseTimer();
        UpdateClaimPanel();
    }
}
