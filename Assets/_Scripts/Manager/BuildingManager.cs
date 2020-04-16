using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    Building prefab;
    Transform[] buildingLocators;

    List<Building> buildings = new List<Building>();

    public BuildingManager(Building prefab, Transform[] buildingLocators)
    {
        this.prefab = prefab;
        this.buildingLocators = buildingLocators;

        Debug.Assert(this.prefab != null, "null building prefab");
        Debug.Assert(this.buildingLocators != null, "null buildingLocators");

        CreateBuildings();
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
            buildings.Add(building);
        }
    }
}
