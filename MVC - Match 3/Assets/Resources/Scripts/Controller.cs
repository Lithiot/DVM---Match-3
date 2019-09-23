using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    private Tile firstTile;
    private bool firstSelected = false;
    private Tile secondTile;
    private bool secondSelected = false;
    Vector3 originalFirstPos;
    Vector3 originalSecondPos;


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
                        tile = new Tile(new Vector2Int(y, x), Color.Red);
                        model.TileList[y].Add(tile);
                        break;
                    case 1:
                        tile = new Tile(new Vector2Int(y, x), Color.Blue);
                        model.TileList[y].Add(tile);
                        break;
                    case 2:
                        tile = new Tile(new Vector2Int(y, x), Color.Green);
                        model.TileList[y].Add(tile);
                        break;
                    case 3:
                        tile = new Tile(new Vector2Int(y, x), Color.Yellow);
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
                {
                    model.TileList[y][x].adjacents[0] = model.TileList[posToCheck.x][posToCheck.y];

                    if (model.TileList[posToCheck.x][posToCheck.y].Type == model.TileList[y][x].Type)
                        up = true;
                }

                // Checkeo tile inferior
                posToCheck = model.TileList[y][x].posInList + new Vector2Int(1, 0);
                if (IsInRange(posToCheck))
                {
                    model.TileList[y][x].adjacents[1] = model.TileList[posToCheck.x][posToCheck.y];

                    if (model.TileList[posToCheck.x][posToCheck.y].Type == model.TileList[y][x].Type)
                        down = true;
                }

                // Checkeo tile derecha
                posToCheck = model.TileList[y][x].posInList + new Vector2Int(0, 1);
                if (IsInRange(posToCheck))
                {
                    model.TileList[y][x].adjacents[2] = model.TileList[posToCheck.x][posToCheck.y];

                    if (model.TileList[posToCheck.x][posToCheck.y].Type == model.TileList[y][x].Type)
                        right = true;
                }

                // Checkeo tile izquierda
                posToCheck = model.TileList[y][x].posInList - new Vector2Int(0, 1);
                if (IsInRange(posToCheck))
                {
                    model.TileList[y][x].adjacents[3] = model.TileList[posToCheck.x][posToCheck.y];

                    if (model.TileList[posToCheck.x][posToCheck.y].Type == model.TileList[y][x].Type)
                        left = true;
                }

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

        if (tilesToChange.Count > 0)
        {
            for (int i = 0; i < tilesToChange.Count; i++)
            {
                Vector2Int aux = tilesToChange[i].posInList;
                model.TileList[aux.x][aux.y] = new Tile(tilesToChange[i].posInList, ChangeType(tilesToChange[i].Type));
            }
            tilesToChange.Clear();

            CheckForMatch();
        }
    }

    private Color ChangeType(Color type)
    {
        Color originalType = type;

        do
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    type = Color.Red;
                    break;
                case 1:
                    type = Color.Blue;
                    break;
                case 2:
                    type = Color.Green;
                    break;
                case 3:
                    type = Color.Yellow;
                    break;
            }
        } while (type == originalType);

        return type;
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
                if (hitInfo.collider.CompareTag("Tile"))
                {
                    CheckTiles(hitInfo);
                    if (firstSelected && secondSelected)
                        SwapTiles();
                }
        }

        view.Draw(model.TileList, prefabList);
    }

    private void SwapTiles()
    {
        GameObject first = prefabList[firstTile.posInList.x][firstTile.posInList.y];
        GameObject second = prefabList[secondTile.posInList.x][secondTile.posInList.y];

        originalFirstPos = first.transform.position;
        originalSecondPos = second.transform.position;

        StartCoroutine(LerpTiles(first, second));
    }

    IEnumerator LerpTiles(GameObject first, GameObject second)
    {
        bool lerpFinished = false;

        while (!lerpFinished)
        {
            first.transform.position = Vector3.Lerp(originalFirstPos, originalSecondPos, 1f);
            second.transform.position = Vector3.Lerp(originalSecondPos, originalFirstPos, 1f);

            if (first.transform.position == originalSecondPos && second.transform.position == originalFirstPos)
                lerpFinished = true;

            yield return null;
        }
        Tile aux;
        aux = firstTile;
        model.TileList[firstTile.posInList.x][firstTile.posInList.y] = model.TileList[secondTile.posInList.x][secondTile.posInList.y];
        model.TileList[secondTile.posInList.x][secondTile.posInList.y] = aux;

        firstSelected = false;
        secondSelected = false;

        first.transform.position = originalFirstPos;
        second.transform.position = originalSecondPos;

        CheckForMatch();
    }

    private void CheckTiles(RaycastHit hitInfo)
    {
        Vector2Int pos = new Vector2Int(-1, -1);

        for (int i = 0; i < prefabList.Count; i++)
        {
            if (prefabList[i].Contains(hitInfo.collider.gameObject))
            {
                pos = new Vector2Int(i, prefabList[i].IndexOf(hitInfo.collider.gameObject));
            }
        }

        if (IsInRange(pos))
        {
            if (!firstSelected)
            {
                firstTile = GetCorrespondantTile(pos);
                firstSelected = true;
            }
            else if (!secondSelected)
            {
                secondTile = GetCorrespondantTile(pos);
                secondSelected = true;
            }
        }
        else
            firstSelected = secondSelected = false;
    }

    private Tile GetCorrespondantTile(Vector2Int pos)
    {
            return model.TileList[pos.x][pos.y];
    }

    /*
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        for (int y = 0; y < model.TileList.Count; y++)
        {
            for (int x = 0; x < model.TileList[y].Count; x++)
            {
                Vector2Int aux = model.TileList[y][x].posInList;
                Handles.Label(prefabList[aux.x][aux.y].transform.position, aux.ToString());
            }
        }
#endif
    }
    */
}