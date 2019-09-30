using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color
{
    Null, Red, Blue, Green, Yellow
}

public class Model
{
    private Color[,] tileData;
    public Color[,] TileData { get => tileData; set => tileData = value; }
}
