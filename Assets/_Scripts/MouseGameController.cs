using UnityEngine;
using System.Collections;
using System;

public class MouseGameController : MonoBehaviour, IGameController
{
    public Action<Vector3> FireButtonPressed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireButtonPressed?.Invoke(GetCurrentClickPosition(Input.mousePosition));
        }
    }

    Vector3 GetCurrentClickPosition(Vector3 mousePosition)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(mousePosition);
        point.z = 0f;
        return point;
    }
}
