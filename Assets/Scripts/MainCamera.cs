using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private Transform player;
    [SerializeField] private float offsetY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.isEngaged == false)
        {
            this.transform.position = new Vector3(player.position.x,
                                              player.position.y + offsetY,
                                              this.transform.position.z);
        } else
        {
            transform.DOMove(new Vector3(152, 6, -10), 1);
        }
    }
}
