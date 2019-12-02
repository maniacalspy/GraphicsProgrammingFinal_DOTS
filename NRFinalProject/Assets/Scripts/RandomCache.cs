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

    #region SubOne
    static List<float> SubOneFloats = new List<float>();
    static int SubOneIndex = 0;

    public static float GetSubOne()
    {
        float CurFloat = SubOneIndex < SubOneFloats.Count ? 
            SubOneFloats[SubOneIndex] : 
            SubOneFloats[0];
        if (SubOneIndex + 1 >= SubOneFloats.Count) SubOneIndex = 0;
        else SubOneIndex++;
        return CurFloat;
    }

    public static int GetSubOneCount()
    {
        return SubOneFloats.Count;
    }

    public static void AddSubOne(float newFloat)
    {
        if (SubOneFloats.Count >= CacheSize)
        {
            try
            {
                if (SubOneIndex > 0) SubOneFloats[SubOneIndex - 1] = newFloat;
            }
            catch {
                SubOneFloats[0] = newFloat;
            }
        }
        else
        {
            try
            {
                SubOneFloats.Add(newFloat);
            }
            catch { }
        }
    }

    #endregion SubOne


    #region Directions

    static List<float3> Directions = new List<float3>();
    static int DirectionIndex = 0;

    public static float3 GetDirection()
    {
        float3 CurDirection = DirectionIndex < Directions.Count ? Directions[DirectionIndex] : CurDirection = Directions[0];

        if (DirectionIndex + 1 >= Directions.Count) DirectionIndex = 0;
        else DirectionIndex++;
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
        if (AngleIndex + 1 >= Angles.Count) AngleIndex = 0;
        else AngleIndex++;
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
        float CurLS = LSIndex < LifeSpans.Count ? LifeSpans[LSIndex] : LifeSpans[0];
        if (LSIndex + 1 >= LifeSpans.Count) LSIndex = 0;
        else LSIndex++;
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
        float CurCD  = CDIndex < CenterDists.Count ? CenterDists[CDIndex] : CenterDists[0];
        if (CDIndex + 1 >= CenterDists.Count) CDIndex = 0;
        else CDIndex++;
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
