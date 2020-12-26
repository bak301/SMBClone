using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShadowFiend : EnemyBase, IActor
{
    [SerializeField] private Shadowraze raze;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BasicAttack()
    {
        Shadowraze(8);
        Shadowraze(5);
        Shadowraze(2);
    }

    private void Shadowraze(float range)
    {
        SimplePool.Spawn(raze, transform.position + new Vector3(range, 0, 0), Quaternion.identity);
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    public void ReceiveDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}
