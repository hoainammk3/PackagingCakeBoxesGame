using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject grid;
    [SerializeField] private TimerCount timerCount;
    [SerializeField] private GameObject[,] gridObjects; // Các đối tượng con trong grid
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private ScoreStore scoreStore;
    [SerializeField] private ResultHandler resultHandler;
    [SerializeField] private AudioManager audioManager;
    
    private const int MaxPos = 560;
    private const int MinPos = 120;
    private const int GridSize = 3;
    private int _level = 1;
    private int _score = 0;
    private GameState _gameState;
    
    public int Level
    {
        get => _level;
        set => _level = value;
    }
    public int Score    
    {
        get => _score;
        set => _score = value;
    }
    public GameState GameState
    {
        get => _gameState;
        set => _gameState = value;
    }

    private void Start()
    {
        _level = ButtonManager.LevelChoose;
        gridObjects = GetChildObjects(grid.transform);
        LoadMap(_level);
        _gameState = GameState.Running;
    }

    GameObject[,] GetChildObjects(Transform parent)
    {
        GameObject[,] childObjects = new GameObject[GridSize, GridSize];

        // Lặp qua từng đối tượng con của parent
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            RectTransform rt = child.GetComponent<RectTransform>();
            var anchoredPosition = rt.anchoredPosition;
            int indexX = (int)anchoredPosition.y / -ConstValue.SPACE_CELL + 1;
            int indexY = (int) anchoredPosition.x / ConstValue.SPACE_CELL + 1;
            childObjects[indexX, indexY] = child.gameObject;
        }

        return childObjects;
    }
    private void Update()
    {
        if (_gameState == GameState.Running)
        {
            if (inputManager.direction == Vector2.down)
            {
                MoveDown();
                Claim();
            }
            if (inputManager.direction == Vector2.left)    MoveLeft();
            if (inputManager.direction == Vector2.right)    MoveRight();
            if (inputManager.direction == Vector2.up)    MoveUp();
//            inputManager.direction = Vector2.zero;
            if (IsWinGame())
            {
                DeleteCakeObj();
                WinGame();
            }
        }

        if (timerCount.IsTimeUp && _gameState == GameState.Running)
        {
            _gameState = GameState.Lose;
            _score = 0;
            audioManager.PlayLoseClip();
        }
    }

    private void HandleMove(int i, int j, Queue<Vector2> queue)
    {
        if (!gridObjects[i, j])
        {
            queue.Enqueue(new Vector2(i, j));
        }
        else
        {
            CellMove cellMove = gridObjects[i, j].GetComponent<CellMove>();
            if (!cellMove)
            {
                queue.Clear();
            }
            else
            {
                if (queue.Count == 0) return;
                queue.Enqueue(new Vector2(i, j));
                Vector2 des = queue.Dequeue();
                gridObjects[i, j].GetComponent<CellMove>().MoveTo(des, 0.2f);
                gridObjects[(int) des.x, (int) des.y] = gridObjects[i, j];
                gridObjects[i, j] = null;
                audioManager.PlayMoveClip();
            }
        }
    }

    private void MoveLeft()
    {
        for (int i = 0; i < GridSize; i++)
        {
            Queue<Vector2> queueLeft = new Queue<Vector2>();
            for (int j = 0; j < GridSize; j++)
            {
                HandleMove(i, j, queueLeft);
            }
        }
    }
    
    private void MoveRight()
    {
        for (int i = 0; i < GridSize; i++)
        {
            Queue<Vector2> queueRight = new Queue<Vector2>();
            for (int j = GridSize - 1; j >= 0; j--)
            {
                HandleMove(i, j, queueRight);
            }
        }
    
    }
    
    private void MoveDown()
    {
        for (int j = 0; j < GridSize; j++)
        {
            Queue<Vector2> queueDown = new Queue<Vector2>();
            for (int i = GridSize - 1; i >= 0; i--)
            {
                HandleMove(i, j, queueDown);
            }
        }
    }
    
    private void MoveUp()
    {
        for (int j = 0; j < GridSize; j++)
        {
            Queue<Vector2> queueUp = new Queue<Vector2>();
            for (int i = 0; i < GridSize; i++)
            {
                HandleMove(i, j, queueUp);
            }
        }
    }

    private void Claim()
    {
        Vector2 giftPos = GetIndexByName("Gift(Clone)");
        int xGift = (int)giftPos.x;
        int yGift = (int)giftPos.y;
        
        for (int i = xGift - 1; i >= 0; i--)
        {
            if (gridObjects[i, yGift] != null)
            {
                if (!gridObjects[i, yGift].CompareTag("CanEat")) return;
                gridObjects[i, yGift].GetComponent<CellMove>().MoveTo(new Vector2(xGift, yGift), 0.3f);
            }
        }
    }

    private bool IsWinGame()
    {
        Vector2 giftPos = new Vector2(-1, -1);
        Vector2 cakePos = new Vector2(1, 1);
        foreach (var obj in gridObjects)
        {
            if (!obj) continue;
            if (obj.name == "Gift(Clone)") giftPos = GetPositionByGameObject(obj);
            if (obj.name == "Cake(Clone)") cakePos = GetPositionByGameObject(obj);
        }
        return giftPos == cakePos;
    }
    
    private Vector2 GetIndexByName(string nameGrid)
    {
        int xGift = -1, yGift = -1;
        for (int j = 0; j < GridSize; j++)
        {
            for (int i = GridSize - 1; i >= 0; i--)
            {
                if (!gridObjects[i, j]) continue;
                if (gridObjects[i, j].name != nameGrid) continue;
                xGift = i;
                yGift = j;
            }
        }

        return new Vector2(xGift, yGift);
    }

    private Vector2 GetPositionByGameObject(GameObject obj)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        if (!rt) return new Vector2(-1, -1);
        var anchoredPosition = rt.anchoredPosition;
        return new Vector2(anchoredPosition.x, anchoredPosition.y);
    }
    
    private void WinGame()
    {
        _gameState = GameState.Win;
        if (timerCount.TimeRemaining > 20) _score = 3;
        else if (timerCount.TimeRemaining > 10) _score = 2;
        else if (timerCount.TimeRemaining > 0) _score = 1;
        audioManager.PlayWinClip();
        scoreStore.AddScore(_level, _score);
    }

    private void DeleteCakeObj()
    {
        foreach (var obj in gridObjects)
        {
            if (!obj) continue;
            if (obj.name == "Cake(Clone)") obj.gameObject.SetActive(false);
        }
    }
    
    public void LoadMap(int level)
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (gridObjects != null) Destroy(gridObjects[i,j]);
            }
        }
        if (mapGenerator != null) mapGenerator.GenerateMap(level);
        timerCount.Reset();
        _gameState = GameState.Running;
        gridObjects = GetChildObjects(grid.transform);
        resultHandler.Reset();
    }

    public void ResetGame()
    {
        LoadMap(_level);
    }

    public void LoadNextLevel()
    {
        LoadMap(++_level);
    }
}
