using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ���� ���̺귯��
using UnityEngine.SceneManagement; // �� ���� ���� ���̺귯��

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText; // ���� ���� �� Ȱ��ȭ �� �ؽ�Ʈ ���� ������Ʈ
    public GameObject startText; // ���� ���� �� Ȱ��ȭ�� �ؽ�Ʈ ������Ʈ
    public Text timeText; // ���� �ð��� ǥ���� �ؽ�Ʈ ������Ʈ
    public Text recordText; // �ְ� ����� ǥ���� �ؽ�Ʈ ������Ʈ
    public Text lifeText; // ������� ǥ���� �ؽ�Ʈ ������Ʈ

    public float surviveTime; // �����ð�
    private bool isGameover; // ���ӿ��� ����
    private bool isGameStarted; // ���� ���� ����

    void Start()
    {
        // �����ð��� ���ӿ���, ���� ���� ���� �ʱ�ȭ
        surviveTime = 0;
        isGameover = false;
        isGameStarted = false;

        // ���� ȭ�� ����
        if (startText != null)
        {
            startText.SetActive(true);
            Debug.Log("���� ȭ�� �ʱ�ȭ �Ϸ�: startText Ȱ��ȭ��");
        }
        else
        {
            Debug.LogError("startText�� Inspector���� ������� �ʾҽ��ϴ�!");
        }

        if (gameoverText != null)
        {
            gameoverText.SetActive(false);
        }
        else
        {
            Debug.LogError("gameoverText�� Inspector���� ������� �ʾҽ��ϴ�!");
        }

        // �ʱ� ����� UI ����
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            UpdateLifeUI(playerController.lifes);
        }

        // �÷��̾� ��Ȱ��ȭ
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // �Ѿ� ������ ��Ȱ��ȭ
        BulletSpawner[] spawners = FindObjectsOfType<BulletSpawner>();
        foreach (BulletSpawner spawner in spawners)
        {
            spawner.enabled = false;
        }
    }

    void Update()
    {
        // ������ ���۵��� �ʾҰ� SŰ�� ������ ���� ����
        if (!isGameStarted && Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S Ű �Է� ����, StartGame ȣ��");
            StartGame();
        }

        // ���ӿ����� �ƴ� ���� (�׸��� ������ ���۵��� ��)
        if (!isGameover && isGameStarted)
        {
            // �����ð� ����
            surviveTime += Time.deltaTime;
            // ������ ���� �ð��� timeText �ؽ�Ʈ ������Ʈ�� �̿��� ǥ��
            timeText.text = "���� �ð�: " + (int)surviveTime + "��";
        }
        else if (isGameover)
        {
            // ���� ���� ���¿��� R Ű�� ���� ���
            if (Input.GetKeyDown(KeyCode.R))
            {
                // SampleScene ���� �ε�
                SceneManager.LoadScene("SampleScene");
            }

            // ���� ���� ���¿��� ESC Ű�� ���� ���
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ���� ����
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif

                gameoverText.SetActive(false);
            }
        }
    }

    private void StartGame()
    {
        Debug.Log("StartGame �޼��� ����");
        isGameStarted = true;

        // startText�� null�� �ƴ��� Ȯ���ϰ� ��Ȱ��ȭ
        if (startText != null)
        {
            Debug.Log("startText ��Ȱ��ȭ �õ� �� ����: " + startText.activeSelf);
            startText.SetActive(false); // ���� �ؽ�Ʈ ��Ȱ��ȭ
            Debug.Log("startText ��Ȱ��ȭ �� ����: " + startText.activeSelf);

            // Canvas �Ǵ� ���� UI ��ҵ��� Ȱ��ȭ �������� Ȯ��
            Transform parent = startText.transform.parent;
            while (parent != null)
            {
                Debug.Log("���� UI ���: " + parent.name + ", Ȱ��ȭ ����: " + parent.gameObject.activeSelf);
                parent = parent.parent;
            }
        }
        else
        {
            Debug.LogError("startText�� null�Դϴ�. Inspector���� ���� Ȯ���ϼ���.");
        }

        // �÷��̾� Ȱ��ȭ
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("�÷��̾� ��Ʈ�ѷ� Ȱ��ȭ��");
        }
        else
        {
            Debug.LogError("PlayerController�� ã�� �� �����ϴ�!");
        }

        // �Ѿ� ������ Ȱ��ȭ
        BulletSpawner[] spawners = FindObjectsOfType<BulletSpawner>();
        if (spawners.Length > 0)
        {
            foreach (BulletSpawner spawner in spawners)
            {
                spawner.enabled = true;
            }
            Debug.Log(spawners.Length + "���� �Ѿ� ������ Ȱ��ȭ��");
        }
        else
        {
            Debug.LogWarning("Ȱ��ȭ�� �Ѿ� �����ʰ� �����ϴ�!");
        }

        Debug.Log("StartGame �޼��� ����, ���� ���� ����: " + isGameStarted);
    }

    public void UpdateLifeUI(int currentLifes)
    {
        // ����� UI ������Ʈ
        if (lifeText != null)
        {
            lifeText.text = "���� ����: " + currentLifes;
        }
        else
        {
            Debug.LogError("lifeText�� null�Դϴ�!");
        }
    }

    public void EndGame()
    {
        // ���� ���¸� ���� ���� ���·� ��ȯ
        isGameover = true;

        // ���ӿ��� �ؽ�Ʈ ���� ������Ʈ�� Ȱ��ȭ
        if (gameoverText != null)
        {
            gameoverText.SetActive(true);
        }
        else
        {
            Debug.LogError("gameoverText�� null�Դϴ�!");
        }

        // BestTime Ű�� ����� ���������� �ְ� ��� ��������
        float bestTime = PlayerPrefs.GetFloat("BestTime");

        // ���������� �ְ� ��Ϻ��� ���� �����ð��� �� ũ�ٸ�
        if (surviveTime > bestTime)
        {
            // �ְ� ��� ���� ���� ���� �ð� ������ ����
            bestTime = surviveTime;
            // ����� �ְ� ����� BestTime Ű�� ����
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        // �ְ� ����� recordText �ؽ�Ʈ ������Ʈ�� �̿��� ǥ��
        if (recordText != null)
        {
            recordText.text = "�ְ� ���: " + (int)bestTime + "��";
        }
        else
        {
            Debug.LogError("recordText�� null�Դϴ�!");
        }
    }

    public bool IsGameover()
    {
        return isGameover; // ���� ���� �ƴ��� �ȵƴ��� �ٸ� cs �� �����ϱ� ���� �뵵
    }

    public bool IsGameStarted()
    {
        return isGameStarted; // ������ ���۵ƴ��� Ȯ��
    }
}