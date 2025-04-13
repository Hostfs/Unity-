using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 라이브러리
using UnityEngine.SceneManagement; // 씬 관리 관련 라이브러리

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText; // 게임 오버 시 활성화 할 텍스트 게임 오브젝트
    public GameObject startText; // 게임 시작 전 활성화할 텍스트 오브젝트
    public Text timeText; // 생존 시간을 표시할 텍스트 컴포넌트
    public Text recordText; // 최고 기록을 표시할 텍스트 컴포넌트
    public Text lifeText; // 생명력을 표시할 텍스트 컴포넌트

    public float surviveTime; // 생존시간
    private bool isGameover; // 게임오버 상태
    private bool isGameStarted; // 게임 시작 상태

    void Start()
    {
        // 생존시간과 게임오버, 게임 시작 상태 초기화
        surviveTime = 0;
        isGameover = false;
        isGameStarted = false;

        // 시작 화면 설정
        if (startText != null)
        {
            startText.SetActive(true);
            Debug.Log("시작 화면 초기화 완료: startText 활성화됨");
        }
        else
        {
            Debug.LogError("startText가 Inspector에서 연결되지 않았습니다!");
        }

        if (gameoverText != null)
        {
            gameoverText.SetActive(false);
        }
        else
        {
            Debug.LogError("gameoverText가 Inspector에서 연결되지 않았습니다!");
        }

        // 초기 생명력 UI 설정
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            UpdateLifeUI(playerController.lifes);
        }

        // 플레이어 비활성화
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // 총알 스포너 비활성화
        BulletSpawner[] spawners = FindObjectsOfType<BulletSpawner>();
        foreach (BulletSpawner spawner in spawners)
        {
            spawner.enabled = false;
        }
    }

    void Update()
    {
        // 게임이 시작되지 않았고 S키를 누르면 게임 시작
        if (!isGameStarted && Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S 키 입력 감지, StartGame 호출");
            StartGame();
        }

        // 게임오버가 아닌 동안 (그리고 게임이 시작됐을 때)
        if (!isGameover && isGameStarted)
        {
            // 생존시간 갱신
            surviveTime += Time.deltaTime;
            // 갱신한 생존 시간을 timeText 텍스트 컴포넌트를 이용해 표시
            timeText.text = "진행 시간: " + (int)surviveTime + "초";
        }
        else if (isGameover)
        {
            // 게임 오버 상태에서 R 키를 누른 경우
            if (Input.GetKeyDown(KeyCode.R))
            {
                // SampleScene 씬을 로드
                SceneManager.LoadScene("SampleScene");
            }

            // 게임 오버 상태에서 ESC 키를 누른 경우
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 게임 종료
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
        Debug.Log("StartGame 메서드 시작");
        isGameStarted = true;

        // startText가 null이 아닌지 확인하고 비활성화
        if (startText != null)
        {
            Debug.Log("startText 비활성화 시도 전 상태: " + startText.activeSelf);
            startText.SetActive(false); // 시작 텍스트 비활성화
            Debug.Log("startText 비활성화 후 상태: " + startText.activeSelf);

            // Canvas 또는 상위 UI 요소들이 활성화 상태인지 확인
            Transform parent = startText.transform.parent;
            while (parent != null)
            {
                Debug.Log("상위 UI 요소: " + parent.name + ", 활성화 상태: " + parent.gameObject.activeSelf);
                parent = parent.parent;
            }
        }
        else
        {
            Debug.LogError("startText가 null입니다. Inspector에서 연결 확인하세요.");
        }

        // 플레이어 활성화
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("플레이어 컨트롤러 활성화됨");
        }
        else
        {
            Debug.LogError("PlayerController를 찾을 수 없습니다!");
        }

        // 총알 스포너 활성화
        BulletSpawner[] spawners = FindObjectsOfType<BulletSpawner>();
        if (spawners.Length > 0)
        {
            foreach (BulletSpawner spawner in spawners)
            {
                spawner.enabled = true;
            }
            Debug.Log(spawners.Length + "개의 총알 스포너 활성화됨");
        }
        else
        {
            Debug.LogWarning("활성화할 총알 스포너가 없습니다!");
        }

        Debug.Log("StartGame 메서드 종료, 게임 시작 상태: " + isGameStarted);
    }

    public void UpdateLifeUI(int currentLifes)
    {
        // 생명력 UI 업데이트
        if (lifeText != null)
        {
            lifeText.text = "남은 생명: " + currentLifes;
        }
        else
        {
            Debug.LogError("lifeText가 null입니다!");
        }
    }

    public void EndGame()
    {
        // 현재 상태를 게임 오버 상태로 전환
        isGameover = true;

        // 게임오버 텍스트 게임 오브젝트를 활성화
        if (gameoverText != null)
        {
            gameoverText.SetActive(true);
        }
        else
        {
            Debug.LogError("gameoverText가 null입니다!");
        }

        // BestTime 키로 저장된 이전까지의 최고 기록 가져오기
        float bestTime = PlayerPrefs.GetFloat("BestTime");

        // 이전까지의 최고 기록보다 현재 생존시간이 더 크다면
        if (surviveTime > bestTime)
        {
            // 최고 기록 값을 현재 생존 시간 값으로 변경
            bestTime = surviveTime;
            // 변경된 최고 기록을 BestTime 키로 저장
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        // 최고 기록을 recordText 텍스트 컴포넌트를 이용해 표시
        if (recordText != null)
        {
            recordText.text = "최고 기록: " + (int)bestTime + "초";
        }
        else
        {
            Debug.LogError("recordText가 null입니다!");
        }
    }

    public bool IsGameover()
    {
        return isGameover; // 게임 오버 됐는지 안됐는지 다른 cs 에 전달하기 위한 용도
    }

    public bool IsGameStarted()
    {
        return isGameStarted; // 게임이 시작됐는지 확인
    }
}