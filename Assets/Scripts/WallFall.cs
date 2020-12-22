using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFall : MonoBehaviour
{
    [SerializeField] private float delay;
    private bool isFalling = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GroundCheck" && isFalling == false)
        {
            DOTween.Sequence()
                .Append(transform.DOShakePosition(delay, new Vector3(1,0,0)))
                .Append(transform.DOLocalMoveY(transform.position.y - 50, 3).SetEase(Ease.InCubic));
            isFalling = true;
        }
    }
}
