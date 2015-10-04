using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {

    public Transform wallPrefab;

    public enum Tile : byte
    {
        Empty = 0,
        Wall,
        SlantUpRight,
        SlantUpLeft,
        SlantDownLeft,
        SlantDownRight
    };

    private List<List<Tile>> map;
    private int mapSize = 16;
    private const int CELL_SIZE = 48;

	// Use this for initialization
	void Start () {
        generateMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void generateMap()
    {
        map = new List<List<Tile>>();
        for (int i = 0; i < mapSize; i++) // row (y)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++) // column (x)
            {
                Tile t = Tile.Empty;
                if (j == 0 || i == 0 || j == mapSize - 1 || i == mapSize - 1)
                {
                    t = Tile.Wall;
                    Instantiate(wallPrefab, new Vector3(j, i, 10), Quaternion.Euler(new Vector3()));
                }
                row.Add(t);
            }
            map.Add(row);
        }
    }

    public Tile getTile(float x, float y)
    {
        int row = (int)Mathf.Floor(y);
        int col = (int)Mathf.Floor(x);
        Tile t = map[row][col];
        float xx = x - col;
        float yy = y - row;
        switch (t)
        {
            case Tile.SlantUpRight:
                if (yy <= (1 - xx))
                {
                    t = Tile.SlantUpRight;
                }
                break;
            case Tile.SlantUpLeft:
                if (yy <= xx) 
                {
                    t = Tile.SlantUpLeft;
                }
                break;
            case Tile.SlantDownLeft:
                if (yy >= (1 - xx))
                {
                    t = Tile.SlantDownLeft;
                }
                break;
            case Tile.SlantDownRight:
                if (yy >= xx)
                {
                    t = Tile.SlantDownRight;
                }
                break;
            default:
                break;
        }
        return t;
    }
}
