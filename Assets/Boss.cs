using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase, IActor
{
    public void FireBullet(Bullet bullet)
    {

    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
