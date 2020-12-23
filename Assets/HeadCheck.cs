using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCheck : MonoBehaviour
{
    internal bool isCollided;
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
            Debug.Log("HEAD IS COLLIDED");
            isCollided = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("wall"))
        {
            isCollided = false;
        }
    }
}
