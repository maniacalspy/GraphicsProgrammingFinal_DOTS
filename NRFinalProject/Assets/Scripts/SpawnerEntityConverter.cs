using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnerEntityConverter : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject Prefab;
    public int countX;
    public int countY;
    public int countZ;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(Prefab);
    }


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var SpawnerData = new TestEntity
        {
            PrefabObject = conversionSystem.GetPrimaryEntity(Prefab),
            CountX = countX,
            CountY = countY,
            CountZ = countZ
        };

        dstManager.AddComponentData(entity, SpawnerData);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
