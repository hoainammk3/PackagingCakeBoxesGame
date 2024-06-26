using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using UnityEngine;

public class ResultHandler : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private List<GameObject> resultList;
    private int[] results;
    private string fileName = "results.txt";
    private int _point;
    private void Awake()
    {
        _point = gameController.Score;
        results = new int[10];
    }

    private void Update()
    {
        if (gameController.GameState != GameState.Running)
        {
            resultList[gameController.Score].gameObject.SetActive(true);

            if (results[gameController.Level] < gameController.Score)
            {
                results[gameController.Level] = gameController.Score;
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < resultList.Count; i++)
        {
            resultList[i].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
                 
        string path = Path.Combine(Application.persistentDataPath, fileName);

        using (StreamWriter writer = new StreamWriter(path))
        {
            for (int i = 0; i < results.Length; i++)
            {
                writer.WriteLine($"{i} {results[i]}");
            }
        }
    }
}
