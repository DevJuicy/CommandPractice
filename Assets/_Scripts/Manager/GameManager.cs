using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    BulletLauncher launcherPrefab;
    BulletLauncher launcher;

    [SerializeField]
    Transform launcherLocator;

    [SerializeField]
    Building buildingPrefab;

    [SerializeField]
    Transform[] buildingLocators;

    [SerializeField]
    Missile missilePrefab;

    [SerializeField]
    DestroyEffect effectPrefab;

    [SerializeField]
    int maxMissileCount = 20;

    [SerializeField]
    float missileSpawnInterval = 0.5f;

    MouseGameController mouseGameController;
    BuildingManager buildingManager;
    TimeManager timeManager;
    MissileManager missileManager;

    void Start()
    {
        launcher = Instantiate(launcherPrefab);
        launcher.transform.position = launcherLocator.position;

        mouseGameController = gameObject.AddComponent<MouseGameController>();

        buildingManager = new BuildingManager(buildingPrefab, buildingLocators, new Factory(effectPrefab));
        timeManager = gameObject.AddComponent<TimeManager>();
        missileManager = gameObject.AddComponent<MissileManager>();
        missileManager.Initialize(new Factory(missilePrefab), buildingManager, maxMissileCount, missileSpawnInterval);
        BindEvents();
        timeManager.StartGame(1f);
    }

    void OnDestroy()
    {
        UnBindEvents();
    }

    void BindEvents()
    {
        mouseGameController.FireButtonPressed += launcher.OnFireButtonPressed;
        timeManager.GameStarted += buildingManager.OnGameStarted;
        timeManager.GameStarted += launcher.OnGameStarted;
        timeManager.GameStarted += missileManager.OnGameStarted;
    }

    void UnBindEvents()
    {
        mouseGameController.FireButtonPressed -= launcher.OnFireButtonPressed;
        timeManager.GameStarted -= buildingManager.OnGameStarted;
        timeManager.GameStarted -= launcher.OnGameStarted;
        timeManager.GameStarted -= missileManager.OnGameStarted;
    }
}
