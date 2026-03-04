using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller_T : MonoBehaviour
{
    public float beatTempo;
    private float originalBeatTempo;
    public bool hasStarted = false;

    void Start()
    {
        originalBeatTempo = beatTempo; // Store the initial tempo
        beatTempo = beatTempo / 60f;
    }

    void Update()
    {
        if(!hasStarted)
        {
            /*if (Input.anyKeyDown)
                hasStarted = true;*/
        }
        else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }

    // beatTempo와 상태를 초기화하는 메서드
    public void ResetBeatTempo()
    {
        beatTempo = originalBeatTempo / 60f; // 초기 beatTempo 값으로 재설정
        hasStarted = false; // 노트 움직임을 멈추고 대기 상태로 변경
    }
}
