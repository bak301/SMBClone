using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float side;
    [SerializeField] private float speed;
    private float lifetime;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        lifetime = 3;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * side * Time.deltaTime * speed);
        timer += Time.deltaTime;

        if (timer > lifetime)
        {
            SimplePool.Despawn(gameObject);
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IActor damageReceiver = collision.gameObject.GetComponent<IActor>();
        if (damageReceiver != null)
        {
            damageReceiver.ReceiveDamage(damage);
            SimplePool.Despawn(gameObject);
        }
        
    }
}
