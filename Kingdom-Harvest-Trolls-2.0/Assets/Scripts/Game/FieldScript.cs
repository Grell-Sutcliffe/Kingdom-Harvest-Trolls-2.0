using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using Game;

public class FieldScript : MonoBehaviour
{
    public GameObject GameController;
    private GameController gameController;

    private CellsScript cellsScript;

    public GameObject checkPanel;
    public GameObject collider;

    public GameObject[,] checks;

    public float interval = 60f;

    const int INF = 1000;

    public int width;
    public int height;

    public int first_castle_x;
    public int first_castle_y;

    [SerializeField] GameObject panel;
    [SerializeField] GameObject zoomPanel;
    [SerializeField] GameObject colliderPanel;

    [SerializeField] GameObject dark_cell_prefab;

    public Vector2 cellSize;

    //int[,] cells;
    //GameObject[,] new_cells;

    public GameObject[,] dark_cells;
    public Cell[,] cells;
    public bool[,] villager_here;
    public bool[,] king_here;
    public bool[,] ready_to_claim;
    //bool[,] is_opened;

    System.Random random = new System.Random();

    private void Start()
    {

        if (DifficultyManager.Instance != null)
        {
            //Debug.Log($"ovhfdsohndiooi111111111 {width} {height}");
            float widthMultiplier = DifficultyManager.Instance.Difficulty.WidthMultiplier;
            float heightMultiplier = DifficultyManager.Instance.Difficulty.HeightMultiplier;

            SetWidth(widthMultiplier);
            SetHeight(heightMultiplier);
            //Debug.Log($"ovhfdsohndiooi22222222 {width} {height} {widthMultiplier} {heightMultiplier}");
        }

        gameController = GameController.GetComponent<GameController>();

        cellsScript = GetComponent<CellsScript>();

        cellSize = panel.GetComponent<GridLayoutGroup>().cellSize;
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize.x * width, cellSize.y * height);
        zoomPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize.x * width, cellSize.y * height);
        checkPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize.x * width, cellSize.y * height);

        StartNewGame();

        //InvokeRepeating("IncreaseAmount", interval, interval);
        //InvokeRepeating("WheatGrow", interval / 2, interval / 2);
    }

    public void IncreaseTimer()
    {
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
            {
                if (cells[i, j].destroyable == true)
                {
                    cells[i, j].time_for_peak--;

                    if (cells[i, j].time_for_peak < 0)
                    {
                        IncreaseAmount(i, j);
                        // ready_to_claim
                        ready_to_claim[i, j] = true;
                        cells[i, j].time_for_peak = 60;
                    }

                    if ((cells[i, j].type == "wheat") && (cells[i, j].time_for_peak == 30))
                    {
                        WheatGrow(i, j);
                    }
                }
            }
    }

    public void IncreaseAmount(int i, int j)
    {
        if (cells[i, j].is_destroyed == false)
        {
            if (cells[i, j].coin_per_time > 0)
            {
                cells[i, j].coin_amount += cells[i, j].coin_per_time;
            }
            if (cells[i, j].wheat_per_time > 0)
            {
                cells[i, j].wheat_amount += cells[i, j].wheat_per_time;
            }

            if (cells[i, j].type == "wheat")
            {
                Cell new_wheat = FindCellByType("wheat", 2, 0, false, cells[i, j].index);
                int wheat_amount = cells[i, j].wheat_amount;
                cells[i, j] = new_wheat;
                cells[i, j].wheat_amount = wheat_amount;
                dark_cells[i, j].GetComponent<Image>().sprite = new_wheat.sprite;
            }
        }
    }

    public void WheatGrow(int i, int j)
    {
        if (cells[i, j].is_destroyed == false)
        {
            if ((cells[i, j].type == "wheat") && (cells[i, j].level == 0))
            {
                Cell last_cell = cells[i, j];
                Cell new_wheat = FindCellByType("wheat", 1, 0, false, cells[i, j].index);
                cells[i, j] = new_wheat;
                dark_cells[i, j].GetComponent<Image>().sprite = new_wheat.sprite;
                cells[i, j].time_for_peak = last_cell.time_for_peak;
            }
        }
    }

    public void StartNewGame()
    {
        villager_here = new bool[height, width];
        king_here = new bool[height, width];
        ready_to_claim = new bool[height, width];
        dark_cells = new GameObject[height, width];
        cells = new Cell[height, width];
        checks = new GameObject[height, width];

        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
            {
                //fit_cells[i, j] = Instantiate(dark_cell_prefab, Vector3.zero, Quaternion.identity, transform);
                //fit_cells[i, j].GetComponent<Image>().sprite = gameController.empty_sprite;

                InitDarkCell(i, j);

                dark_cells[i, j].gameObject.GetComponent<AddCellScript>().index_i = i;
                dark_cells[i, j].gameObject.GetComponent<AddCellScript>().index_j = j;

                checks[i, j].gameObject.GetComponent<ColliderScript>().index_i = i;
                checks[i, j].gameObject.GetComponent<ColliderScript>().index_j = j;

                checks[i, j].gameObject.GetComponent<ColliderScript>().index_i = i;
                checks[i, j].gameObject.GetComponent<ColliderScript>().index_j = j;

                ready_to_claim[i, j] = false;
            }

        PlaceRandomCastle();
    }

    public void OnCellClick(int i, int j)
    {
        //if (cells[i, j].type == "castle")
        if (dark_cells[i, j].GetComponent<Image>() != null)
        {
            //Debug.Log("YOOO BROO");
        }
    }

    public void PlaceRandomCastle()
    {
        int castle_x = random.Next(height / 3, 2 * height / 3);
        int castle_y = random.Next(width / 3, 2 * width / 3);

        Cell cell = FindCellByType("castle", 0, 1, false, -1);
        cells[castle_x, castle_y] = cell;
        dark_cells[castle_x, castle_y].GetComponent<Image>().sprite = cell.sprite;
        ChangeCellTag(castle_x, castle_y, "Village");

        first_castle_x = castle_x;
        first_castle_y = castle_y;
        Invoke("CreateFirstVillager", 2f);

        cell = FindCellByType("quater_village", 0, 0, false, -1);
        cells[castle_x, castle_y + 1] = cell;
        dark_cells[castle_x, castle_y + 1].GetComponent<Image>().sprite = cell.sprite;
        ChangeCellTag(castle_x, castle_y + 1, "Village");

        cell = FindCellByType("quater_village", 0, 2, false, -1);
        cells[castle_x - 1, castle_y] = cell;
        dark_cells[castle_x - 1, castle_y].GetComponent<Image>().sprite = cell.sprite;
        ChangeCellTag(castle_x - 1, castle_y, "Village");

        cell = FindCellByType("wheat", 0, 0, false, -1);
        cells[castle_x, castle_y - 1] = cell;
        dark_cells[castle_x, castle_y - 1].GetComponent<Image>().sprite = cell.sprite;
        ChangeCellTag(castle_x, castle_y - 1, "Village");

        cell = FindCellByType("road", 4, 4, false, -1);
        cells[castle_x + 1, castle_y] = cell;
        dark_cells[castle_x + 1, castle_y].GetComponent<Image>().sprite = cell.sprite;
        ChangeCellTag(castle_x + 1, castle_y, "Village");
    }

    private void CreateFirstVillager()
    {
        Vector3 new_villager = checks[first_castle_x, first_castle_y].transform.position;
        zoomPanel.GetComponent<EnemySpawner>().VillagerSpawn(new_villager, first_castle_x, first_castle_y);
        //CreateVillager(first_castle_x, first_castle_y);
    }

    public void CreateVillager(int i, int j)
    {
        i++;
        j++;
        Vector2 coords = zoomPanel.transform.position;
        float x = (float)(coords.x - (width / 2 + (width % 2 == 0 ? 0 : 0.5) - j + 0.5) * cellSize.x); 
        float y = (float)(coords.y + (height / 2 + (height % 2 == 0 ? 0 : 0.5) - i + 0.5) * cellSize.y);

        Vector3 villagerSpawnPoint = new Vector3(x, y, 0f);

        zoomPanel.GetComponent<EnemySpawner>().VillagerSpawn(villagerSpawnPoint, i - 1, j - 1);

        //Vector3 new_villager = checks[i, j].transform.position;
        //zoomPanel.GetComponent<EnemySpawner>().VillagerSpawn(new_villager, i, j);
    }

    private void InitDarkCell(int i, int j)
    {
        dark_cells[i, j] = Instantiate(dark_cell_prefab, Vector3.zero, Quaternion.identity, transform);
        RectTransform rect_transform = dark_cells[i, j].GetComponent<RectTransform>();

        rect_transform.anchorMin = new Vector2(i * 1.0f / width, j * 1.0f / height);
        rect_transform.anchorMax = new Vector2((i + 1) * 1.0f / width, (j + 1) * 1.0f / height);

        rect_transform.offsetMin = Vector2.zero;
        rect_transform.offsetMax = Vector2.zero;

        int index = random.Next(0, 3);
        dark_cells[i, j].GetComponent<Image>().sprite = cellsScript.all_cells[index].sprite;

        dark_cells[i, j].SetActive(true);

        checks[i, j] = Instantiate(collider, Vector3.zero, Quaternion.identity, colliderPanel.transform);
        rect_transform = checks[i, j].GetComponent<RectTransform>();

        rect_transform.anchorMin = new Vector2(i * 1.0f / width, j * 1.0f / height);
        rect_transform.anchorMax = new Vector2((i + 1) * 1.0f / width, (j + 1) * 1.0f / height);

        rect_transform.offsetMin = Vector2.zero;
        rect_transform.offsetMax = Vector2.zero;

        checks[i, j].SetActive(true);
        //Debug.Log($"{dark_cells[i, j].GetComponent<Image>().sprite.name} {i} {j}");
    }

    public Cell FindCellByType(string type, int level, int count_of_road, bool is_destroyed, int index)
    {
        for (int i = 0; i < cellsScript.all_cells.Length; i++)
            if ((cellsScript.all_cells[i].type == type) &&
                ((level == -10) ? (true) : (cellsScript.all_cells[i].level == level)) &&
                ((count_of_road == -10) ? (true) : (cellsScript.all_cells[i].count_of_road == count_of_road)) &&
                (cellsScript.all_cells[i].is_destroyed == is_destroyed) &&
                ((index == -1) ? (true) : (cellsScript.all_cells[i].index == index)))
            {
                return cellsScript.all_cells[i];
            }
        return cellsScript.all_cells[0];
    }

    public void ChangeCellTag(int x, int y, string new_tag)
    {
        checks[x, y].tag = new_tag;
    }

    public void SetWidth(float multiplier)
    {
        width *= (int)multiplier;
        //widthMultipler = multiplier;
        //Debug.Log("ovhfdsohndiooi");
    }

    public void SetHeight(float multiplier)
    {
        height *= (int)multiplier;
    }
}
