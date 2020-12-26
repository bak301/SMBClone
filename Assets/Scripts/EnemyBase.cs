using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public enum EnemyState
    {
        IDLE,
        ENGAGED,
        DEAD
    }

    [SerializeField] protected float health;
    [SerializeField] protected EnemyState state;

    // Start is called before the first frame update
    void Start()
    {
        this.state = EnemyState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
