using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoadSelectLevel : MonoBehaviour
{
    [SerializeField] private GameObject btnLevel0StarPrefab;
    [SerializeField] private GameObject btnLevel1StarPrefab;
    [SerializeField] private GameObject btnLevel2StarPrefab;
    [SerializeField] private GameObject btnLevel3StarPrefab;
    [SerializeField] private GameObject btnLevelLockPrefab;
    [SerializeField] private Transform grid;
    [SerializeField] private ButtonManager buttonManager;
    
    private string fileName = "results";
    private int[] _results;
    private int _numLevel = 0;

    void ReadFile()
    {
        TextAsset data = Resources.Load<TextAsset>(fileName);
        string[] lines;
        if (data)
        {
            lines = data.text.Split('\n');
        }
        else return;

        if (lines.Length > 0)
        {
            foreach (string line in lines)
            {
                Debug.Log("line: " + line);
                string[] parts = line.Split(' ');
                if (parts.Length >= 2)
                {
                    _numLevel++;
                    int index = int.Parse(parts[0]);
                    int value = int.Parse(parts[1]);

                    _results[index-1] = value;
                }
            }
        }
        else
        {
            Debug.LogError("File is empty or doesn't exist.");
        }
    }
    
    
    private void Awake()
    {
        Debug.Log("Awake");
        _results = new int[10];
        ReadFile();
        for (int i = 0; i < _numLevel; i++)
        {
            GameObject level = null;
            if (_results[i] == 0)
            {
                level = Instantiate(btnLevel0StarPrefab, grid);
                Text text = level.GetComponentInChildren<Text>();
                text.text = (i+1).ToString();
                buttonManager.btnLevels.Add(level.GetComponent<Button>());
            }
            else if (_results[i] == 1)
            {
                level = Instantiate(btnLevel1StarPrefab, grid);
                Text text = level.GetComponentInChildren<Text>();
                text.text = (i+1).ToString();
                buttonManager.btnLevels.Add(level.GetComponent<Button>());
            }
            else if (_results[i] == 2)
            {
                level = Instantiate(btnLevel2StarPrefab, grid);
                Text text = level.GetComponentInChildren<Text>();
                text.text = (i+1).ToString();
                buttonManager.btnLevels.Add(level.GetComponent<Button>());
            }
            else if (_results[i] == 3)
            {
                level = Instantiate(btnLevel3StarPrefab, grid);
                Text text = level.GetComponentInChildren<Text>();
                text.text = (i+1).ToString();
                buttonManager.btnLevels.Add(level.GetComponent<Button>());
            }else if (_results[i] == -1)
            {
                level = Instantiate(btnLevelLockPrefab, grid);
            }
        }
        
        Debug.Log("Last Awake");
    }
}
