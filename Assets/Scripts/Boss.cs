using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum BossBattleState
{
    IDLE,
    NORMAL,
    RAGED,
    RAMPANT,
    DEAD
}

public class Boss : EnemyBase, IActor
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float rageThresholdRate;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fireInterval;
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform target;
    [SerializeField] internal bool isEngaged;
    [SerializeField] internal bool isRaged;

    private float fireTimer;
    private Vector3 direction;
    private BossBattleState state;

    // Start is called before the first frame update
    void Start()
    {
        isRaged = false;
        isEngaged = false;
        fireTimer = 0;
        health = maxHealth;
    }

    void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        direction = (target.position - transform.position).normalized;
        UpdateState();
 
        switch (state)
        {
            case BossBattleState.IDLE:
                break;
            case BossBattleState.NORMAL:
                Chase(1);
                Fire(3);
                break;
            case BossBattleState.RAGED:
                Chase(2);
                Fire(5);
                break;
            case BossBattleState.RAMPANT:
                
            case BossBattleState.DEAD:
                break;
            default:
                break;
        }
    }

    private void UpdateState()
    {
        if (state != BossBattleState.RAMPANT)
        {
            if (isEngaged)
            {
                state = BossBattleState.NORMAL;
            }

            if (isRaged)
            {
                state = BossBattleState.RAGED;
            }

            if (health < 0)
            {
                state = BossBattleState.DEAD;
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
        isRaged = true;
        state = BossBattleState.RAMPANT;

        transform.GetComponent<SpriteRenderer>().color = new Color(255, 50, 0);
        transform.DOScaleX(20, 0.5f);
        transform.DOScaleY(20, 0.5f);
        transform.DOMoveX(171, 1);
        transform.DOMoveY(transform.position.y + 0.35f, 0.1f);
        //transform.position = new Vector3(171, transform.position.y, transform.position.z);

        yield return new WaitForSeconds(2);
        StartCoroutine(FlowerShot(100));
        DOTween.Sequence()
            .Append(transform.DOMoveX(transform.position.x - 38, 1f).SetEase(Ease.Linear))
            .Append(transform.DOMoveX(transform.position.x, 1f).SetEase(Ease.Linear))
            .SetLoops(3).OnComplete(() => state = BossBattleState.RAGED);
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
            FireBullet(bullet, direction);
        }
    }

    private IEnumerator FlowerShot(int count)
    {

        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.2f);
            FireBullet(bullet, Quaternion.AngleAxis(25 * i, Vector3.forward) * direction);
            FireBullet(bullet, Quaternion.AngleAxis(25 * i + 90, Vector3.forward) * direction);
            FireBullet(bullet, Quaternion.AngleAxis(25 * i + 180, Vector3.forward) * direction);
            FireBullet(bullet, Quaternion.AngleAxis(25 * i + 270, Vector3.forward) * direction);
        }
    }

    public void Chase(float scale)
    {
        if (Mathf.Abs(transform.position.x - target.position.x) > 3)
        {
            transform.Translate(Mathf.Sign(direction.x) * moveSpeed * Time.deltaTime * scale, 0, 0);
        }
    }

    public void FireBullet(Bullet bullet, Vector3 direction)
    {
        Bullet firingBullet = SimplePool.Spawn(bullet, transform.position, Quaternion.identity);

        firingBullet.SetOwner(name);
        firingBullet.SetBulletDirection(direction);
    }

    public void FireBullet(Bullet bullet)
    {

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
        } else if (health < rageThresholdRate * maxHealth && isRaged == false)
        {
            StartCoroutine(Rampant());
        }
    }
}
