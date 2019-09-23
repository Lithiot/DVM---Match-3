using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Red, Blue, Green, Yellow
}

public struct Tile
{
    public Vector2Int posInList;
    private Type type;

    public void ChangeType(Type type)
    {
        this.type = type;
    }

    public Tile(Vector2Int posInList, Type type)
    {
        this.posInList = posInList;
        this.type = type;
    }

    public Type Type { get => type; set => type = value; }
}

public class Model
{
    private List<List<Tile>> tileList;
    internal List<List<Tile>> TileList { get => tileList; set => tileList = value; }
}
