using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color
{
    Red, Blue, Green, Yellow
}

public class TileClass
{
    private Vector2Int posInList;
    private List<TileClass> adjacents;
    private Color type;

    public TileClass(Vector2Int posInList, Color type)
    {
        this.posInList = posInList;
        this.type = type;
        adjacents = new List<TileClass>();
    }

    public Color Type { get => type; set => type = value; }
    public Vector2Int PosInList { get => posInList; set => posInList = value; }
    public List<TileClass> Adjacents { get => adjacents; set => adjacents = value; }
}
