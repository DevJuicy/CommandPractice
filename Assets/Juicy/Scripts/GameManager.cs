using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JUICY
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        MissileLauncher missileLauncherPrefab;
        MissileLauncher missileLauncher;

        MouseController mouseController;


        void Start()
        {
            missileLauncher = Instantiate(missileLauncherPrefab);
            mouseController = gameObject.AddComponent<MouseController>();
            BindEvents();
        }

        void OnDestroy()
        {
            UnBindEvents();
        }

        void BindEvents()
        {
            mouseController.KeyPressed += missileLauncher.Shoot;
        }

        void UnBindEvents()
        {
            mouseController.KeyPressed -= missileLauncher.Shoot;
        }
    }
}