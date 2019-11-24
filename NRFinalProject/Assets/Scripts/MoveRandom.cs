using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct MoveRandom : IComponentData
{
    public static float DirectionMin = .25f;
    public static float DirectionMax = 1.01f;
    public static float LifeSpanMax = 15f;
    public float LifeSpan;
    public float3 Direction;
    //This is a tag struct
    public MoveRandom(float lifespan) : this(lifespan, new float3(0,.7f,.7f))
    {
    }

    public MoveRandom(float lifespan, float3 movedirection)
    {
        LifeSpan = lifespan;
        Direction = movedirection;
    }
}
