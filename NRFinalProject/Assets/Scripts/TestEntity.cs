using Unity.Entities;
using Unity.Mathematics;

public struct TestEntity : IComponentData
{
    public int CountX;
    public int CountY;
    public int CountZ;
    public Entity PrefabObject;
}
