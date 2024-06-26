using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class CellMove : MonoBehaviour
{
    public bool running = false;
    public void MoveTo(Vector2 newPosition, float duration)
    {
        if (running) return;
        StartCoroutine(MoveCoroutine(newPosition, duration));
    }
    
    private IEnumerator MoveCoroutine(Vector2 newPosition, float duration)
    {
        running = true;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = new Vector2((newPosition.y-1) * ConstValue.SPACE_CELL, (newPosition.x-1) * -ConstValue.SPACE_CELL);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = endPosition;
        running = false;
    }
}
