using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FloorTilingEditor : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private GameObject floorPrefab = null;

    private List<GameObject> currentTiles = new List<GameObject>();

    public int tilesRows, tilesColumns;

    private int savedTilesRow, savedTilesColumn;

    private void Update()
    {
        if (!Application.isEditor)
            return;
        if(tilesColumns!=savedTilesColumn || tilesRows != savedTilesRow)
        {
            savedTilesColumn = tilesColumns;
            savedTilesRow = tilesRows;
            LoadTiles();
        }
    }

    private void LoadTiles()
    {
        DestroyTiles();
        for(int i = 0; i < tilesColumns; ++i)
        {
            for(int j = 0; j < tilesRows; ++j)
            {
                GameObject curTile = Instantiate(floorPrefab, transform);
                curTile.transform.localPosition = new Vector3(i, j, 0f);
                currentTiles.Add(curTile);
            }
        }
    }

    private void DestroyTiles()
    {
        foreach(var tile in currentTiles)
        {
            SafeDestroy(tile);
        }
        currentTiles.Clear();
    }

    public static T SafeDestroy<T>(T obj) where T : Object
    {
        if (Application.isEditor)
            Object.DestroyImmediate(obj);
        else
            Object.Destroy(obj);

        return null;
    }
}
