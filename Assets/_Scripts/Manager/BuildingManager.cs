using System.Collections.Generic;
using UnityEngine;
using System;
public class BuildingManager
{
    Building prefab;
    Transform[] buildingLocators;
    Factory effectFactory;

    List<Building> buildings = new List<Building>();

    public Action AllBuildingsDestroyed;

    public int BuildingCount
    {
        get
        {
            return buildings.Count;
        }
    }

    public bool HasBuilding
    {
        get
        {
            return buildings.Count > 0;
        }
    }

    public BuildingManager(Building prefab, Transform[] buildingLocators, Factory effectFactory)
    {
        this.prefab = prefab;
        this.buildingLocators = buildingLocators;
        this.effectFactory = effectFactory;
    }

    public void OnGameStarted()
    {
        CreateBuildings();
    }

    public Vector3 GetRandomBuildingPosition()
    {
        Building building = buildings[UnityEngine.Random.Range(0, buildings.Count)];
        return building.transform.position;
    }

    void CreateBuildings()
    {
        if(buildings.Count > 0)
        {
            Debug.LogWarning("Buildings have been already created");
            return;
        }

        for(int i = 0;i<buildingLocators.Length;i++)
        {
            Building building = GameObject.Instantiate(prefab);
            building.transform.position = buildingLocators[i].position;
            building.Destroyed += OnBuildingDestryed;
            buildings.Add(building);
        }
    }

    void OnBuildingDestryed(Building building)
    {
        AudioManager.instance.PlaySound(SoundID.BuildingExplosion);

        RecycleObject effect = effectFactory.Get();
        effect.Activate(building.transform.position);
        effect.Destroyed += OnEffectDestroyed;

        building.Destroyed -= OnBuildingDestryed;
        int index = buildings.IndexOf(building);
        buildings.RemoveAt(index);
        GameObject.Destroy(building.gameObject);

        if(buildings.Count == 0)
        {
            AllBuildingsDestroyed?.Invoke();
        }
    }

    void OnEffectDestroyed(RecycleObject effect)
    {
        effect.Destroyed -= OnEffectDestroyed;
        effectFactory.Restore(effect);
    }
}
