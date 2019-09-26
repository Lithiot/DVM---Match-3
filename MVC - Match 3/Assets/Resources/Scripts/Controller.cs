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
    private List<TileClass> tilesToChange = new List<TileClass>();

    private TileClass firstTile;
    private bool firstSelected = false;
    private TileClass secondTile;
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
        TileClass tile;
        model.TileList = new List<List<TileClass>>();

        for (int y = 0; y < size.y; y++)
        {
            model.TileList.Add(new List<TileClass>());

            for (int x = 0; x < size.x; x++)
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        tile = new TileClass(new Vector2Int(y, x), Color.Red);
                        model.TileList[y].Add(tile);
                        break;
                    case 1:
                        tile = new TileClass(new Vector2Int(y, x), Color.Blue);
                        model.TileList[y].Add(tile);
                        break;
                    case 2:
                        tile = new TileClass(new Vector2Int(y, x), Color.Green);
                        model.TileList[y].Add(tile);
                        break;
                    case 3:
                        tile = new TileClass(new Vector2Int(y, x), Color.Yellow);
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
                posToCheck = model.TileList[y][x].PosInList - new Vector2Int(1, 0);
                if (IsInRange(posToCheck))
                {
                    if (!model.TileList[y][x].Adjacents.Contains(model.TileList[posToCheck.x][posToCheck.y]))
                        model.TileList[y][x].Adjacents.Add(model.TileList[posToCheck.x][posToCheck.y]);

                    if (model.TileList[posToCheck.x][posToCheck.y].Type == model.TileList[y][x].Type)
                        up = true;
                }

                // Checkeo tile inferior
                posToCheck = model.TileList[y][x].PosInList + new Vector2Int(1, 0);
                if (IsInRange(posToCheck))
                {
                    if (!model.TileList[y][x].Adjacents.Contains(model.TileList[posToCheck.x][posToCheck.y]))
                        model.TileList[y][x].Adjacents.Add(model.TileList[posToCheck.x][posToCheck.y]);

                    if (model.TileList[posToCheck.x][posToCheck.y].Type == model.TileList[y][x].Type)
                        down = true;
                }

                // Checkeo tile derecha
                posToCheck = model.TileList[y][x].PosInList + new Vector2Int(0, 1);
                if (IsInRange(posToCheck))
                {
                    if(!model.TileList[y][x].Adjacents.Contains(model.TileList[posToCheck.x][posToCheck.y]))
                        model.TileList[y][x].Adjacents.Add(model.TileList[posToCheck.x][posToCheck.y]);

                    if (model.TileList[posToCheck.x][posToCheck.y].Type == model.TileList[y][x].Type)
                        right = true;
                }

                // Checkeo tile izquierda
                posToCheck = model.TileList[y][x].PosInList - new Vector2Int(0, 1);
                if (IsInRange(posToCheck))
                {
                    if (!model.TileList[y][x].Adjacents.Contains(model.TileList[posToCheck.x][posToCheck.y]))
                        model.TileList[y][x].Adjacents.Add(model.TileList[posToCheck.x][posToCheck.y]);

                    if (model.TileList[posToCheck.x][posToCheck.y].Type == model.TileList[y][x].Type)
                        left = true;
                }
                /*
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
                */
            }
        }

        if (tilesToChange.Count > 0)
        {
            for (int i = 0; i < tilesToChange.Count; i++)
            {
                tilesToChange[i].Type = ChangeType(tilesToChange[i].Type);
            }
            tilesToChange.Clear();

            CheckForMatch();
        }
    }

    public void TriggerMatch()
    {
        if (tilesToChange.Count > 0)
        {
            for (int i = 0; i < tilesToChange.Count; i++)
            {
                tilesToChange[i].Type = ChangeType(tilesToChange[i].Type);
            }
            tilesToChange.Clear();
            view.Draw(model.TileList, prefabList);
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

    public bool CheckTiles(RaycastHit hitInfo)
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
            TileClass aux = GetCorrespondantTile(pos);

            if (tilesToChange.Count > 0)
            {
                if(tilesToChange[tilesToChange.Count - 1].Adjacents.Contains(aux))
                    if (tilesToChange[tilesToChange.Count - 1].Type == GetCorrespondantTile(pos).Type)
                        if(!tilesToChange.Contains(GetCorrespondantTile(pos)))
                            tilesToChange.Add(GetCorrespondantTile(pos));
            }
            else
            {
                tilesToChange.Add(GetCorrespondantTile(pos));
            }
        }
        return false;
    }

    
    private TileClass GetCorrespondantTile(Vector2Int pos)
    {
            return model.TileList[pos.x][pos.y];
    }
    
    private void OnDrawGizmos()
    {
        foreach (TileClass g in tilesToChange)
        {
            Gizmos.color = UnityEngine.Color.white;
            Gizmos.DrawWireCube(prefabList[g.PosInList.x][g.PosInList.y].transform.position, Vector3.one);
        }


        /*
#if UNITY_EDITOR
        for (int y = 0; y < model.TileList.Count; y++)
        {
            for (int x = 0; x < model.TileList[y].Count; x++)
            {
                Vector2Int aux = model.TileList[y][x].PosInList;
                Handles.Label(prefabList[aux.x][aux.y].transform.position, aux.ToString());
            }
        }
#endif
*/
    }
}















/*
    private void SwapTiles()
    {
        GameObject first = prefabList[firstTile.PosInList.x][firstTile.PosInList.y];
        GameObject second = prefabList[secondTile.PosInList.x][secondTile.PosInList.y];

        originalFirstPos = first.transform.position;
        originalSecondPos = second.transform.position;

        StartCoroutine(LerpTiles(first, second));
    }

    IEnumerator LerpTiles(GameObject first, GameObject second)
    {
        bool lerpFinished = false;
        float lerp = 0;

        while (!lerpFinished)
        {
            lerp += Time.deltaTime;

            first.transform.position = Vector3.Lerp(originalFirstPos, originalSecondPos, lerp);
            second.transform.position = Vector3.Lerp(originalSecondPos, originalFirstPos, lerp);

            if (first.transform.position == originalSecondPos && second.transform.position == originalFirstPos)
                lerpFinished = true;

            yield return null;
        }

        Color aux = firstTile.Type;

        firstTile.Type = secondTile.Type;
        secondTile.Type = aux;

        firstSelected = false;
        secondSelected = false;

        first.transform.position = originalFirstPos;
        second.transform.position = originalSecondPos;

        CheckForMatch();
    }
    */
