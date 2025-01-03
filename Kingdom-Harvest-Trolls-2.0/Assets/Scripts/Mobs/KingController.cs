using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour
{
    public GameObject target;
    private MouseUIController controller;
    GameController gameController;

    public int max_health = 100;
    private int current_health;
    public float speed = 15;

    private float zoom = 1f;

    private bool is_flipped = false;
    private bool is_dead = false;

    public float min_difference = 0.05f;

    private string targetTag = "Village";
    private bool is_waiting = false;

    public int king_x;
    public int king_y;

    private void Start()
    {
        current_health = max_health;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MouseUIController>();

        target = null;
    }

    private void Update()
    {
        zoom = controller.zoom;

        if (!is_waiting)
        {
            GameObject[] colliders = GameObject.FindGameObjectsWithTag(targetTag);

            target = FindClosestObject(colliders);
        }

        if (target != null)
        {
            Move();
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

    private void Move()
    {
        Vector3 direction = target.transform.position - transform.position;
        if ((Math.Abs(direction.x) < min_difference) && (Math.Abs(direction.y) < min_difference) && !is_waiting)
        {
            target = null;
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
        
        InvokeClaimTaxes();
    }

    private void InvokeClaimTaxes()
    {
        target = null;
        is_waiting = true;
        float time = UnityEngine.Random.Range(2f, 5f);
        Invoke("ClaimTaxes", time);
    }

    private void ClaimTaxes()
    {
        // считать x, y текущей клетки и собрать накопленный налог

        is_waiting = false;
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

    public void DeathKing()
    {
        gameController.IncreaseKingAmount(-1);
        gameController.OpenDude("Your regent died a brave death, my king!");
        DestroyKnight();
    }

    private void DestroyKnight()
    {
        Destroy(gameObject, 0f);
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
}
