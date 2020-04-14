using UnityEngine;
using System.Collections;
using System;

public class KeyGameController : MonoBehaviour, IGameController
{
    public Action FireButtonPressed;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireButtonPressed?.Invoke();
        }
    }
}
