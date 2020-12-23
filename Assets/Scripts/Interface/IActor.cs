using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor
{
    void ReceiveDamage(float damage);
    void OnDeath();
    void FireBullet(Bullet bullet);
}
