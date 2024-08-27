using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VillagerController : MonoBehaviour
{
    private MouseUIController controller;
    private GameController gameController;
    private FieldScript fieldScript;
    private GameObject zoomPanel;

    [SerializeField] GameObject villager_dialog;
    [SerializeField] TextMeshProUGUI villager_speech;

    public int max_health = 50;
    private int current_health;
    public float speed = 10;
    public bool is_moving = false;

    private float zoom = 1f;

    private bool is_flipped = false;
    private bool is_dead = false;

    public int index_i;
    public int index_j;

    private int go_to_x;
    private int go_to_y;

    public float min_difference;

    private bool needs_help = false;

    private string help = "HELP!";
    private string[] in_touch = { 
        "Hmm?",
        "Ah??",
        "???",
        "Hello!",
        "Hehe",
        "Beep"
    };

    System.Random random = new System.Random();

    private void Start()
    {
        current_health = max_health;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MouseUIController>();
        fieldScript = GameObject.Find("FieldPanel").GetComponent<FieldScript>();

        zoomPanel = GameObject.Find("ZoomPanel");

        villager_dialog.SetActive(false);

        //transform.position = fieldScript.checks[index_i, index_j].transform.position;

        go_to_x = index_i;
        go_to_y = index_j;

        FindWay();
    }

    private void Update()
    {
        zoom = controller.zoom;

        if (fieldScript.cells[go_to_x, go_to_y].is_destroyed == false || fieldScript.cells[index_i, index_j].is_destroyed == false)
        {
            if (needs_help == true)
            {
                HideDialog();
                needs_help = false;
            }
        }

        if ((index_i != go_to_x) || (index_j != go_to_y))
        {
            is_moving = true;
            Move();

            /*if ((index_i == go_to_x) && (index_j == go_to_y))
            {
                float time = UnityEngine.Random.Range(3f, 6f);
                Invoke("FindWay", time);
            }*/
        }
        else
        {
            if (is_moving == true)
            {
                InvokeFindWay();
            }
            is_moving = false;
        }

        if (current_health <= 0)
        {
            if (is_dead == false)
            {
                is_dead = true;
                DeathVillager();
            }
        }
    }

    public void InTouch()
    {
        if (needs_help == false)
        {
            int index = random.Next(0, in_touch.Length);
            ShowDiaog(in_touch[index], 3f);
        }
    }

    private void InvokeFindWay()
    {
        float time = UnityEngine.Random.Range(3f, 6f);
        Invoke("FindWay", time);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void Move()
    {
        Vector3 new_way = fieldScript.checks[go_to_x, go_to_y].transform.position;

        Vector3 direction = new_way - transform.position;
        if ((Math.Abs(direction.x) < min_difference) && (Math.Abs(direction.y) < min_difference))
        {
            index_i = go_to_x;
            index_j = go_to_y;
        }

        if ((direction.x < 0) && (is_flipped == false))
        {
            Flip();
        }
        if ((direction.x > 0) && (is_flipped == true))
        {
            Flip();
        }

        direction.Normalize();

        transform.position += direction * speed * zoom * Time.deltaTime;
        //transform.position = new_way;
    }

    private void FindWay()
    {
        Tuple<int, int>[] ways = new Tuple<int, int>[0];

        if (fieldScript.cells[index_i, index_j].down == "road")
        {
            if (index_i + 1 < fieldScript.height)
                if (fieldScript.cells[index_i + 1, index_j].up == "road")
                {
                    if (fieldScript.cells[index_i + 1, index_j].is_destroyed == false/* || fieldScript.cells[index_i + 1, index_j].is_just_road == false*/)
                    {
                        System.Array.Resize(ref ways, ways.Length + 1);
                        ways[ways.Length - 1] = new Tuple<int, int>(index_i + 1, index_j);
                    }
                }
        }

        if (fieldScript.cells[index_i, index_j].up == "road")
        {
            if (index_i - 1 > -1)
                if (fieldScript.cells[index_i - 1, index_j].down == "road")
                {
                    if (fieldScript.cells[index_i - 1, index_j].is_destroyed == false/* || fieldScript.cells[index_i - 1, index_j].is_just_road == false*/)
                    {
                        System.Array.Resize(ref ways, ways.Length + 1);
                        ways[ways.Length - 1] = new Tuple<int, int>(index_i - 1, index_j);
                    }
                }
        }

        if (fieldScript.cells[index_i, index_j].right == "road")
        {
            if (index_j + 1 < fieldScript.width)
                if (fieldScript.cells[index_i, index_j + 1].left == "road")
                {
                    if (fieldScript.cells[index_i, index_j + 1].is_destroyed == false/* || fieldScript.cells[index_i, index_j + 1].is_just_road == false*/)
                    {
                        System.Array.Resize(ref ways, ways.Length + 1);
                        ways[ways.Length - 1] = new Tuple<int, int>(index_i, index_j + 1);
                    }
                }
        }

        if (fieldScript.cells[index_i, index_j].left == "road")
        {
            if (index_j - 1 > -1)
                if (fieldScript.cells[index_i, index_j - 1].right == "road")
                {
                    if (fieldScript.cells[index_i, index_j - 1].is_destroyed == false/* || fieldScript.cells[index_i, index_j - 1].is_just_road == false*/)
                    {
                        System.Array.Resize(ref ways, ways.Length + 1);
                        ways[ways.Length - 1] = new Tuple<int, int>(index_i, index_j - 1);
                    }
                }
        }

        if (ways.Length > 0)
        {
            int index = random.Next(0, ways.Length);
            go_to_x = ways[index].Item1;
            go_to_y = ways[index].Item2;

            if (needs_help == true)
            {
                HideDialog();
                needs_help = false;
            }
        }
        else
        {
            go_to_x = index_i;
            go_to_y = index_j;

            if (fieldScript.cells[index_i, index_j].is_destroyed == true)
            {
                if (needs_help == false)
                {
                    AskForHelp(help, 30f);
                }

                needs_help = true;
            }

            InvokeFindWay();
        }

        //Debug.Log($"moves from {index_i} {index_j} to {go_to_x} {go_to_y}");

        System.Array.Resize(ref ways, 0);
    }

    private void ShowDiaog(string speech, float time)
    {
        villager_dialog.SetActive(true);
        villager_speech.text = speech;

        Invoke("HideDialog", time);
    }

    private void AskForHelp(string speech, float time)
    {
        villager_dialog.SetActive(true);
        villager_speech.text = speech;

        Invoke("DieOrLive", time);
    }

    private void DieOrLive()
    {
        if (needs_help == true)
        {
            DeathVillager();
        }
        else
        {
            HideDialog();
        }
    }

    private void HideDialog()
    {
        villager_dialog.SetActive(false);
    }

    public void Flip()
    {
        Vector3 scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);

        Vector3 dialog_scale = villager_dialog.transform.localScale;
        villager_dialog.transform.localScale = new Vector3(-dialog_scale.x, dialog_scale.y, dialog_scale.z);

        is_flipped = !is_flipped;
    }

    public void TakeDamage(int amount)
    {
        current_health -= amount;
    }

    public void DeathVillager()
    {
        gameController.IncreaseVillagerAmount(-1);
        gameController.OpenDude("The population of villagers is decreasing!");
        DestroyVillager();
    }

    private void DestroyVillager()
    {
        Destroy(gameObject, 0f);
    }
}
