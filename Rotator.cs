using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;
    private GameManager gameManager;

    void Start()
    {
        // GameManager 찾기
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // 게임매니저가 있고, 게임이 시작되었으며, 게임오버 상태가 아닐 때만 회전
        if (gameManager != null && gameManager.IsGameStarted() && !gameManager.IsGameover())
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }
}