using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollSwordScript : MonoBehaviour
{
    [SerializeField] GameObject Troll;
    TrollController troll;

    private void Start()
    {
        troll = Troll.GetComponent<TrollController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Knight")
        {
            KnightController knight = collision.gameObject.GetComponent<KnightController>();

            int damage = troll.attack;

            knight.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Knight")
        {
            KnightController knight = collider.GetComponent<KnightController>();

            int damage = troll.attack;

            knight.TakeDamage(damage);
        }
    }
}
