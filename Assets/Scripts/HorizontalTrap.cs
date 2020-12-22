using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HorizontalTrap : Trap
{
    [SerializeField] private float range;
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Sequence()
            .Append(transform.DOLocalMoveX(transform.localPosition.x + range, 1))
            .Append(transform.DOLocalMoveX(transform.localPosition.x, 1))
            .SetLoops(-1);
    }
}
