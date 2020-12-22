using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] protected float delay;
    [SerializeField] protected float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            collision.gameObject.GetComponent<IActor>().ReceiveDamage(damage);
        }
    }
}
