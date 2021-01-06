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
    FLOWER,
    RAMPANT,
    DEAD
}

public class Boss : EnemyBase, IActor
{
    [SerializeField] private float rampantCooldown;
    [SerializeField] private float maxHealth;
    [SerializeField] private float rageThresholdRate;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fireInterval;
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform target;
    [SerializeField] internal bool isEngaged;
    [SerializeField] internal bool isRaged;

    [SerializeField] private List<Transform> walls;

    private float rampantTimer;
    private float fireTimer;
    private Vector3 direction;
    private BossBattleState battleState;

    // Start is called before the first frame update
    void Start()
    {
        isRaged = false;
        isEngaged = false;
        fireTimer = 0;
        health = maxHealth;
        rampantTimer = 0;
    }

    void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        direction = (target.position - transform.position).normalized;
        UpdateState();
 
        switch (battleState)
        {
            case BossBattleState.IDLE:
                break;
            case BossBattleState.NORMAL:
                Chase(1);
                Fire(3);
                break;
            case BossBattleState.RAGED:
                rampantTimer += Time.deltaTime;
                if (rampantTimer >= rampantCooldown)
                {
                    rampantTimer = 0;
                    StartCoroutine(FlowerFiring());
                }

                if (new System.Random().Next(1000) == 1)
                {
                    StartCoroutine(Rampant());
                }
                Chase(2);
                Fire(4);
                break;
            case BossBattleState.RAMPANT:
            case BossBattleState.FLOWER:
            case BossBattleState.DEAD:
                break;
            default:
                break;
        }
    }

    private IEnumerator Rampant()
    {
        battleState = BossBattleState.RAMPANT;
        transform.DOMoveX(171, 1).OnComplete(()=>
        {
            DOTween.Sequence()
            .Append(transform.DOMoveX(133, 1f).SetEase(Ease.Linear))
            .Append(transform.DOShakeScale(0.5f, 10, 100))
                .Join(walls[0].DOShakePosition(0.5f, 0.2f, 3))
                .Join(walls[1].DOShakePosition(0.5f, 0.2f, 3))
                .Join(walls[2].DOShakePosition(0.5f, 0.2f, 3))
                .Join(walls[3].DOShakePosition(0.5f, 0.2f, 3))
            .Append(transform.DOMoveX(171, 1f).SetEase(Ease.Linear))
            .Append(transform.DOShakeScale(0.5f, 10, 100))
                .Join(walls[0].DOShakePosition(0.5f, 0.2f, 3))
                .Join(walls[1].DOShakePosition(0.5f, 0.2f, 3))
                .Join(walls[2].DOShakePosition(0.5f, 0.2f, 3))
                .Join(walls[3].DOShakePosition(0.5f, 0.2f, 3))
            .SetLoops(3).OnComplete(() => battleState = BossBattleState.RAGED);
        });
        yield return null;
    }

    private void UpdateState()
    {
        if (battleState != BossBattleState.FLOWER && battleState != BossBattleState.RAMPANT)
        {
            if (isEngaged)
            {
                battleState = BossBattleState.NORMAL;
            }

            if (isRaged)
            {
                battleState = BossBattleState.RAGED;
            }

            if (health < 0)
            {
                battleState = BossBattleState.DEAD;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.GetComponent<Player>().isInvulnerable == false)
        {
            collision.GetComponent<IActor>().ReceiveDamage(collisionDamage);
        }
    }

    private IEnumerator FlowerFiring()
    {
        isRaged = true;
        battleState = BossBattleState.FLOWER;

        GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0);
        transform.DOScaleX(20, 0.5f);
        transform.DOScaleY(20, 0.5f);
        transform.DOMoveX(152, 1);

        yield return new WaitForSeconds(2);
        StartCoroutine(FlowerShot(60, 19));
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
            FireBullet(bullet, Quaternion.AngleAxis(20, Vector3.forward) * direction);
            FireBullet(bullet, Quaternion.AngleAxis(-20, Vector3.forward) * direction);
        }
    }

    private IEnumerator FlowerShot(int count, float offset)
    {

        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.13f);
            FireBullet(bullet, Quaternion.AngleAxis(offset * i, Vector3.forward) * Vector3.up);
            FireBullet(bullet, Quaternion.AngleAxis(offset * i *1.1f + 60, Vector3.forward) * Vector3.up);
            FireBullet(bullet, Quaternion.AngleAxis(offset * i *1.2f + 120, Vector3.forward) * Vector3.up);
            FireBullet(bullet, Quaternion.AngleAxis(offset * i *1.3f + 180, Vector3.forward) * Vector3.up);
            FireBullet(bullet, Quaternion.AngleAxis(offset * i *1.4f + 240, Vector3.forward) * Vector3.up);
            FireBullet(bullet, Quaternion.AngleAxis(offset * i *1.5f + 300, Vector3.forward) * Vector3.up);
        }

        battleState = BossBattleState.RAGED;
    }

    public void Chase(float scale)
    {
        if (Mathf.Abs(transform.position.x - target.position.x) > 2)
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

    public void BasicAttack()
    {

    }

    public void OnDeath()
    {
        DOTween.Sequence()
            .Append(transform.DOShakeScale(2, 10, 50).SetEase(Ease.Linear))
            .Append(transform.DOScale(0, 2).SetEase(Ease.InOutCubic))
            .Append(walls[3].DOMoveY(walls[3].transform.position.y + 10, 1));
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(4);
        isEngaged = false;
        Destroy(gameObject);
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;

        if (health < 0 && battleState != BossBattleState.DEAD)
        {
            OnDeath();
        } else if (health < rageThresholdRate * maxHealth && isRaged == false)
        {
            transform.DOMoveY(transform.position.y + 0.35f, 0.1f);
            StartCoroutine(FlowerFiring());
        }
    }
}
