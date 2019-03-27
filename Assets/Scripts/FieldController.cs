﻿using System;
using System.Collections.Generic;
using GravityDJ;
using UnityEngine;
using Zenject;

public class FieldController : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public int fieldSize = 12;
        public float cellSize = 1f;
        public List<Vector2Int>  holeCoordList = new List<Vector2Int>();
        public Vector2 fieldCenter = new Vector2(0,0);
    }


    public float BorderSize => CellSize;


    public int FieldSize//todo inline
    {
        get { return settings.fieldSize; }
        set { settings.fieldSize = value; }
    }

    public float CellSize
    {
        get { return settings.cellSize; }
        set { settings.cellSize = value; }
    }

    private GameObject[,] field;

    private Settings settings;
    private Boundary.Factory boundaryFactory;

    public GameObject this[Vector2Int coord]
    {
        get { return field[coord.x, coord.y]; }
        set { field[coord.x, coord.y] = value; }      
    }

    [Inject]
    void Init(Settings settings, Boundary.Factory boundaryFactory)
    {
        this.settings = settings;
        this.boundaryFactory = boundaryFactory;
    }
    
    void Awake()
    {
        field = new GameObject[FieldSize,FieldSize];
        for (int x = 0, y = 0; x < FieldSize && y < FieldSize; x++, y++)
        {
            SpawnBorderTile(x, FieldSize - 1);
            SpawnBorderTile(x, 0);
            SpawnBorderTile(0, y);
            SpawnBorderTile(FieldSize-1, y);
        }

        foreach (var holeCoord in settings.holeCoordList)
        {
            var borderTile = this[holeCoord];
            if (borderTile != null)
            {
                Destroy(borderTile);   
            }
        }
    }

    private void SpawnBorderTile(int x, int y)
    {
        var boundary = boundaryFactory.Create();
        boundary.transform.position = GetPosByFieldCoordinates(x, y);
        
        field[x,y] = boundary.gameObject;
    }

    private Vector2 GetPosByFieldCoordinates(int x, int y)
    {
        return GetBottomLeftCoord() + Vector2.up *(y * CellSize) + Vector2.right*(x * CellSize);
    }

    private Vector2 GetBottomLeftCoord()
    {
        return settings.fieldCenter + (Vector2.down  + Vector2.left)*(FieldSize/2 - CellSize/2);
    }
}
