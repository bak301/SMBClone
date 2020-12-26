using System;
using UnityEngine;

public class EnemyShooter : EnemyBase, IEnemy, IActor
{
    [SerializeField] private float fireInterval;
    [SerializeField] private Bullet bullet;
    [SerializeField] private float detectionRange;

    private bool foundPlayer;
    private float fireTimer;
    // Start is called before the first frame update
    void Start()
    {
        fireTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    private void ScanForPlayer()
    {

    }

    private void Fire()
    {
        if (fireTimer < fireInterval)
        {
            fireTimer += Time.deltaTime;
        } else
        {
            BasicAttack();
            fireTimer = 0;
        }
    }

    public void BasicAttack()
    {
        Vector3 position = new Vector3(this.transform.position.x,
                                        transform.position.y,
                                        transform.position.z);
        
        SimplePool.Spawn(bullet, position,Quaternion.identity).SetOwner(name);
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
    }

    public void OnDeath()
    {
        throw new NotImplementedException();
    }
}
