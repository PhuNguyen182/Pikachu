using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    [SerializeField] private float duration;

    private float _elapsedTime = 0;

    private void OnEnable()
    {
        _elapsedTime = 0;
    }

    private void Update()
    {
        if (_elapsedTime <= duration)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > duration)
            {
                SimplePool.Despawn(this.gameObject);
            }
        }
    }
}
