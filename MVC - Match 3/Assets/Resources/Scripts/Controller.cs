using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Singleton
    // -----------------------------------------------------------------------------------------//

    public static Controller instance;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Variables
    // -----------------------------------------------------------------------------------------//

    [SerializeField] private Vector2Int size;
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private GameObject prefab;

    private Model model = new Model();
    [SerializeField] private View view;

    private List<List<GameObject>> prefabList = new List<List<GameObject>>();
    private List<Tile> tilesToChange = new List<Tile>();


    // Functions
    // -----------------------------------------------------------------------------------------//

    private void Start()
    {
        GeneratePrefabGrid();
        GenerateTiles();
        SendToDraw();
    }

    private void GenerateTiles()
    {
        Tile tile;
        model.TileList = new List<List<Tile>>();

        for (int y = 0; y < size.y; y++)
        {
            model.TileList.Add(new List<Tile>());

            for (int x = 0; x < size.x; x++)
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        tile = new Tile(new Vector2Int(y, x), Type.Red);
                        model.TileList[y].Add(tile);
                        break;
                    case 1:
                        tile = new Tile(new Vector2Int(y, x), Type.Blue);
                        model.TileList[y].Add(tile);
                        break;
                    case 2:
                        tile = new Tile(new Vector2Int(y, x), Type.Green);
                        model.TileList[y].Add(tile);
                        break;
                    case 3:
                        tile = new Tile(new Vector2Int(y, x), Type.Yellow);
                        model.TileList[y].Add(tile);
                        break;
                }
            }
        }

        CheckForMatch();
    }

    private void CheckForMatch()
    {
        Vector2Int posToCheck;

        for (int y = 0; y < model.TileList.Count; y++)
        {
            for (int x = 0; x < model.TileList[y].Count; x++)
            {
                bool up = false;
                bool down = false;
                bool left = false;
                bool right = false;

                // Checkeo tile superior
                posToCheck = model.TileList[y][x].posInList - new Vector2Int(1, 0);
                if (IsInRange(posToCheck))
                    if (model.TileList[y - 1][x].Type == model.TileList[y][x].Type)
                        up = true;

                // Checkeo tile inferior
                posToCheck = model.TileList[y][x].posInList + new Vector2Int(1, 0);
                if (IsInRange(posToCheck))
                    if (model.TileList[y + 1][x].Type == model.TileList[y][x].Type)
                        down = true;

                // Checkeo tile derecha
                posToCheck = model.TileList[y][x].posInList + new Vector2Int(0, 1);
                if (IsInRange(posToCheck))
                    if (model.TileList[y][x + 1].Type == model.TileList[y][x].Type)
                        right = true;

                // Checkeo tile izquierda
                posToCheck = model.TileList[y][x].posInList - new Vector2Int(0, 1);
                if (IsInRange(posToCheck))
                    if (model.TileList[y][x - 1].Type == model.TileList[y][x].Type)
                        left = true;

                if (right && left)
                {
                    if (!tilesToChange.Contains(model.TileList[y][x]))
                        tilesToChange.Add(model.TileList[y][x]);
                    
                    if (!tilesToChange.Contains(model.TileList[y][x + 1]))
                        tilesToChange.Add(model.TileList[y][x + 1]);

                    if (!tilesToChange.Contains(model.TileList[y][x - 1]))
                        tilesToChange.Add(model.TileList[y][x - 1]);
                    
                }
                else if (up && down)
                {
                    if (!tilesToChange.Contains(model.TileList[y][x]))
                        tilesToChange.Add(model.TileList[y][x]);
                    
                    if (!tilesToChange.Contains(model.TileList[y - 1][x]))
                        tilesToChange.Add(model.TileList[y - 1][x]);

                    if (!tilesToChange.Contains(model.TileList[y + 1][x]))
                        tilesToChange.Add(model.TileList[y + 1][x]);
                    
                }
            }
        }

        if(tilesToChange.Count > 0)
        {
            for (int i = 0; i < tilesToChange.Count; i++)
            {
                Type originalType = tilesToChange[i].Type;

                do
                {
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            tilesToChange[i].ChangeType(Type.Red);
                            break;
                        case 1:
                            tilesToChange[i].ChangeType(Type.Blue);
                            break;
                        case 2:
                            tilesToChange[i].ChangeType(Type.Green);
                            break;
                        case 3:
                            tilesToChange[i].ChangeType(Type.Yellow);
                            break;
                    }
                } while (tilesToChange[i].Type == originalType);
            }
            tilesToChange.Clear();

            CheckForMatch();
        }
    }

    private void ChangeType(Tile tile)
    {
        
    }

    private bool IsInRange(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < model.TileList.Count)
            if (pos.y >= 0 && pos.y < model.TileList[pos.x].Count)
                return true;

        return false;
    }

    private void SendToDraw()
    {
        view.Draw(model.TileList, prefabList);
    }

    public void GeneratePrefabGrid()
    {
        for (int y = 0; y < size.y; y++)
        {
            prefabList.Add(new List<GameObject>());

            for (int x = 0; x < size.x; x++)
            {
                float posX = startingPos.x + x;
                float posY = startingPos.y - y;
                Vector3 pos = new Vector3(posX, posY);
                GameObject obj = Instantiate(prefab, pos, Quaternion.identity, transform);
                prefabList[y].Add(obj);
            }
        }

        Debug.Log("prefabList is: " + prefabList.Count + " long");
    }

    public void ClearGrid()
    {
        prefabList.Clear();
        Debug.Log("prefabList is: " + prefabList.Count + " long");
    }
}
