using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : EnemyBase, IActor
{
    [SerializeField] private float rageThreshold;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fireInterval;
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform target;
    [SerializeField] internal bool isEngaged;
    [SerializeField] internal bool isRaged;

    private float fireTimer;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        isRaged = false;
        isEngaged = false;
        fireTimer = 0;
    }

    void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        direction = (target.position - transform.position).normalized;

        if (isEngaged)
        {
            if (isRaged == false)
            {
                Chase(1);
                Fire(3);
            } else
            {
                Chase(2);
                Fire(3);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.GetComponent<Player>().isInvulnerable == false)
        {
            collision.GetComponent<IActor>().ReceiveDamage(3);
        }
    }

    private IEnumerator Rampant()
    {
        yield return new WaitForSeconds(2);
        transform.position = new Vector3(171, transform.position.y, transform.position.z);
        DOTween.Sequence()
            .Append(transform.DOMoveX(transform.position.x - 38, 2).SetEase(Ease.Linear))
            .Append(transform.DOMoveX(transform.position.x, 2).SetEase(Ease.Linear))
            .SetLoops(3);
    }

    private void Fire(int count)
    {
        if (fireTimer < fireInterval)
        {
            fireTimer += Time.deltaTime;
        }
        else
        {
            StartCoroutine(MultiShot(count));
            fireTimer = 0;
        }
    }

    private IEnumerator MultiShot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.2f);
            FireBullet(bullet);
        }
    }

    public void Chase(float scale)
    {
        if (Mathf.Abs(transform.position.x - target.position.x) > 3)
        {
            transform.Translate(Mathf.Sign(direction.x) * moveSpeed * Time.deltaTime * scale, 0, 0);
        }
    }

    public void FireBullet(Bullet bullet)
    {
        Bullet firingBullet = SimplePool.Spawn(bullet, transform.position, Quaternion.identity);

        firingBullet.SetOwner(name);
        firingBullet.SetBulletDirection(direction);
    }

    public void OnDeath()
    {
        DOTween.Sequence()
            .Append(transform.DOShakeScale(2, 10, 50).SetEase(Ease.Linear))
            .Append(transform.DOScale(0, 2).SetEase(Ease.InOutCubic));
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;

        if (health < 0)
        {
            OnDeath();
        } else if (health < rageThreshold && isRaged == false)
        {
            StartCoroutine(Rampant());
            isRaged = true;
            transform.GetComponent<SpriteRenderer>().color = new Color(255, 50, 0);
            transform.DOScaleX(20, 0.5f);
            transform.DOScaleY(20, 0.5f);
            transform.DOMoveY(transform.position.y + 0.35f, 0.1f);
        }
    }
}
