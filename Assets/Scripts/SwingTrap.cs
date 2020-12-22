using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwingTrap : Trap
{
    [SerializeField] private float timeScale;
    private void Start()
    {
        var sequence = DOTween.Sequence()
            .Append(transform.DOLocalRotate(new Vector3(0, 0, -170), 2).SetEase(Ease.InOutCubic))
            .Append(transform.DOLocalRotate(new Vector3(0, 0, -10), 2).SetEase(Ease.InOutCubic))
            .SetLoops(-1);

        sequence.timeScale = timeScale;
    }
}
