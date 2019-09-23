using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color
{
    Red, Blue, Green, Yellow
}

public struct Tile
{
    public Vector2Int posInList;
    public Tile[] adjacents;
    private Color type;

    public void ChangeType(Color type)
    {
        this.type = type;
    }

    public Tile(Vector2Int posInList, Color type)
    {
        this.posInList = posInList;
        this.type = type;
        adjacents = new Tile[4];
    }

    public Color Type { get => type; set => type = value; }
}

public class Model
{
    private List<List<Tile>> tileList;
    internal List<List<Tile>> TileList { get => tileList; set => tileList = value; }
}
