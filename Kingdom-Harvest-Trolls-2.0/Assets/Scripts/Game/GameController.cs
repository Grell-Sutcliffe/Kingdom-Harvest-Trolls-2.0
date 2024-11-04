using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int coin_amount = 500;
    public int wheat_amount = 200;
    public int knight_amount = 1;
    public int villager_amount = 1;

    public int cost_of_new_cell = 200;

    public TextMeshProUGUI coin_amount_text;
    public TextMeshProUGUI wheat_amount_text;
    public TextMeshProUGUI knight_amount_text;
    public TextMeshProUGUI villager_amount_text;

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
    public GameObject RoadOkayPanel;
    public GameObject CastleOkayPanel;
    public GameObject NotOkayPanel;
    public GameObject UpgrateCastlePanel;
    public GameObject BuyKnightsPanel;
    public GameObject SurePanel;

    public GameObject WinPanel;
    public GameObject LosePanel;

    public GameObject Dude;
    public TextMeshProUGUI dudesText;

    public Image image;
    public Cell new_cell;

    private Sprite cell_befre_fit = null;
    private int last_ind_i = -1;
    private int last_ind_j = -1;
    private Vector3 last_rotation = new Vector3(0, 0, 0);

    private int x, y;
    private GameObject set_new_cell;
    private bool need_to_fit_cell = true;

    private string not_enougth = "My lord, you do not have enough of materials to do the action.";

    private string[] greetings = {
        "Greetings, my king! I'm your servant Sycophant and I will assist you! You, my king, must defend the kingdom and villagers from evil trolls and expand it!",
        "To the left of the screen, there is a panel displating the kingdom's resources. You can buy a new cell and rotate it by clicking on the bigger image of it for the best match on the map!",
        "Castles bring you gold, farms bring you wheat. If you click on a castle, willage or farm you can collect resources. Roads also bring you gold if those are used by villagers.",
        "With gold and wheat you can hire knights who will protect your kingdom from trolls and upgrate your castle. So, be wise, my king! Fill every cell on the map to win."
    };

    private int current_greeting = 0;

    private bool is_win = false;
    private bool is_lose = false;

    public int free_cells_amount;

    private void Start()
    {
        fieldScript = Field.GetComponent<FieldScript>();
        cellsScript = Field.GetComponent<CellsScript>();

        free_cells_amount = fieldScript.width * fieldScript.height - 5;

        OpenOptionPanel();

        CloseBuyKnightsPanel();
        CloseCastleOkayPanel();
        CloseCellPressedPanel();
        CloseNotOkayPanel();
        CloseOkayPanel();
        CloseRoadOkayPanel();
        CloseUpgrateCastlePanel();
        CloseSurePanel();
        CloseDude();

        WinPanel.gameObject.SetActive(false);
        LosePanel.gameObject.SetActive(false);

        ClosingOfBuyingANewCell(true);
        costOfCell.text = cost_of_new_cell.ToString();

        IncreaseCoinAmount(0);
        IncreaseWheatAmount(0);
        IncreaseKnightAmount(0);
        IncreaseVillagerAmount(0);

        InvokeRepeating("Timer", 1f, 1f);
    }

    private void Update()
    {
        if ((Dude.active == false) && (current_greeting < greetings.Length))
        {
            OpenDude(greetings[current_greeting]);
            current_greeting++;
        }

        if (sprite != null)
        {
            CellFitter();
        }
    }

    public void NewCellOnClick(GameObject cell)
    {
        sprite = cell.GetComponent<Image>().sprite;
        image.sprite = sprite;
        image.transform.localEulerAngles = new Vector3(0, 0, 0);
        
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
            CloseCellPressedPanel();
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
        set_new_cell = cell;

        if (fieldScript.villager_here[x, y] && sprite != null)
        {
            OpenDude("My king, you can't rebuild a cell with a villager on it");
        }
        else
        {
            if ((sprite != null) && (fieldScript.cells[index_i, index_j].title == null))
            {
                free_cells_amount--;
                SetNewCell();
            }
            else if ((sprite != null) && (fieldScript.cells[index_i, index_j].title != null))
            {
                OpenSurePanel();
                //SetNewCell();
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
    }

    public void SetNewCell()
    {
        last_rotation = new Vector3(0, 0, 0);

        if (x != last_ind_i || y != last_ind_j)
        {
            if (last_ind_i != -1 && last_ind_j != -1)
            {
                fieldScript.dark_cells[last_ind_i, last_ind_j].GetComponent<Image>().sprite = cell_befre_fit;
                fieldScript.dark_cells[last_ind_i, last_ind_j].GetComponent<Image>().transform.localEulerAngles = last_rotation;
            }
        }
        last_ind_i = -1;
        last_ind_j = -1;

        Debug.Log("CLICK");
        set_new_cell.GetComponent<Image>().sprite = sprite;
        set_new_cell.GetComponent<Image>().transform.localEulerAngles = new Vector3(0, 0, 90 * new_cell.rotation);

        EditCell();

        fieldScript.dark_cells[x, y] = set_new_cell;
        fieldScript.cells[x, y] = new_cell;
        if (fieldScript.cells[x, y].destroyable)
        {
            fieldScript.ChangeCellTag(x, y, "Village");
        }

        sprite = null;
        new_cell.rotation = 0;
        image.sprite = empty_sprite;
        image.transform.localEulerAngles = new Vector3(0, 0, 0);

        if (fieldScript.cells[x, y].type == "castle")
        {
            CreateVillager();
        }

        ClosingOfBuyingANewCell(true);

        cellsScript.RandomCell();

        if (free_cells_amount == 0)
        {
            ShowWinPanel();
        }

        CloseSurePanel();
    }

    public void CreateVillager()
    {
        Vector3 new_villager = fieldScript.checks[x, y].transform.position;
        Zoom.GetComponent<EnemySpawner>().VillagerSpawn(new_villager, x, y);

        IncreaseVillagerAmount(1);
    }

    public void EditCell()
    {
        int rotation = new_cell.rotation;

        while (rotation > 0)
        {
            string temp = new_cell.down;
            new_cell.down = new_cell.left;
            new_cell.left = new_cell.up;
            new_cell.up = new_cell.right;
            new_cell.right = temp;

            rotation--;
        }
    }

    public Cell EditRepairedCell(Cell repaired_cell)
    {
        int rotation = repaired_cell.rotation;

        while (rotation > 0)
        {
            string temp = repaired_cell.down;
            repaired_cell.down = repaired_cell.left;
            repaired_cell.left = repaired_cell.up;
            repaired_cell.up = repaired_cell.right;
            repaired_cell.right = temp;

            rotation--;
        }

        return repaired_cell;
    }

    public void SpriteOnClick()
    {
        new_cell.rotation = (new_cell.rotation + 1) % 4;
        image.transform.localEulerAngles = new Vector3(0, 0, 90 * new_cell.rotation);
    }

    public void IncreaseAmount()
    {
        if ((fieldScript.cells[x, y].coin_per_time > 0) || (fieldScript.cells[x, y].type == "road"))
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

    public void IncreaseVillagerAmount(int amount)
    {
        villager_amount += amount;

        villager_amount_text.text = villager_amount.ToString();
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
            else if (fieldScript.cells[index_i, index_j].type == "road")
            {
                cellPressedPanel.GetComponent<CellPressedPanelScript>().claim_button.gameObject.SetActive(true);
                OpenRoadOkayPanel();
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
        CloseRoadOkayPanel();
        CloseUpgrateCastlePanel();

        cellPressedPanel.gameObject.SetActive(false);
    }

    public void OpenOkayPanel()
    {
        CloseCastleOkayPanel();
        CloseRoadOkayPanel();
        CloseNotOkayPanel();

        OkayPanel.gameObject.SetActive(true);

        UpgrateTimer();
    }

    public void OpenRoadOkayPanel()
    {
        CloseCastleOkayPanel();
        CloseNotOkayPanel();
        CloseOkayPanel();

        RoadOkayPanel.SetActive(true);
    }

    public void CloseRoadOkayPanel()
    {
        RoadOkayPanel.SetActive(false);
    }

    public void CloseOkayPanel()
    {
        OkayPanel.gameObject.SetActive(false);
    }

    public void OpenCastleOkayPanel()
    {
        CloseNotOkayPanel();
        CloseRoadOkayPanel();
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
        CloseRoadOkayPanel();
        CloseOkayPanel();

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
        new_cell.rotation = cell.rotation;
        new_cell = EditRepairedCell(new_cell);

        if (coin_amount - new_cell.cost_of_upgrate < 0)
        {
            OpenDude(not_enougth);
        }
        else
        {
            IncreaseCoinAmount(-new_cell.cost_of_upgrate);
            UpgrateCellInfo(x, y, new_cell);

            fieldScript.ChangeCellTag(x, y, "Village");

            UpdateUpgratePanelInfo();

            CloseNotOkayPanel();
            OpenCellPressedPanel(x, y);
        }
    }

    public void UpgrateCastle()
    {
        int next_lvl = fieldScript.cells[x, y].level + 1;
        Cell new_castle = fieldScript.FindCellByType("castle", next_lvl, 1, false);
        new_castle.rotation = fieldScript.cells[x, y].rotation;
        new_castle = EditRepairedCell(new_castle);
        new_castle.coin_amount = fieldScript.cells[x, y].coin_amount;

        if (coin_amount - new_castle.cost_of_upgrate < 0)
        {
            OpenDude(not_enougth);
        }
        else
        {
            int new_villagers_amount = new_castle.villager_amount - fieldScript.cells[x, y].villager_amount;

            IncreaseCoinAmount(-new_castle.cost_of_upgrate);
            UpgrateCellInfo(x, y, new_castle);

            UpdateUpgratePanelInfo();

            CreateNewVillagers(new_villagers_amount);
        }
    }

    private void CreateNewVillagers(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CreateVillager();
        }
    }

    public void UpdateUpgratePanelInfo()
    {
        UpgrateCastlePanelScript script = UpgrateCastlePanel.GetComponent<UpgrateCastlePanelScript>();

        int castle_current_lvl = fieldScript.cells[x, y].level + 1;
        int current_coin_per_time = fieldScript.cells[x, y].coin_per_time;
        int current_villager_amount = fieldScript.cells[x, y].villager_amount;
        Cell new_castle = fieldScript.FindCellByType("castle", castle_current_lvl, 1, false);
        int cost = new_castle.cost_of_upgrate;

        if (fieldScript.cells[x, y].level < 2)
        {
            script.level_upgrate.text = castle_current_lvl.ToString() + " -> " + (castle_current_lvl + 1).ToString();

            int next_coin_per_time = new_castle.coin_per_time;
            script.coin_upgrate.text = current_coin_per_time.ToString() + " -> " + next_coin_per_time.ToString();

            int villager_amount = new_castle.villager_amount;
            script.villagers_upgrate.text = current_villager_amount.ToString() + " -> " + villager_amount.ToString();

            script.cost.text = cost.ToString();

            script.upgrate_button.gameObject.SetActive(true);
        }
        else
        {
            script.level_upgrate.text = castle_current_lvl.ToString();
            script.coin_upgrate.text = current_coin_per_time.ToString();
            script.villagers_upgrate.text = current_villager_amount.ToString();

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

    public void OpenSurePanel()
    {
        need_to_fit_cell = false;
        SurePanel.SetActive(true);
    }

    public void CloseSurePanel()
    {
        need_to_fit_cell = true;
        SurePanel.SetActive(false);
    }

    private void Timer()
    {
        fieldScript.IncreaseTimer();
        UpdateClaimPanel();
    }

    private void CellFitter()
    {
        Vector2 coords = Zoom.transform.position;
        Vector3 mouse_position = Input.mousePosition;
        float zoom = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MouseUIController>().zoom;
        float mouse_x = mouse_position.x;
        float mouse_y = mouse_position.y;
        double j = (fieldScript.width * zoom) / 2 + (fieldScript.width % 2 == 0 ? 0 : 0.5) - ((coords.x - mouse_x) / (fieldScript.cellSize.x * zoom));
        double i = (fieldScript.height * zoom) / 2 + (fieldScript.height % 2 == 0 ? 0 : 0.5) - ((mouse_y - coords.y) / (fieldScript.cellSize.y * zoom));
        float delta_j = (fieldScript.width / 2) * (zoom - 1);
        float delta_i = (fieldScript.height / 2) * (zoom - 1);
        //i /= zoom;
        //j /= zoom;
        i -= delta_i;
        j -= delta_j;
        //float x = (float)(coords.x - (width / 2 + (width % 2 == 0 ? 0 : 0.5) - j + 0.5) * cellSize.x);
        //float y = (float)(coords.y + (height / 2 + (height % 2 == 0 ? 0 : 0.5) - i + 0.5) * cellSize.y);
        int ind_i = (int)i;
        int ind_j = (int)j;

        if ((ind_i >= 0 && ind_i < fieldScript.height) && (ind_j >= 0 && ind_j < fieldScript.width) && need_to_fit_cell)
        {
            if (ind_i != last_ind_i || ind_j != last_ind_j)
            {
                if (last_ind_i != -1 && last_ind_j != -1)
                {
                    fieldScript.dark_cells[last_ind_i, last_ind_j].GetComponent<Image>().sprite = cell_befre_fit;
                    fieldScript.dark_cells[last_ind_i, last_ind_j].GetComponent<Image>().transform.localEulerAngles = last_rotation;
                }

                cell_befre_fit = fieldScript.dark_cells[ind_i, ind_j].GetComponent<Image>().sprite;
                last_rotation = fieldScript.dark_cells[ind_i, ind_j].GetComponent<Image>().transform.localEulerAngles;
                last_ind_i = ind_i;
                last_ind_j = ind_j;
            }
            fieldScript.dark_cells[ind_i, ind_j].GetComponent<Image>().sprite = image.sprite;
            fieldScript.dark_cells[ind_i, ind_j].GetComponent<Image>().transform.localEulerAngles = new Vector3(0, 0, 90 * new_cell.rotation);
        }
        else
        {
            if (need_to_fit_cell)
            {
                if (last_ind_i != -1 && last_ind_j != -1)
                {
                    fieldScript.dark_cells[last_ind_i, last_ind_j].GetComponent<Image>().sprite = cell_befre_fit;
                    fieldScript.dark_cells[last_ind_i, last_ind_j].GetComponent<Image>().transform.localEulerAngles = last_rotation;
                }

                last_ind_i = -1;
                last_ind_j = -1;
            }
        }
    }
}
