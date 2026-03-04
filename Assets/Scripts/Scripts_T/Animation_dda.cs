using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReturnAnimation : MonoBehaviour
{
    private Animator Anim;
    private bool isDda = false;


    void Start()
    {
        // 현재 오브젝트에 연결된 Animator 컴포넌트를 가져옵니다.
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Return 키를 눌렀을 때 애니메이션 재생
        if (Input.GetKeyDown(KeyCode.Return))
            Anim.SetBool("isDda", true);

        // Return 키를 떼었을 때 애니메이션 멈춤
        if (Input.GetKeyUp(KeyCode.Return))
            Anim.SetBool("isDda", false);
    }
}