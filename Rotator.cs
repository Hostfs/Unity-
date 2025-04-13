using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;
    private GameManager gameManager;

    void Start()
    {
        // GameManager ã��
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // ���ӸŴ����� �ְ�, ������ ���۵Ǿ�����, ���ӿ��� ���°� �ƴ� ���� ȸ��
        if (gameManager != null && gameManager.IsGameStarted() && !gameManager.IsGameover())
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }
}