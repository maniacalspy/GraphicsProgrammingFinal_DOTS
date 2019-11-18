using System;
using Unity.Entities;

[Serializable]
public struct MoveUp : IComponentData
{
    public float LifeSpan;
    //This is a tag struct
    public MoveUp(float lifespan)
    {
        LifeSpan = lifespan;
    }
}
