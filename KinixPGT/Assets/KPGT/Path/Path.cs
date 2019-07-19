﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    //Propieties
    [SerializeField,HideInInspector]
    List<Vector2> points;
    EdgeCollider2D collider;

    //Floats
    private float offset;
    private float amplitude;

    public Path(Vector2 startPos)
    {
        points = new List<Vector2>
        {
            startPos,
        };
    }

    public Vector2 this[int i]
    {
        get
        {
            return points[i];
        }
    }

    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }

    public int NumSegments
    {
        get
        {
            return points.Count - 1;
        }
    }

    //
    // Summary:
    // Get the points of the path as a Vector2 Array.
    public Vector2[] GetPoints
    {
        get
        {
            return points.ToArray();
        }
    }

    public void AddPoint(Vector2 pos)
    {
        points.Add(pos);
    }

    public void RemovePoint(int pointIndex)
    {
        points.RemoveAt(pointIndex);
    }

    public void MovePoint(int i, Vector2 pos)
    {
        points[i] = pos;
    }

    public float vOffset
    {
        get
        {
            return offset;
        }
        set
        {
            offset = value;
        }
    }

    public float tAmplitude
    {
        get
        {
            return amplitude;
        }
        set
        {
            amplitude = value;
        }
    }

}
