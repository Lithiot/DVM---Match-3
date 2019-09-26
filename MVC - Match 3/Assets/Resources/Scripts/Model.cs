using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model
{
    private List<List<TileClass>> tileList;
    internal List<List<TileClass>> TileList { get => tileList; set => tileList = value; }
}
