using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    [SerializeField] private Sprite redSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite yellowSprite;
    public Controller controller;

    public Text scoreText;

    private GameObject[,] tileObjects;
    public GameObject[,] TileObjects { get => tileObjects; set => tileObjects = value; }

    float holdingTime = 0.0f;

    private void Update()
    {
        if (controller.CurrentState == State.Matching)
            DetectInput();

        scoreText.text = "Score: " + controller.Points.ToString();
    }

    private void DetectInput()
    {
        if (Input.GetMouseButton(0))
        {
            holdingTime += Time.deltaTime;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                    if (hitInfo.collider.CompareTag("Tile"))
                    {
                        controller.CheckTiles(hitInfo);
                    }
        }
        else if (holdingTime > 0)
        {
            controller.TriggerMatch();
            holdingTime = 0.0f;
        }
    }

    public void Draw(Color[,] data, Vector2Int size) 
    {
        foreach (GameObject g in TileObjects)
        {
            g.SetActive(true);
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                switch (data[x, y])
                {
                    case Color.Null:
                        TileObjects[x, y].SetActive(false);
                        break;
                    case Color.Red:
                        TileObjects[x, y].GetComponent<SpriteRenderer>().sprite = redSprite;
                        break;
                    case Color.Blue:
                        TileObjects[x, y].GetComponent<SpriteRenderer>().sprite = blueSprite;
                        break;
                    case Color.Green:
                        TileObjects[x, y].GetComponent<SpriteRenderer>().sprite = greenSprite;
                        break;
                    case Color.Yellow:
                        TileObjects[x, y].GetComponent<SpriteRenderer>().sprite = yellowSprite;
                        break;
                }
            }
        }
    }
}
