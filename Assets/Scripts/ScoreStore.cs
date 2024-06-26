using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreStore : MonoBehaviour
{
    private int[] scores;
    private string filePath = "Assets/Resources/results.txt";
    // Đọc tất cả các dòng từ file vào mảng lines
    private string[] _lines;
    
    private void Awake()
    {
        _lines = File.ReadAllLines(filePath);
        scores = new int[10];
    }

    public void AddScore(int level, int score)
    {
        scores[level] = score;
        // Kiểm tra xem có dữ liệu trong file hay không
        if (_lines.Length > 0)
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                // Tách dòng thành level và score
                string[] parts = _lines[i].Split(' ');
                int _level = int.Parse(parts[0]);
                int _score = int.Parse(parts[1]);

                // Kiểm tra điều kiện sửa đổi
                if (level == _level && score > _score)
                {
                    // Thực hiện sửa đổi
                    _lines[i] = level + " " + score;
                }

                if (_level == level + 1 && _score == -1)
                {
                    _lines[i] = _level + " 0";
                }
            }

            // Ghi lại dữ liệu vào file
            File.WriteAllLines(filePath, _lines);
        }
        else
        {
            Debug.LogError("File is empty or doesn't exist.");
        }
        
    }

    public int GetScore(int level)
    {
        return scores[level];
    }
}
