using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSwordScript : MonoBehaviour
{
    [SerializeField] GameObject Knight;
    KnightController knight;

    private void Start()
    {
        knight = Knight.GetComponent<KnightController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Troll")
        {
            TrollController troll = collision.gameObject.GetComponent<TrollController>();

            int damage = knight.attack;

            troll.TakeDamage(damage);
        }
    }
}
