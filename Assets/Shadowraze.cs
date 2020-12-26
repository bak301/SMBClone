using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadowraze : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] private float lifetime;

    internal float timer;
    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer--;
        } else
        {
            SimplePool.Despawn(gameObject);
        }
    }

    public void ResetTimer()
    {
        timer = lifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<IActor>().ReceiveDamage(damage);
        }
    }
}
