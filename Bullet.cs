using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 8f; // 단일 이동 속력
    private Rigidbody bulletRigidbody; /// 이동에 사용할 리지드바디 컴포넌트

    void Start() {
        // 게임 오브젝트에서 Rigidbody 컴포넌트를 찾아 bulletRigidbody 에 할당
        bulletRigidbody = GetComponent<Rigidbody>();
        // 리지드바디의 속도   = 앞쪽방향 * 이동속력
        bulletRigidbody.velocity = transform.forward * speed;

        // 3초 뒤에 자신의 게임 오브젝트 파괴
        Destroy(gameObject, 3f);
    }

    // 트리거 충돌 시 자동으로 실행되는 메서드
    void OnTriggerEnter(Collider other) {
        // 충돌한 상대방 게임 오브젝트가 Player 태그를 가질 경우
        if (other.tag == "Player") {
            // 상대방 게임 오브젝트에서 PlayerController 컴포넌트 가져오기
            PlayerController playerController = other.GetComponent<PlayerController>();

            // playerController가 null이 아닌 경우 TakeDamage() 메서드 실행
            if (playerController != null) {
                playerController.TakeDamage();

                // 총알도 제거
                Destroy(gameObject);
            }
        }
    }
}