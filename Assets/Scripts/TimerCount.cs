using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class TimerCount : MonoBehaviour
{
    [SerializeField] private const float timeReset = 45f;
    [SerializeField] private GameController _gameController;
    [SerializeField] private Text timerText;
    
    private float _timeRemaining;

    public float TimeRemaining
    {
        get => _timeRemaining;
        set => _timeRemaining = value;
    }

    private bool _isTimeUp;
    
    public bool IsTimeUp
    {
        get => _isTimeUp;
        set => _isTimeUp = value;
    }
//    public Text timerText;

    void Start()
    {
        // Thiết lập thời gian ban đầu (45 giây)
        _timeRemaining = timeReset;
        _isTimeUp = false;
        
        // Hiển thị thời gian ban đầu
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (_timeRemaining > 1 && _gameController.GameState == GameState.Running)
        {
            // Giảm thời gian theo từng frame
            _timeRemaining -= Time.deltaTime;

            // Cập nhật hiển thị
            UpdateTimerDisplay();
        }
        else
        {
            _timeRemaining = 0;
            _isTimeUp = true;
        }
    }

    void UpdateTimerDisplay()
    {
        // Hiển thị thời gian dưới dạng phút:giây
        int minutes = Mathf.FloorToInt(_timeRemaining / 60);
        int seconds = Mathf.FloorToInt(_timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Reset()
    {
        _timeRemaining = timeReset;
        _isTimeUp = false;
        
        UpdateTimerDisplay();
    }
}
