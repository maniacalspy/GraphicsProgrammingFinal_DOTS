using Unity.Entities;

public struct TestEntity : IComponentData
{
    public int CountX;
    public int CountY;
    public Entity PrefabObject;
}
