using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    Matching, Resolving, Paused
}

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector2Int size;
    [SerializeField] private Vector3 startingPos;

    private int points = 0;
    public int Points { get => points; set => points = value; }

    [SerializeField] private View view;
    private Model model;

    private List<Vector2Int> tilesToChange = new List<Vector2Int>();

    private State currentState = State.Matching;
    internal State CurrentState { get => currentState; set => currentState = value; }

    private void Awake()
    {
        model = new Model();
        CreateGrids();
    }

    private void Update()
    {
        if (CurrentState == State.Resolving)
            ResolveMatch();
    }

    private void ResolveMatch()
    {
        int changes = 0;

        do
        {
            changes = 0;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (model.TileData[x, y] == Color.Null)
                    {
                        if (y - 1 >= 0)
                        {
                            if (model.TileData[x, y - 1] != Color.Null)
                            {
                                model.TileData[x, y] = model.TileData[x, y - 1];
                                model.TileData[x, y - 1] = Color.Null;
                                changes += 1;
                            }
                        }
                    }
                }
            }

        } while (changes != 0);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (model.TileData[x, y] == Color.Null)
                    model.TileData[x, y] = RandomColor();
            }
        }

        SendToDraw();
        currentState = State.Matching;
    }

    public void CheckTiles(RaycastHit hitInfo)
    {
        Vector2Int pos = new Vector2Int(-1, -1);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (view.TileObjects[x, y] == hitInfo.collider.gameObject)
                    pos = new Vector2Int(x, y);
            }
        }

        if (tilesToChange.Count > 0)
        {
            if (pos != tilesToChange[tilesToChange.Count - 1])
            {
                if (isAdjacent(tilesToChange[tilesToChange.Count - 1], pos))
                {
                    if (model.TileData[pos.x, pos.y] == model.TileData[tilesToChange[tilesToChange.Count - 1].x, tilesToChange[tilesToChange.Count - 1].y])
                        if (!tilesToChange.Contains(pos))
                        {
                            tilesToChange.Add(pos);
                        }
                        else
                        {
                            for (int i = tilesToChange.IndexOf(pos) + 1; i < tilesToChange.Count; i++)
                            {
                                tilesToChange.RemoveAt(i);
                            }
                        }
                }

            }
        }
        else
            tilesToChange.Add(pos);
    }

    public void TriggerMatch() 
    {
        if (tilesToChange.Count >= 3)
        {
            foreach (Vector2Int pos in tilesToChange)
            {
                model.TileData[pos.x, pos.y] = Color.Null;
            }

            if (points <= 0)
                GooglePlayServicesController.TriggerArchievement(GPGSIds.achievement_your_first_match);

            int aux = tilesToChange.Count - 3;
            points += 1 + aux;
        }


        tilesToChange.Clear();
        CurrentState = State.Resolving;
        SendToDraw();
    }

    private bool isAdjacent(Vector2Int first, Vector2Int second)
    {
        Vector2Int aux = second - first;

        if (aux == Vector2Int.up)
            return true;
        else if (aux == Vector2Int.left)
            return true;
        else if (aux == Vector2Int.right)
            return true;
        else if (aux == Vector2Int.down)
            return true;
        else
            return false;
    }

    private void CreateGrids()
    {
        view.TileObjects = new GameObject[size.x, size.y];
        model.TileData = new Color[size.x, size.y];

        Vector3 pos;

        for (int y = 0; y < size.x; y++)
        {
            for (int x = 0; x < size.y; x++)
            {
                pos = startingPos + new Vector3(1 * y, -1 * x, 0);

                view.TileObjects[y, x] = InstantiatePrefab(pos);
                model.TileData[y, x] = RandomColor();
            }
        }

        CheckMatches(false);
        SendToDraw();
    }

    private void SendToDraw()
    {
        view.Draw(model.TileData, size);
    }

    private Color RandomColor()
    {
        int aux = Random.Range(1, 5);
        Debug.Log($"aux is {aux}");
        return (Color)aux;
    }

    private GameObject InstantiatePrefab(Vector3 position)
    {
        GameObject obj = Instantiate(tilePrefab, position, Quaternion.identity, transform);
        return obj;
    }

    private void CheckMatches(bool addpoints)
    {
        for (int x = 1; x < size.x; x++)
        {
            for (int y = 1; y < size.y; y++)
            {
                Vector2Int tileToCheck = new Vector2Int(x, y);
                Vector2Int left = tileToCheck + Vector2Int.left;
                Vector2Int right = tileToCheck + Vector2Int.right;

                if (model.TileData[tileToCheck.x, tileToCheck.y] == Color.Null)
                    break;


                if (left.x >= 0 && model.TileData[left.x, left.y] == model.TileData[tileToCheck.x, tileToCheck.y])
                {
                    if (right.x < size.x && model.TileData[right.x, right.y] == model.TileData[tileToCheck.x, tileToCheck.y])
                    {
                        if (!tilesToChange.Contains(left))
                            tilesToChange.Add(left);
                        if (!tilesToChange.Contains(tileToCheck))
                            tilesToChange.Add(tileToCheck);
                        if (!tilesToChange.Contains(right))
                            tilesToChange.Add(right);
                    }
                }

                Vector2Int up = tileToCheck + Vector2Int.down;
                Vector2Int down = tileToCheck + Vector2Int.up;
                if (up.y >= 0 && model.TileData[up.x, up.y] == model.TileData[tileToCheck.x, tileToCheck.y])
                {
                    if (down.y < size.y && model.TileData[down.x, down.y] == model.TileData[tileToCheck.x, tileToCheck.y])
                    {
                        if (!tilesToChange.Contains(up))
                            tilesToChange.Add(up);
                        if (!tilesToChange.Contains(tileToCheck))
                            tilesToChange.Add(tileToCheck);
                        if (!tilesToChange.Contains(down))
                            tilesToChange.Add(down);
                    }
                }
            }
        }

        if (tilesToChange.Count > 0)
        {
            foreach (Vector2Int tile in tilesToChange)
            {
                    model.TileData[tile.x, tile.y] = RandomColor();
            }

            tilesToChange.Clear();
            CheckMatches(false);
        }
    }

    public void EndGame()
    {
        GooglePlayServicesController.AddScoreToBoard(GPGSIds.leaderboard_highscore, Points);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;

        foreach (Vector2Int pos in tilesToChange)
        {
            Gizmos.DrawWireCube(view.TileObjects[pos.x, pos.y].transform.position, Vector3.one);
        }
    }
}
