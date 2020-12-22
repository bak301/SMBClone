using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    internal bool isCollided;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        isCollided = true;
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
            FixPosition(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("wall"))
        {
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

    private void FixPosition(Collider2D collision)
    {
        player.transform.position = new Vector3(player.transform.position.x,
                                                collision.bounds.max.y + player.GetComponent<BoxCollider2D>().bounds.extents.y,
                                                player.transform.position.z);
    }
}
