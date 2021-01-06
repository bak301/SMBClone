using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Warning : MonoBehaviour
{
    [SerializeField] public float lifetime;

    private float timer;
    private SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        timer = lifetime;
        DOTween.Sequence()
            .Append(rend.DOFade(0.5f, 0.1f))
            .Append(rend.DOFade(1, 0.1f))
            .SetLoops(15);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            SimplePool.Despawn(gameObject);
        }
    }
}
