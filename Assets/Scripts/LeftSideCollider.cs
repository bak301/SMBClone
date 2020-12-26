﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSideCollider : MonoBehaviour
{
    internal bool isCollided;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        isCollided = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("wall"))
        {
            isCollided = true;
            if (collision.bounds.max.x < player.transform.position.x)
            {
                FixPosition(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("wall"))
        {
            isCollided = false;
        }
    }

    private void FixPosition(Collider2D collision)
    {
        player.transform.position = new Vector3(collision.bounds.max.x + player.GetComponent<BoxCollider2D>().bounds.extents.x,
                                                player.transform.position.y,
                                                player.transform.position.z);
    }
}