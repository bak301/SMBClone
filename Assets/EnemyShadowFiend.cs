using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShadowFiend : EnemyBase, IActor
{
    [SerializeField] private Warning warning;
    [SerializeField] private Shadowraze raze;
    [SerializeField] private float initialDelay;
    [SerializeField] private float razeCooldown;
    [SerializeField] private float razeInterval;
    [SerializeField] private float side;

    private float razeTimer;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyState.ENGAGED;
        razeTimer = 0 - initialDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (razeTimer >= razeCooldown)
        {
            razeTimer = 0;
            BasicAttack();
        } else
        {
            razeTimer += Time.deltaTime;
        }
    }

    public void BasicAttack()
    {
        StartCoroutine(Warning());
        StartCoroutine(TripleRaze());
    }

    private IEnumerator Warning()
    {
        WarningBeforeRaze(2);
        yield return new WaitForSeconds(razeInterval);
        WarningBeforeRaze(5);
        yield return new WaitForSeconds(razeInterval);
        WarningBeforeRaze(8);
    }

    private IEnumerator TripleRaze()
    {
        yield return new WaitForSeconds(warning.lifetime);
        Shadowraze(2);
        yield return new WaitForSeconds(razeInterval);
        Shadowraze(5);
        yield return new WaitForSeconds(razeInterval);
        Shadowraze(8);
    }

    private void WarningBeforeRaze(float range)
    {
        SimplePool.Spawn(warning, transform.position + new Vector3(side * range, -1, 0), Quaternion.identity);
    }

    private void Shadowraze(float range)
    {
        SimplePool.Spawn(raze, transform.position + new Vector3(side*range, -3 , 0), Quaternion.identity);
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            OnDeath();
        }
    }



    // Collider
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.GetComponent<Player>().isInvulnerable == false)
        {
            collision.GetComponent<IActor>().ReceiveDamage(collisionDamage);
        }
    }
}
