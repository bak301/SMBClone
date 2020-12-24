using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCheck : MonoBehaviour
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
            FixPosition(collision);
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
                                                collision.bounds.min.y - player.GetComponent<BoxCollider2D>().bounds.extents.y,
                                                player.transform.position.z);
    }
}
