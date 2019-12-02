using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct RandomCache
{
    public static int CacheSize = 10000;

    #region positions
    static List<float3> Positions = new List<float3>();
    static int PositionIndex = 0;

    public static float3 GetPosition()
    {
        float3 CurPosition = PositionIndex < Positions.Count ? Positions[PositionIndex] : Positions[0]; ;
        PositionIndex++;
        if (PositionIndex >= Positions.Count) PositionIndex = 0;
        return CurPosition;
    }

    public static int GetPositionCount()
    {
        return Positions.Count;
    }

    public static void AddPosition(float3 newPosition)
    {
        if (Positions.Count >= CacheSize)
        {
            if (PositionIndex > 0) Positions[PositionIndex - 1] = newPosition;
            else Positions[0] = newPosition;
        }
        else
        {
            Positions.Add(newPosition);
        }
    }
    #endregion positions


    #region Directions

    static List<float3> Directions = new List<float3>();
    static int DirectionIndex = 0;

    public static float3 GetDirection()
    {
        float3 CurDirection = DirectionIndex < Directions.Count ? Directions[DirectionIndex] : Directions[0];
        DirectionIndex++;
        if (DirectionIndex >= Directions.Count) DirectionIndex = 0;
        return CurDirection;
    }

    public static int GetDirectionCount()
    {
        return Directions.Count;
    }

    public static void AddDirection(float3 newDirection)
    {
        if (Directions.Count >= CacheSize)
        {
            if (DirectionIndex > 0) Directions[DirectionIndex - 1] = newDirection;
            else Directions[0] = newDirection;
        }
        else
        {
            Directions.Add(newDirection);
        }
    }

    #endregion Directions


    #region Angles

    static List<float> Angles = new List<float>();
    static int AngleIndex = 0;

    public static float GetAngle()
    {
        float CurAngle = AngleIndex < Angles.Count ? Angles[AngleIndex] : Angles[0];
        AngleIndex++;
        if (AngleIndex >= Angles.Count) AngleIndex = 0;
        return CurAngle;
    }

    public static int GetAngleCount()
    {
        return Angles.Count;
    }

    public static void AddAngle(float newAngle)
    {
        if (Angles.Count >= CacheSize)
        {
            if (AngleIndex > 0) Angles[AngleIndex - 1] = newAngle;
            else Angles[0] = newAngle;
        }
        else
        {
            Angles.Add(newAngle);
        }
    }

    #endregion Angles


    #region LifeSpans

    static List<float> LifeSpans = new List<float>();
    static int LSIndex = 0;

    public static float GetLifeSpan()
    {
        float CurLS = LSIndex < LifeSpans.Count ?  LifeSpans[LSIndex] : LifeSpans[0];
        LSIndex++;
        if (LSIndex >= LifeSpans.Count) LSIndex = 0;
        return CurLS;
    }

    public static int GetLifeSpanCount()
    {
        return LifeSpans.Count;
    }

    public static void AddLifeSpan(float newLS)
    {
        if (LifeSpans.Count >= CacheSize)
        {
            if (LSIndex > 0) LifeSpans[LSIndex - 1] = newLS;
            else LifeSpans[0] = newLS;
        }
        else
        {
            LifeSpans.Add(newLS);
        }
    }

    #endregion LifeSpans

    #region CenterDists
    static List<float> CenterDists = new List<float>();
    static int CDIndex = 0;

    public static float GetCenterDistance()
    {
        float CurCD = CDIndex < CenterDists.Count ?  CenterDists[CDIndex] : CenterDists[0];
        CDIndex++;
        if (CDIndex >= CenterDists.Count) CDIndex = 0;
        return CurCD;
    }

    public static int GetCenterDistsCount()
    {
        return CenterDists.Count;
    }

    public static void AddCenterDist(float newCD)
    {
        if (CenterDists.Count >= CacheSize)
        {
            if (CDIndex > 0) CenterDists[LSIndex - 1] = newCD;
            else CenterDists[0] = newCD;
        }
        else
        {
            CenterDists.Add(newCD);
        }
    }

    #endregion CenterDists

}
