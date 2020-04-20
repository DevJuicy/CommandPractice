using UnityEngine;
using System.Collections;
using System;

namespace JUICY
{
    public class MouseController : MonoBehaviour, IController
    {
        public Action<Vector3> KeyPressed;
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                KeyPressed?.Invoke(GetInputPosition(Input.mousePosition));
            }
        }

        Vector3 GetInputPosition(Vector3 mousePosition)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(mousePosition);
            point.z = 0;
            return point;
        }
    }
}