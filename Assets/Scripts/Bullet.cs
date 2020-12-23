using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float side;
    [SerializeField] private float speed;
    
    private string owner;
    private Vector3 direction;
    private float lifetime;
    private float timer;
    // Start is called before the first frame update
    void Awake()
    {
        direction = Vector3.left;
        lifetime = 3;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
        timer += Time.deltaTime;

        if (timer > lifetime)
        {
            SimplePool.Despawn(gameObject);
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != owner)
        {
            Debug.Log($"{owner} shot {collision.gameObject.name}");
            IActor damageReceiver = collision.gameObject.GetComponent<IActor>();
            if (damageReceiver != null)
            {
                damageReceiver.ReceiveDamage(damage);
                SimplePool.Despawn(gameObject);
            }
        } 
    }

    public void SetBulletDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void SetOwner(string name)
    {
        this.owner = name;
    }
}
