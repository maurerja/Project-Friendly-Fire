using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {

    public Transform wallPrefab;
	public Transform blankPrefab;
	public Transform roadPrefab;
	public Transform groundPrefab;
	public Transform doorPrefab;

	public TextAsset csv;


    public enum Tile : byte
    {
        Empty = 0,
		Blank,
        Wall,
        SlantUpRight,
        SlantUpLeft,
        SlantDownLeft,
        SlantDownRight,
		Road,
		Door,
		Ground
    };

    private List<List<Tile>> map;
    private int width = 40;
	private int height = 22;
    private const int CELL_SIZE = 48;
	private string[,] area = new string[40,22];

	// Use this for initialization
	void Start () {
		area = CSVReader.SplitCsvGrid (csv.text);
		CSVReader.DebugOutputGrid (area);
        generateMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void generateMap()
    {
        map = new List<List<Tile>>();
        for (int i = 0; i < height; i++) // row (y)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < width; j++) // column (x)
			{
				Tile t = Tile.Empty;
				Debug.Log(i+ " : " + j);
				switch(area[j,i])
				{
				case "0":
					t = Tile.Blank;
					Instantiate(blankPrefab, new Vector3(j, i, 10), Quaternion.Euler(new Vector3()));
					break;
				case "1":
					t = Tile.Wall;
					Instantiate(wallPrefab, new Vector3(j, i, 10), Quaternion.Euler(new Vector3()));
					break;
				case "2":
					t = Tile.Road;
					Instantiate(roadPrefab, new Vector3(j, i, 10), Quaternion.Euler(new Vector3()));
					break;
				case "5":
					t = Tile.Door;
					Instantiate(doorPrefab, new Vector3(j, i, 10), Quaternion.Euler(new Vector3()));
					break;
				case "6":
					t = Tile.Ground;
					Instantiate(groundPrefab, new Vector3(j, i, 10), Quaternion.Euler(new Vector3()));
					break;
				default:
					t = Tile.Blank;
					Instantiate(blankPrefab, new Vector3(j, i, 10), Quaternion.Euler(new Vector3()));
					break;

				}
				
					/*
                if (j == 0 || i == 0 || j == width - 1 || i == height - 1)
                {
                    t = Tile.Wall;
                    Instantiate(wallPrefab, new Vector3(j, i, 10), Quaternion.Euler(new Vector3()));
                }*/
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
