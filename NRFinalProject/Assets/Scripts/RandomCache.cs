using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
class RandomCache : ComponentSystem
{
    public static int CacheSize = 10000;

    static Unity.Mathematics.Random rand = new Unity.Mathematics.Random(2515646);

    #region SubOne
    static List<float> SubOneFloats = new List<float>();
    static int SubOneIndex = 0;
    static object SubOneLock = new object();
    public static float GetSubOne()
    {
        float output = AccessCacheList(ref SubOneFloats, ref SubOneIndex);
        if (SubOneIndex + 1 >= SubOneFloats.Count) SubOneIndex = 0;
        return output;
    }
    static T AccessCacheList<T>(ref List<T> list, ref int index)
    {
        T item = list[index];
        index++;
        if (index + 1 >= list.Count) index = 0;
        return item;
    }

    #endregion SubOne


    #region Directions

    static List<float3> Directions = new List<float3>();
    static int DirectionIndex = 0;

    public static float3 GetDirection()
    {
        float3 output = AccessCacheList(ref Directions, ref DirectionIndex);
        return output;
    }

    #endregion Directions


    #region Angles

    static List<float> Angles = new List<float>();
    static int AngleIndex = 0;

    public static float GetAngle()
    {
        float output = AccessCacheList(ref Angles, ref AngleIndex);
        return output;
    }

    #endregion Angles


    #region LifeSpans

    static List<float> LifeSpans = new List<float>();
    static int LSIndex = 0;

    public static float GetLifeSpan()
    {
        float output = AccessCacheList(ref LifeSpans, ref LSIndex);
        return output;
    }

    #endregion LifeSpans

    #region CenterDists
    static List<float> CenterDists = new List<float>();
    static int CDIndex = 0;
    static float radius = 1f;
    public static float GetCenterDistance()
    {
        float output = AccessCacheList(ref CenterDists, ref CDIndex);
        return output;
    }

    #endregion CenterDists

    #region CubeTypes
    static List<uint> CubeTypes = new List<uint>();
    static int CubeIndex = 0;
    public static uint GetCubeType()
    {
        uint output = AccessCacheList(ref CubeTypes, ref CubeIndex);
        return output;
    }
    #endregion CubeTypes

    protected override void OnCreate()
    {
        for (int i = 0; i < CacheSize; i++)
        {
            SubOneFloats.Add(rand.NextFloat());
            Directions.Add(rand.NextFloat3(MoveRandom.DirectionMin, MoveRandom.DirectionMax));
            Angles.Add(rand.NextFloat(0f, Mathf.PI * 2));
            LifeSpans.Add(rand.NextFloat(1f, 15f));
            CenterDists.Add(rand.NextFloat(radius));
            CubeTypes.Add(rand.NextUInt((uint)SpawnerSystem.SpawnOdds));
        }
        base.OnCreate();
    }

    protected override void OnUpdate()
    {

    }
}
