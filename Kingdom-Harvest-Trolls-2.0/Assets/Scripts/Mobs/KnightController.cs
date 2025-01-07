using Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    public GameObject target;
    private MouseUIController controller;
    GameController gameController;

    public int max_health = 50;
    private int current_health;
    public int attack = 5;
    public float speed = 15;
    public bool is_attacing = false;

    private float zoom = 1f;

    private bool is_flipped = false;
    private bool is_dead = false;

    //private string targetTag = "Troll";

    [SerializeField] GameObject knight_dialog;
    [SerializeField] TextMeshProUGUI knight_speech;

    System.Random random = new System.Random();

    private string[] in_touch = {
        "Fight",
        "Ouch",
        "Hi!",
        "Yes",
        "Duty",
        "Ready"
    };

    private void Start()
    {
        current_health = max_health;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MouseUIController>();

        target = null;

        knight_dialog.SetActive(false);
    }

    private void Update()
    {
        /*
        GameObject[] colliders = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject closestTarget = null;

        if (colliders.Length > 0)
        {
            float closestDistance = Mathf.Infinity;

            foreach (GameObject collider in colliders)
            {
                if (collider.gameObject.CompareTag(targetTag))
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);

                    if (distance < closestDistance)
                    {
                        closestTarget = collider;
                        closestDistance = distance;
                    }
                }
            }
        }

        target = closestTarget;
        */

        //target = GameObject.FindGameObjectWithTag("Troll");

        zoom = controller.zoom;

        if (is_attacing == false)
        {
            Move();
        }

        if (current_health <= 0)
        {
            if (is_dead == false)
            {
                is_dead = true;
                DeathKnight();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Troll")
        {
            is_attacing = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Troll")
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

        Vector3 dialog_scale = knight_dialog.transform.localScale;
        knight_dialog.transform.localScale = new Vector3(-dialog_scale.x, dialog_scale.y, dialog_scale.z);

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
        knight_dialog.SetActive(true);
        knight_speech.text = speech;

        Invoke("HideDialog", time);
    }

    private void HideDialog()
    {
        knight_dialog.SetActive(false);
    }

    public void DeathKnight()
    {
        gameController.IncreaseKnightAmount(-1);
        gameController.OpenDude("Your forces are taking casualties, my king!");
        DestroyKnight();
    }

    private void DestroyKnight()
    {
        Destroy(gameObject, 0f);
    }
}
