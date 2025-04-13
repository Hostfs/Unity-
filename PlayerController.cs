using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Rigidbody playerRigidbody; // 이동에 사용할 리지드바디 컴포넌트
    public float speed = 9f; // 이동 속력
    public int lifes = 3; // 플레이어 생명력 초기값 3으로 구현

    // Start is called before the first frame update
    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // 실제 이동 속도를 입력값과 이동 속력을 사용해 결정
        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;
        // Vector 3 속도를 xSpeed, 0, zSpeed 로 생성
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        // 리지드 바디의 속도에 newVelocity 할당
        playerRigidbody.velocity = newVelocity;
    }

    public void TakeDamage() {
        // 생명력 감소
        lifes--;

        // 생명력이 0 이하라면 게임 오버
        if (lifes <= 0) {
            Die();
        } else {
            // 피격 효과 추가 (선택 사항)
            StartCoroutine(DamageEffect());
        }

        // GameManager에 생명력 업데이트 알림
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.UpdateLifeUI(lifes);
    }

    private IEnumerator DamageEffect() {
        // 피격시 잠시 깜빡
        for (int i = 0; i < lifes; i++) {
            GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Die() {
        // 자신의 게임 오브젝트를 비활성화
        gameObject.SetActive(false);

        // 씬에 존재하는 GameManager 타입의 오브젝트를 찾아서 가져오기
        GameManager gameManager = FindObjectOfType<GameManager>();

        // 가져온 GameManager 오브젝트의 EndGame() 메서드 실행
        gameManager.EndGame();
    }
}