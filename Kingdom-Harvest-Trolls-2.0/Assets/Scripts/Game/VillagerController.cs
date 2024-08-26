using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    private MouseUIController controller;
    private GameController gameController;
    private FieldScript fieldScript;

    public int max_health = 50;
    private int current_health;
    public float speed = 10;
    public bool is_moving = true;

    private float zoom = 1f;

    private bool is_flipped = false;
    private bool is_dead = false;

    public int index_i;
    public int index_j;

    private void Start()
    {
        current_health = max_health;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MouseUIController>();
        fieldScript = GameObject.Find("FieldPanel").GetComponent<FieldScript>();
    }

    private void Update()
    {
        zoom = controller.zoom;

        Move();

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
        Vector3 direction = transform.position - transform.position;

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
