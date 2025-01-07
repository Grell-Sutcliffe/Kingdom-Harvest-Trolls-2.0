using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrollController : MonoBehaviour
{
    public GameObject target;
    public GameObject[] enemies;
    public GameObject closestKnight1;
    public GameObject closestKnight2;
    private MouseUIController controller;
    private GameController gameController;

    public int max_health = 50;
    private int current_health;
    public int attack = 6;
    public float speed = 10;
    public bool is_attacing = false;

    private float zoom = 1f;

    private bool is_flipped = false;

    private string targetTag = "Village";
    private string enemyTag = "Knight";

    [SerializeField] GameObject troll_dialog;
    [SerializeField] TextMeshProUGUI troll_speech;

    System.Random random = new System.Random();

    private string[] in_touch = {
        "War",
        "Blood",
        "Grr",
        "...",
        "Break",
        "Ruin"
    };

    private void Start()
    {
        current_health = max_health;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MouseUIController>();

        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        closestKnight1 = FindClosestObject(enemies);
        closestKnight2 = FindClosestObject(enemies);

        troll_dialog.SetActive(false);
    }

    private void Update()
    {
        GameObject[] colliders = GameObject.FindGameObjectsWithTag(targetTag);

        target = FindClosestObject(colliders);

        if (closestKnight1 == null)
        {
            closestKnight1 = FindClosestKnight();
        }
        if (closestKnight1 != null)
        {
            closestKnight1.GetComponent<KnightController>().target = gameObject;
        }

        if (closestKnight2 == null)
        {
            closestKnight2 = FindClosestKnight();
        }
        if (closestKnight2 != null)
        {
            closestKnight2.GetComponent<KnightController>().target = gameObject;
        }

        //target = GameObject.FindGameObjectWithTag("Knight");

        if (target == null)
        {
            Invoke("PrepareLosePanel", 3.5f);
        }

        zoom = controller.zoom;

        if (is_attacing == false)
        {
            Move();
        }

        if (current_health <= 0)
        {
            DeathTroll();
        }
    }

    private GameObject FindClosestKnight()
    {
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closestTarget = null;

        if (enemies.Length > 0)
        {
            float closestDistance = Mathf.Infinity;

            foreach (GameObject obj in enemies)
            {
                if (obj.GetComponent<KnightController>().target == null)
                {
                    float distance = Vector3.Distance(transform.position, obj.transform.position);
                    
                    if (distance < closestDistance)
                    {
                        closestTarget = obj;
                        closestDistance = distance;
                    }
                }
            }
        }

        return closestTarget;
    }

    private GameObject FindClosestObject(GameObject[] list)
    {
        GameObject closestTarget = null;

        if (list.Length > 0)
        {
            float closestDistance = Mathf.Infinity;

            foreach (GameObject obj in list)
            {
                if (obj.gameObject.CompareTag(targetTag))
                {
                    float distance = Vector3.Distance(transform.position, obj.transform.position);

                    if (distance < closestDistance)
                    {
                        closestTarget = obj;
                        closestDistance = distance;
                    }
                }
            }
        }

        return closestTarget;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLISION");
        if (collision.gameObject.tag == "Knight")
        {
            is_attacing = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Knight")
        {
            is_attacing = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        is_attacing = false;
    }

    private void Move()
    {
        if (target == null)
            return;

        Vector3 direction = target.transform.position - transform.position;

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

    public void Flip()
    {
        Vector3 scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);

        Vector3 dialog_scale = troll_dialog.transform.localScale;
        troll_dialog.transform.localScale = new Vector3(-dialog_scale.x, dialog_scale.y, dialog_scale.z);

        is_flipped = !is_flipped;
    }

    public void TakeDamage(int amount)
    {
        current_health -= amount;
    }

    public void InTouch()
    {
        CancelInvoke("HideDialog");

        int index = random.Next(0, in_touch.Length);
        ShowDiaog(in_touch[index], 3f);
    }

    private void ShowDiaog(string speech, float time)
    {
        troll_dialog.SetActive(true);
        troll_speech.text = speech;

        Invoke("HideDialog", time);
    }

    private void HideDialog()
    {
        troll_dialog.SetActive(false);
    }

    public void DeathTroll()
    {
        DestroyTroll();
    }

    private void DestroyTroll()
    {
        Destroy(gameObject, 0f);
    }

    private void PrepareLosePanel()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);

        if (target == null)
        {
            gameController.ShowLosePanel();
            return;
        }
    }
}
