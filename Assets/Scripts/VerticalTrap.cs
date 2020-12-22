using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class VerticalTrap : Trap
{
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Sequence()
            .SetDelay(delay)
            .Append(transform.DOLocalMoveY(transform.localPosition.y - 5, 0.5f))
            .Append(transform.DOLocalMoveY(transform.localPosition.y, 1))
            .SetLoops(-1);
    }
}
