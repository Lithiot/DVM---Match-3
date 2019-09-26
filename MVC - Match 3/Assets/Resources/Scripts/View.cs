using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public enum State
{
    Idle, Matching, Moving
}

public class View : MonoBehaviour
{
    private State state = State.Idle;

    [SerializeField] private Sprite redSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite yellowSprite;

    [SerializeField] private Controller controller;
    [SerializeField] private float responseTime;

    float holdingTime = 0.0f;

    private void Update()
    {
        switch (state)
        {
            case State.Idle:

                break;
            case State.Matching:

                break;
            case State.Moving:

                break;
        }

        if (Input.GetMouseButton(0))
        {
            holdingTime += Time.deltaTime;

            if (holdingTime >= responseTime)
            {
                DetectInput();
            }
        }
        else if (holdingTime > 0)
        {
            controller.TriggerMatch();
            holdingTime = 0.0f;
        }
        else
        {
            holdingTime = 0.0f;
        }
    }

    private void DetectInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
            if (hitInfo.collider.CompareTag("Tile"))
            {
                controller.CheckTiles(hitInfo);
            }
    }

    public void Draw(List<List<TileClass>> tileList, List<List<GameObject>> objList)
    {
        for (int y = 0; y < objList.Count; y++)
        {
            for (int x = 0; x < objList[y].Count; x++)
            {
                switch (tileList[y][x].Type)
                {
                    case Color.Red:
                        objList[y][x].GetComponent<SpriteRenderer>().sprite = redSprite;
                        break;
                    case Color.Blue:
                        objList[y][x].GetComponent<SpriteRenderer>().sprite = blueSprite;
                        break;
                    case Color.Green:
                        objList[y][x].GetComponent<SpriteRenderer>().sprite = greenSprite;
                        break;
                    case Color.Yellow:
                        objList[y][x].GetComponent<SpriteRenderer>().sprite = yellowSprite;
                        break;
                }
            }
        }
    }
}
