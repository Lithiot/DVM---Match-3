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


    public void Draw(List<List<Tile>> tileList, List<List<GameObject>> objList)
    {
        for (int y = 0; y < objList.Count; y++)
        {
            for (int x = 0; x < objList[y].Count; x++)
            {
                switch (tileList[y][x].Type)
                {
                    case Type.Red:
                        objList[y][x].GetComponent<SpriteRenderer>().sprite = redSprite;
                        break;
                    case Type.Blue:
                        objList[y][x].GetComponent<SpriteRenderer>().sprite = blueSprite;
                        break;
                    case Type.Green:
                        objList[y][x].GetComponent<SpriteRenderer>().sprite = greenSprite;
                        break;
                    case Type.Yellow:
                        objList[y][x].GetComponent<SpriteRenderer>().sprite = yellowSprite;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
