using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    private MouseUIController controller;
    private GameController gameController;
    private FieldScript fieldScript;
    private GameObject zoomPanel;

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

    System.Random random = new System.Random();

    private void Start()
    {
        current_health = max_health;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MouseUIController>();
        fieldScript = GameObject.Find("FieldPanel").GetComponent<FieldScript>();

        zoomPanel = GameObject.Find("ZoomPanel");

        FindWay();
    }

    private void Update()
    {
        zoom = controller.zoom;

        if ((index_i != go_to_x) || (index_j != go_to_y))
        {
            is_moving = true;
            Move();

            if ((index_i == go_to_x) && (index_j == go_to_y))
            {
                float time = UnityEngine.Random.Range(3f, 6f);
                Invoke("FindWay", time);
            }
        }
        else
        {
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
        /*Vector2 coords = zoomPanel.transform.position;
        float x = (float)(coords.x - (fieldScript.width / 2 + (fieldScript.width % 2 == 0 ? 0 : 0.5) - (go_to.y + 1) + 0.5) * fieldScript.cellSize.x) * zoom;
        float y = (float)(coords.y + (fieldScript.height / 2 + (fieldScript.height % 2 == 0 ? 0 : 0.5) - (go_to.x + 1) + 0.5) * fieldScript.cellSize.y) * zoom;
        Vector3 new_way = new Vector3(x, y, 0f);*/

        Vector3 new_way = fieldScript.checks[go_to_x, go_to_y].transform.position;

        Vector3 direction = new_way - transform.position;
        if (direction == new Vector3(0f, 0f, 0f))
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
    }

    private void FindWay()
    {
        Tuple<int, int>[] ways = new Tuple<int, int>[0];

        if (fieldScript.cells[index_i, index_j].down == "road")
        {
            if (index_i + 1 < fieldScript.height)
                if (fieldScript.cells[index_i + 1, index_j].up == "road")
                {
                    System.Array.Resize(ref ways, ways.Length + 1);
                    ways[ways.Length - 1] = new Tuple<int, int>(index_i + 1, index_j);
                }
        }

        if (fieldScript.cells[index_i, index_j].up == "road")
        {
            if (index_i - 1 > -1)
                if (fieldScript.cells[index_i - 1, index_j].down == "road")
                {
                    System.Array.Resize(ref ways, ways.Length + 1);
                    ways[ways.Length - 1] = new Tuple<int, int>(index_i - 1, index_j);
                }
        }

        if (fieldScript.cells[index_i, index_j].right == "road")
        {
            if (index_j + 1 < fieldScript.width)
                if (fieldScript.cells[index_i, index_j + 1].left == "road")
                {
                    System.Array.Resize(ref ways, ways.Length + 1);
                    ways[ways.Length - 1] = new Tuple<int, int>(index_i, index_j + 1);
                }
        }

        if (fieldScript.cells[index_i, index_j].left == "road")
        {
            if (index_j - 1 > -1)
                if (fieldScript.cells[index_i, index_j - 1].right == "road")
                {
                    System.Array.Resize(ref ways, ways.Length + 1);
                    ways[ways.Length - 1] = new Tuple<int, int>(index_i, index_j - 1);
                }
        }

        int index = random.Next(0, ways.Length);
        go_to_x = ways[index].Item1;
        go_to_y = ways[index].Item2;

        Debug.Log($"moves from {index_i} {index_j} to {go_to_x} {go_to_y}");

        System.Array.Resize(ref ways, 0);
    }

    public void Flip()
    {
        Vector3 scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        is_flipped = !is_flipped;
    }

    public void TakeDamage(int amount)
    {
        current_health -= amount;
    }

    public void DeathVillager()
    {
        gameController.IncreaseKnightAmount(-1);
        gameController.OpenDude("The population of villagers is decreasing!");
        DestroyVillager();
    }

    private void DestroyVillager()
    {
        Destroy(gameObject, 0f);
    }
}
