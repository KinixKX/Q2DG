﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathGenerator))]
public class PathEditor : Editor
{
    PathGenerator creator;
    Path path;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Reset points"))
        {
            Undo.RecordObject(creator, "Points reset");
            creator.GeneratePath();
            path = creator.path;
            creator.UpdateCollider();
            SceneView.RepaintAll();
        }

        if (GUILayout.Button(creator.GetComponent<EdgeCollider2D>() != null ? "Collider ON" : "Collider OFF"))
        {
            creator.ToggleCollider();
        }

        if(GUILayout.Button("Generate Terrain"))
        {
            Undo.RecordObject(creator, "Terrain Altered");
            creator.RandomizeTerrain();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Amplitude");
        creator.TAmplitude = EditorGUILayout.FloatField(creator.TAmplitude);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Details");
        creator.TFrequency = EditorGUILayout.IntField(creator.TFrequency);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Start/End Size");
        creator.VOffset = EditorGUILayout.FloatField(creator.VOffset);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Seed");
        creator.Seed = EditorGUILayout.IntField(creator.Seed);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Start Type");
        creator.StartTerrain = (PathGenerator.SEType)EditorGUILayout.EnumPopup(creator.StartTerrain);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("End Type");
        creator.EndTerrain = (PathGenerator.SEType)EditorGUILayout.EnumPopup(creator.EndTerrain);
        EditorGUILayout.EndHorizontal();

        creator.UpdateCollider();

        if (GUILayout.Button("Open Terrain Exporter"))
        {
            ExporterEditor.terrain.currentPath = creator;
            ExporterEditor.ShowExporter();
        }

    }

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(creator, "Add point");
            path.AddPoint(mousePos);
            creator.UpdateCollider();

        }
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1 && guiEvent.shift)
        {
            float minDstToPoint = 0.15f;
            int closestPointIndex = -1;

            for(int i = 0; i < path.NumPoints; i++)
            {
                float dist = Vector2.Distance(mousePos, path[i]);
                if (dist < minDstToPoint)
                {
                    minDstToPoint = dist;
                    closestPointIndex = i;
                }
            }
            if(closestPointIndex != -1)
            {
                Undo.RecordObject(creator, "Remove point");
                path.RemovePoint(closestPointIndex);
                creator.UpdateCollider();
            }
        }
    }

    void Draw()
    {
        Vector2[] points = path.GetPoints;
        for(int i = 0; i < path.NumSegments; i++)
        {
            Handles.DrawLine(points[i], points[i + 1]);
        }

        Handles.color = Color.cyan;
        for(int i = 0; i < path.NumPoints; i++)
        {
            Vector2 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, 0.1f, Vector2.one, Handles.CylinderHandleCap);
            if(path[i] != newPos)
            {
                Undo.RecordObject(creator, "Move point");
                path.MovePoint(i, newPos);
            }
        }
    }

    void OnEnable()
    {
        creator = (PathGenerator)target;
        if(creator.path == null)
        {
            creator.GeneratePath();
        }
        path = creator.path;
    }
}
