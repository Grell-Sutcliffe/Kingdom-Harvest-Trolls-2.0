using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KingController : MonoBehaviour
{
    private MouseUIController controller;
    private GameController gameController;
    private FieldScript fieldScript;
    private GameObject zoomPanel;

    [SerializeField] GameObject king_dialog;
    [SerializeField] TextMeshProUGUI king_speech;

    public int max_health = 100;
    private int current_health;
    public float speed = 13;
    public bool is_moving = false;

    private float zoom = 1f;

    private bool is_flipped = false;
    private bool is_dead = false;

    public int index_i;
    public int index_j;

    private int go_to_x;
    private int go_to_y;

    public float min_difference = 0.05f;

    private bool needs_help = false;

    private string help = "HELP!";
    private string[] in_touch = {
        "Taxes",
        "Oh??",
        "!!!",
        "Bye!",
        "Hoho",
        "Money"
    };

    System.Random random = new System.Random();

    private void Start()
    {
        current_health = max_health;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MouseUIController>();
        fieldScript = GameObject.Find("FieldPanel").GetComponent<FieldScript>();

        zoomPanel = GameObject.Find("ZoomPanel");

        king_dialog.SetActive(false);

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
                DeathKing();
            }
        }
    }

    public void InTouch()
    {
        CancelInvoke("HideDialog");
        if (needs_help == false)
        {
            int index = random.Next(0, in_touch.Length);
            ShowDiaog(in_touch[index], 3f);
        }
    }

    private void InvokeFindWay()
    {
        float time = UnityEngine.Random.Range(2f, 4f);
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
        fieldScript.king_here[go_to_x, go_to_y] = true;
        fieldScript.king_here[index_i, index_j] = false;

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

        // go down
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
        else if ((fieldScript.cells[index_i, index_j].down == "wheat") ||
                 (fieldScript.cells[index_i, index_j].down == "castle") || 
                 (fieldScript.cells[index_i, index_j].down == "village"))
        {
            if (index_i + 1 < fieldScript.height)
                if ((fieldScript.cells[index_i + 1, index_j].up == "wheat") ||
                    (fieldScript.cells[index_i + 1, index_j].up == "castle") ||
                    (fieldScript.cells[index_i + 1, index_j].up == "cillage"))
                {
                    if (fieldScript.cells[index_i + 1, index_j].is_destroyed == false/* || fieldScript.cells[index_i + 1, index_j].is_just_road == false*/)
                    {
                        System.Array.Resize(ref ways, ways.Length + 1);
                        ways[ways.Length - 1] = new Tuple<int, int>(index_i + 1, index_j);
                    }
                }
        }

        // go up
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
        else if ((fieldScript.cells[index_i, index_j].up == "wheat") ||
                 (fieldScript.cells[index_i, index_j].up == "castle") ||
                 (fieldScript.cells[index_i, index_j].up == "village"))
        {
            if (index_i - 1 > -1)
                if ((fieldScript.cells[index_i - 1, index_j].down == "wheat") ||
                    (fieldScript.cells[index_i - 1, index_j].down == "castle") ||
                    (fieldScript.cells[index_i - 1, index_j].down == "village"))
                {
                    if (fieldScript.cells[index_i - 1, index_j].is_destroyed == false/* || fieldScript.cells[index_i - 1, index_j].is_just_road == false*/)
                    {
                        System.Array.Resize(ref ways, ways.Length + 1);
                        ways[ways.Length - 1] = new Tuple<int, int>(index_i - 1, index_j);
                    }
                }
        }

        // go right
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
        else if ((fieldScript.cells[index_i, index_j].right == "wheat") ||
                 (fieldScript.cells[index_i, index_j].right == "castle") ||
                 (fieldScript.cells[index_i, index_j].right == "village"))
        {
            if (index_j + 1 < fieldScript.width)
                if ((fieldScript.cells[index_i, index_j + 1].left == "wheat") ||
                    (fieldScript.cells[index_i, index_j + 1].left == "castle") ||
                    (fieldScript.cells[index_i, index_j + 1].left == "village"))
                {
                    if (fieldScript.cells[index_i, index_j + 1].is_destroyed == false/* || fieldScript.cells[index_i, index_j + 1].is_just_road == false*/)
                    {
                        System.Array.Resize(ref ways, ways.Length + 1);
                        ways[ways.Length - 1] = new Tuple<int, int>(index_i, index_j + 1);
                    }
                }
        }

        // go left
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
        else if ((fieldScript.cells[index_i, index_j].left == "wheat") ||
                 (fieldScript.cells[index_i, index_j].left == "castle") || 
                 (fieldScript.cells[index_i, index_j].left == "village"))
        {
            if (index_j - 1 > -1)
                if ((fieldScript.cells[index_i, index_j - 1].right == "wheat") ||
                    (fieldScript.cells[index_i, index_j - 1].right == "castle") ||
                    (fieldScript.cells[index_i, index_j - 1].right == "village"))
                {
                    if (fieldScript.cells[index_i, index_j - 1].is_destroyed == false/* || fieldScript.cells[index_i, index_j - 1].is_just_road == false*/)
                    {
                        System.Array.Resize(ref ways, ways.Length + 1);
                        ways[ways.Length - 1] = new Tuple<int, int>(index_i, index_j - 1);
                    }
                }
        }

        // choose way (random)
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
                CancelInvoke("HideDialog");
            }

            InvokeFindWay();
        }

        //Debug.Log($"moves from {index_i} {index_j} to {go_to_x} {go_to_y}");

        System.Array.Resize(ref ways, 0);
    }

    private void ShowDiaog(string speech, float time)
    {
        king_dialog.SetActive(true);
        king_speech.text = speech;

        Invoke("HideDialog", time);
    }

    private void AskForHelp(string speech, float time)
    {
        king_dialog.SetActive(true);
        king_speech.text = speech;

        Invoke("DieOrLive", time);
    }

    private void DieOrLive()
    {
        if (needs_help == true)
        {
            DeathKing();
        }
        else
        {
            HideDialog();
        }
    }

    private void HideDialog()
    {
        king_dialog.SetActive(false);
    }

    public void Flip()
    {
        Vector3 scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);

        Vector3 dialog_scale = king_dialog.transform.localScale;
        king_dialog.transform.localScale = new Vector3(-dialog_scale.x, dialog_scale.y, dialog_scale.z);

        is_flipped = !is_flipped;
    }

    public void TakeDamage(int amount)
    {
        current_health -= amount;
    }

    public void DeathKing()
    {
        gameController.IncreaseKingAmount(-1);
        gameController.OpenDude("Your regent died a brave death, my king!");
        DestroyKing();
    }

    private void DestroyKing()
    {
        Destroy(gameObject, 0f);
    }

    //public GameObject target;
    //private MouseUIController controller;
    //GameController gameController;

    //public int max_health = 100;
    //private int current_health;
    //public float speed = 15;

    //private float zoom = 1f;

    //private bool is_flipped = false;
    //private bool is_dead = false;

    //public float min_difference = 0.05f;

    //private string targetTag = "Village";
    //private bool is_waiting = false;

    //public int king_x;
    //public int king_y;

    //private void Start()
    //{
    //    current_health = max_health;

    //    gameController = GameObject.Find("GameController").GetComponent<GameController>();
    //    controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MouseUIController>();

    //    target = null;
    //}

    //private void Update()
    //{
    //    zoom = controller.zoom;

    //    if (!is_waiting)
    //    {
    //        GameObject[] colliders = GameObject.FindGameObjectsWithTag(targetTag);

    //        target = FindClosestObject(colliders);
    //    }

    //    if (target != null)
    //    {
    //        Move();
    //    }

    //    if (current_health <= 0)
    //    {
    //        if (is_dead == false)
    //        {
    //            is_dead = true;
    //            DeathKing();
    //        }
    //    }
    //}

    //private void Move()
    //{
    //    Vector3 direction = target.transform.position - transform.position;
    //    if ((Math.Abs(direction.x) < min_difference) && (Math.Abs(direction.y) < min_difference) && !is_waiting)
    //    {
    //        target = null;
    //    }

    //    if ((direction.x < 0) && (is_flipped == false))
    //    {
    //        Flip();
    //    }
    //    if ((direction.x > 0) && (is_flipped == true))
    //    {
    //        Flip();
    //    }

    //    direction.Normalize();

    //    transform.position += direction * speed * zoom * Time.deltaTime;
        
    //    InvokeClaimTaxes();
    //}

    //private void InvokeClaimTaxes()
    //{
    //    target = null;
    //    is_waiting = true;
    //    float time = UnityEngine.Random.Range(2f, 5f);
    //    Invoke("ClaimTaxes", time);
    //}

    //private void ClaimTaxes()
    //{
    //    // считать x, y текущей клетки и собрать накопленный налог

    //    is_waiting = false;
    //}

    //public void Flip()
    //{
    //    Vector3 scale = gameObject.transform.localScale;
    //    gameObject.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
    //    is_flipped = !is_flipped;
    //}

    //public void TakeDamage(int amount)
    //{
    //    current_health -= amount;
    //}

    //public void DeathKing()
    //{
    //    gameController.IncreaseKingAmount(-1);
    //    gameController.OpenDude("Your regent died a brave death, my king!");
    //    DestroyKnight();
    //}

    //private void DestroyKnight()
    //{
    //    Destroy(gameObject, 0f);
    //}

    //private GameObject FindClosestObject(GameObject[] list)
    //{
    //    GameObject closestTarget = null;

    //    if (list.Length > 0)
    //    {
    //        float closestDistance = Mathf.Infinity;

    //        foreach (GameObject obj in list)
    //        {
    //            if (obj.gameObject.CompareTag(targetTag))
    //            {
    //                float distance = Vector3.Distance(transform.position, obj.transform.position);

    //                if (distance < closestDistance)
    //                {
    //                    closestTarget = obj;
    //                    closestDistance = distance;
    //                }
    //            }
    //        }
    //    }

    //    return closestTarget;
    //}
}
