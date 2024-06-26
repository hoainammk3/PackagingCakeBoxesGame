using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;
    private bool _isSwiping = false;
    public Vector2 direction;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPosition = touch.position;
                    _isSwiping = true;
                    break;

                case TouchPhase.Ended:
                    if (_isSwiping)
                    {
                        _endTouchPosition = touch.position;
                        direction = DetectSwipe();
                        _isSwiping = false;
                    }
                    break;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            _startTouchPosition = Input.mousePosition;
            _isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_isSwiping)
            {
                _endTouchPosition = Input.mousePosition;
                direction = DetectSwipe();
                _isSwiping = false;
            }
        }
    }
    
    public Vector2 DetectSwipe()
    {
        Vector2 swipeDirection = _endTouchPosition - _startTouchPosition;
        float swipeDistance = swipeDirection.magnitude;

        // Chỉ nhận diện swipe nếu khoảng cách vuốt đủ lớn
        if (swipeDistance > 50) // Bạn có thể điều chỉnh giá trị này theo nhu cầu
        {
            swipeDirection.Normalize();

            // Nhận diện hướng swipe
            if (IsSwipeUp(swipeDirection))
            {
//                Debug.Log("Up");
                // Xử lý khi vuốt lên
                return Vector2.up;
            }
            else if (IsSwipeDown(swipeDirection))
            {
//                Debug.Log("Down");
                // Xử lý khi vuốt xuống
                return Vector2.down;
            }
            else if (IsSwipeLeft(swipeDirection))
            {
//                Debug.Log("Left");
                // Xử lý khi vuốt trái
                return Vector2.left;
            }
            else if (IsSwipeRight(swipeDirection))
            {
//                Debug.Log("Right");
                // Xử lý khi vuốt phải
                return Vector2.right;
            }
        }
        return Vector2.zero;
    }

    private bool IsSwipeUp(Vector2 direction)
    {
        return direction.y > 0 && Mathf.Abs(direction.y) > Mathf.Abs(direction.x);
    }

    private bool IsSwipeDown(Vector2 direction)
    {
        return direction.y < 0 && Mathf.Abs(direction.y) > Mathf.Abs(direction.x);
    }

    private bool IsSwipeLeft(Vector2 direction)
    {
        return direction.x < 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
    }

    private bool IsSwipeRight(Vector2 direction)
    {
        return direction.x > 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
    }
}
