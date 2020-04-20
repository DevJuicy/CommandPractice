using UnityEngine;
using System.Collections;
//using System;

namespace JUICY
{
    public class MouseController : IController
    {
        public bool PressAttackKey()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}