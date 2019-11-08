using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance;
    public static int horizontalTilesQty = 22;
    public static int verticalTilesQty = 11;
    public GameObject[] tilePrefabs;
    public Vector3[,] positionMatrix;
    public int[,] indexMatrix;
    public Vector3 spawnPointLocation;
    public Vector3 homeBaseLocation;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    public float TileSize {
        get {
            return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }


    // Start is called before the first frame update
    void Start() {
        positionMatrix = new Vector3[verticalTilesQty, horizontalTilesQty];
        indexMatrix = new int[verticalTilesQty, horizontalTilesQty];
        CreateLevel();
        //GameObject.Find("Map").transform.Translate(new Vector3(-TileSize/2, TileSize/2));
    }

    // Update is called once per frame
    void Update() {

    }

    // 0 - Ground
    // 1 - Enemy Path
    // 2 - Path walked by an enemy (will only be modified on their own matrix)
    // 3 - Turrets (changed by scripts)
    // 4 - Spawn point
    // 5 - Home Base location
    // 9 - Map Limit / Outer Wall
    private void CreateLevel() {
        string[] mapData = new string[]
            {
                "9999999999999999999999",
                "9000000000000000005009",
                "9410001111100000001009",
                "9010001000100111111009",
                "9010001000100100000009",
                "9010001000100111111009",
                "9010001000100000001009",
                "9010001000100000001009",
                "9011111000111111111009",
                "9000000000000000000009",
                "9999999999999999999999"
            };

        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        int tileIndex;
        for (int y = 0; y < mapYSize; y++) {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapXSize; x++) {
                tileIndex = (int) char.GetNumericValue(newTiles[x]);

                // Spawn point
                if (tileIndex == 4) {
                    tileIndex = 1;
                    spawnPointLocation = new Vector3(worldStart.x + TileSize * x - TileSize / 2, worldStart.y - TileSize * y + TileSize / 2, 0);
                }

                // Home Base
                if (tileIndex == 5) {
                    tileIndex = 1;
                    homeBaseLocation = new Vector3(worldStart.x + TileSize * x - TileSize / 2, worldStart.y - TileSize * y + TileSize / 2, 0);
                }

                indexMatrix[y, x] = tileIndex;

                if (tileIndex == 9) {
                    tileIndex = 2;
                }

                PlaceTile(tileIndex, x, y, worldStart);
            }
        }
    }

    private void PlaceTile(int tileIndex, int x, int y, Vector3 worldStart) {
        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
        newTile.transform.SetParent(GameObject.Find("Map").transform);
        newTile.transform.position = new Vector3(worldStart.x + TileSize * x - TileSize / 2, worldStart.y - TileSize * y + TileSize / 2, 0);
        positionMatrix[y, x] = newTile.transform.position;
    }

    int GetCornerType(int x, int y) {
        int ht = horizontalTilesQty;
        int vt = verticalTilesQty;
        if (y < vt - 1 && indexMatrix[y + 1, x] == 1 && x < ht - 1 && indexMatrix[y, x + 1] == 1) {
            return 0;
        } else if (y > 0 && indexMatrix[y - 1, x] == 1 && x < ht - 1 && indexMatrix[y, x + 1] == 1) {
            return 1;
        } else if (y < vt - 1 && indexMatrix[y + 1, x] == 1 && x > 0 && indexMatrix[y, x - 1] == 1) {
            return 2;
        } else if (y > 0 && indexMatrix[y - 1, x] == 1 && x > 0 && indexMatrix[y, x - 1] == 1) {
            return 3;
        }
        return -1;
    }

    int GetPathType(int x, int y) {
        if ((y > 0 && indexMatrix[y - 1, x] == 1) || (y < verticalTilesQty - 1 && indexMatrix[y + 1, x] == 1)) {
            return 0;
        } else if ((x > 0 && indexMatrix[y, x - 1] == 1) || (x < horizontalTilesQty - 1 && indexMatrix[y, x + 1] == 1)) {
            return 1;
        }
        return -1;
    }

    public int[] GetCurrentTile(Vector3 position) {
        float shortestDistance = Mathf.Infinity;
        int[] retval = { 0, 0 };
        for (int i = 0; i < positionMatrix.GetLength(0); i++) {
            for (int j = 0; j < positionMatrix.GetLength(1); j++) {
                float distance = Vector3.Distance(position, positionMatrix[i, j]);
                if (distance < shortestDistance) {
                    shortestDistance = distance;
                    retval = new int[] { j, i };
                }
            }
        }
        return retval;
    }
}