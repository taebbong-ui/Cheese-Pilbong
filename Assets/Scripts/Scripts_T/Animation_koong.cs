using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TapAnimation : MonoBehaviour
{
    private Animator anim;
    private bool isKoong = false;
   

    void Start()
    {
        // 현재 오브젝트에 연결된 Animator 컴포넌트를 가져옵니다.
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Tap 키를 눌렀을 때 애니메이션 재생
        if (Input.GetKeyDown(KeyCode.Tab))
            anim.SetBool("isKoong", true);

        // Tap 키를 떼었을 때 애니메이션 멈춤
        if (Input.GetKeyUp(KeyCode.Tab))
            anim.SetBool("isKoong", false);
    }
}
