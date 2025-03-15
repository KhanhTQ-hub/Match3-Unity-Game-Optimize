using System.Collections;
using System.Collections.Generic;
using com.ktgame.core;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameObjectInjecter : MonoBehaviour
{
    private bool _isResolved = false;

    private void Awake()
    {
        Resolve();
    }

    private void Resolve()
    {
        if (!_isResolved)
        {
            _isResolved = true;
            var monoBehaviours = GetComponentsInChildren<MonoBehaviour>(true);
            for (var i = 0; i < monoBehaviours.Length; i++)
            {
                var monoBehaviour = monoBehaviours[i];
                if (monoBehaviour != null)
                {
                    Game.Instance.Injector.Resolve(monoBehaviour);
                }
            }
        }
    }
}
