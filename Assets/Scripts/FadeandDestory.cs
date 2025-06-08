using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FadeandDestory : MonoBehaviour
{
    [SerializeField] private float _delay = 0.1f;
    private void Start()
    {
        transform.GetComponent<SpriteRenderer>().DOFade(0, 1.2f).SetDelay(_delay).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

}
