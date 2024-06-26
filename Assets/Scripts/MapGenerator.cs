using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cakeCellPrefab;
    [SerializeField] private GameObject candyCellPrefab;
    [SerializeField] private GameObject giftCellPrefab;
    [SerializeField] private Transform grid;
    private string _fileName ;
    
    private void Awake()
    {
        _fileName = "map1";
        int[,] map = LoadMapFromFile(_fileName);
        if (map != null)
        {
            GenerateMap(map);
        }
    }

    int[,] LoadMapFromFile(string fileName)
    {
        TextAsset data = Resources.Load<TextAsset>(fileName);

        string[] lines = data.text.Split("\r\n");
        int rows = lines.Length-1;
        int columns = lines[0].Length;

        int[,] map = new int[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                map[i, j] = int.Parse(lines[i][j].ToString());
            }
        }

        return map;
    }

    void GenerateMap(int[,] map)
    {
        int rows = map.GetLength(0);
        int columns = map.GetLength(1);
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float posX =  ConstValue.SPACE_CELL * (j - 1);
                float posY = -ConstValue.SPACE_CELL * (i - 1);
                Vector2 pos = new Vector2(posX, posY);
                GameObject tile = null;
                switch (map[i, j])
                {
                    case 1:
                        tile = Instantiate(candyCellPrefab, pos, Quaternion.identity, grid);
                        break;
                    case 2:
                        tile = Instantiate(cakeCellPrefab, pos, Quaternion.identity, grid);
                        break;
                    case 3:
                        tile = Instantiate(giftCellPrefab, pos, Quaternion.identity, grid);
                        break;
                }

                if (tile != null)
                {
                    RectTransform rt = tile.GetComponent<RectTransform>();
                    rt.anchoredPosition = pos;
                }
                
            }
        }
    }

    public void GenerateMap(int level)
    {
        _fileName = "map" + level;
        int[,] map = LoadMapFromFile(_fileName);
        if (map != null)
        {
            GenerateMap(map);
        }
    }
}
