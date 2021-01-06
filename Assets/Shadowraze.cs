using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shadowraze : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] private float lifetime;

    private SpriteRenderer rend;
    internal float timer;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0.3f);
        
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        timer = lifetime;
        transform.localScale = new Vector3(transform.localScale.x, 0.2f, transform.localScale.z);
        DOTween.Sequence()
            .Append(rend.DOFade(1, lifetime / 2))
                .Join(transform.DOScaleY(2, lifetime / 2))
            .Append(rend.DOFade(0.3f, lifetime / 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer-= Time.deltaTime;
        } else
        {
            SimplePool.Despawn(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<IActor>().ReceiveDamage(damage);
        }
    }
}
