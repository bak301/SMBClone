using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossArena : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private Transform door;

    private bool isDoorClosed;
    // Start is called before the first frame update
    void Start()
    {
        isDoorClosed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && isDoorClosed == false)
        {
            boss.isEngaged = true;
            door.DOMoveY(door.position.y - 10, 1).SetEase(Ease.OutBounce);
            isDoorClosed = true;
        }
    }
}
