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

        void Start()
        {
            missileLauncher = Instantiate(missileLauncherPrefab);
            missileLauncher.SetGameController(new MouseController());
        }
    }
}