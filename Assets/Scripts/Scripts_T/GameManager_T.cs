using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_T : MonoBehaviour
{
    public GameObject menuSet;
    public GameObject retrySet; // Retry 버튼을 포함한 Retry Set UI
    public GameObject nextSet;

    public AudioSource theMusic;
    public AudioSource speaker;   // 버튼 오디오를 위한 AudioSource를 연결하세요.

    public bool startPlaying;

    public BeatScroller_T theBS;

    public static GameManager_T instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 200;
    public int scorePerPerfectNote = 300;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public Text scoreText;
    public Text multiText;

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    public GameObject resultsScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;
  
    public bool isStageComplete = false;
    public string rankVal = "";  // 플레이어의 현재 스테이지 등급을 저장

    public List<NoteObject_T> noteObjects = new List<NoteObject_T>(); // 리스트 초기화

    public Button startButton; // Start 버튼 참조
    public Button retryButton; // Retry 버튼 참조
    public Button nextButton;
    public Button speakerButton; // SpeakerButton을 에디터에서 연결하세요.



    private bool gameStarted = false;
    private float originalBeatTempo;


    private void Awake()
    {
        instance = this;

        // 게임 오브젝트로부터 NoteObject들을 자동으로 추가할 수도 있습니다.
        noteObjects.AddRange(FindObjectsOfType<NoteObject_T>());

        theBS = FindObjectOfType<BeatScroller_T>();
        originalBeatTempo = theBS.beatTempo;

        speakerButton.onClick.AddListener(PlaySong);

        startButton.onClick.AddListener(StartGame); // Start 버튼 클릭 시 StartGame 호출
        //retryButton.onClick.AddListener(RetryStage1); // Retry 버튼 클릭 시 RetryStage1 호출

        menuSet.SetActive(true);  // 게임 시작 전에는 Menu Set 활성화
        retrySet.SetActive(false); // Retry Set은 숨김
    }

    void PlaySong()
    {
        // buttonAudioSource가 재생 중이 아니면 재생합니다.
        if (!speaker.isPlaying)
        {
            speaker.Play();
        }
    }

        private void StartGame()
    {
        gameStarted = true;
        startPlaying = true;
        theBS.hasStarted = true;

        if(speaker.isPlaying)
        {
            speaker.Stop();
        }

        theMusic.PlayDelayed(0.07f); // 0.1초 지연 후 음악 재생
        theMusic.Play();            // 음악 재생

        menuSet.SetActive(false); // Menu Set 숨기기
        retrySet.SetActive(false); // Retry Set 숨기기
    }


    private void ResetNotes()
    {
        foreach (NoteObject_T note in noteObjects)
        {
            note.ResetNote();  // 각 노트를 초기 상태로 리셋
            Debug.Log("Resetting note: " + note.name);  // 호출 확인
        }
    }

    void Start()
    {
        instance = this;

        scoreText.text = "점   수 : 0";
        currentMultiplier = 1;

        totalNotes = FindObjectsOfType<NoteObject_T>().Length;
    }
  
   

    void Update()
    {
        if (menuSet.activeSelf)
            menuSet.SetActive(true);

       if(startPlaying)
        {
            // Stage1 음악이 끝났을 때
            if (!theMusic.isPlaying && !isStageComplete)
            {
                isStageComplete = true; // Stage1 완료 표시

                // Stage1 등급 계산
                CalculateRank();

                // 등급에 따라 Stage2로 전환 또는 Stage1 재시도
                if (rankVal == "B" || rankVal == "A" || rankVal == "S")
                {
                    NextStage();
                }
                else
                {
                    RetryStage1(); // B 미만 등급일 경우 Stage1을 재시도
                }
            }
        }
    }

    void NextStage()
    {
        nextSet.SetActive(true);

        nextButton.onClick.AddListener(() =>
        {
            nextSet.SetActive(false); // Next 버튼을 누르면 Next Set을 숨김
            Destroy(GameManager_T.instance.gameObject);
            SceneManager.LoadScene("game02_2");
        });
    }

    // 등급을 계산하는 함수
    private void CalculateRank()
    {
        float totalHit = normalHits + goodHits + perfectHits;
        float percentHit = (totalHit / totalNotes) * 100f;

        // 등급 계산
        rankVal = "F";
        if (percentHit > 40) rankVal = "D";
        if (percentHit > 55) rankVal = "C";
        if (percentHit > 70) rankVal = "B";
        if (percentHit > 85) rankVal = "A";
        if (percentHit > 95) rankVal = "S";
    }
     

    // Stage1을 다시 시도
    private void RetryStage1()
    {
        Debug.Log("Retrying Stage 1");
        isStageComplete = false;
        gameStarted = false;
        rankVal = "";

        ResetGameSettings();
        ResetNotes();
        theBS.ResetBeatTempo();
        theMusic.Stop();

        retrySet.SetActive(true);  // 이 부분이 제대로 실행되는지 확인
        startButton.gameObject.SetActive(false); // Start 버튼 숨기기
        retryButton.onClick.RemoveAllListeners(); // 이전 리스너 제거


        retryButton.onClick.AddListener(() =>
        {
            retrySet.SetActive(false); // Retry 버튼을 누르면 Retry Set을 숨김


            gameStarted = true;
            startPlaying = true;
            theBS.hasStarted = true;    // BeatScroller 시작
            theMusic.PlayDelayed(0.04f); // 0.05초 지연 후 음악 재생
            theMusic.Play();            // 음악 재생
        });
    }


    private void ResetGameSettings()
    {
        currentScore = 0;
        normalHits = 0;
        goodHits = 0;
        perfectHits = 0;
        missedHits = 0;
        startPlaying = false;
        theBS.ResetBeatTempo();
        theBS.hasStarted = false;
        resultsScreen.SetActive(false);

        scoreText.text = "점   수 : 0";
        currentMultiplier = 1;
        multiText.text = "곱빼기 : x" + currentMultiplier;
    }


    public void NoteHit()
    {
        Debug.Log("Hit on Time");

        if(currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        multiText.text = "곱빼기 : x" + currentMultiplier;

        //currentScore += scorePerNote * currentMultiplier;
        scoreText.text = "점   수 : " + currentScore;
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        normalHits++;
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        goodHits++;
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        perfectHits++;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");

        currentMultiplier = 1;
        multiplierTracker = 0;

        multiText.text = "곱빼기 : x" + currentMultiplier;

        missedHits++;
    }
}




/*
// Stage1에서 Stage2로 전환
private void TransitionToStage2()
{
    Debug.Log("Transitioning to Stage 2");
    currentStage = Stage.Stage2;
    // Stage2 전환 시 필요한 추가 작업 수행
}

// Stage2가 끝났을 때 호출할 함수
public void EndGame()
{
    Debug.Log("Game Over");
    Application.Quit();
}


void Update()
{
    if(menuSet.activeSelf)
        menuSet.SetActive(true);


    if(!startPlaying)
    {
        if(Input.anyKeyDown)
        {
            startPlaying = true;
            theBS.hasStarted = true;

            theMusic.Play();
        }
    }
    else
    {
        if(!theMusic.isPlaying && !resultsScreen.activeInHierarchy)
        {
            resultsScreen.SetActive(true);

            normalsText.text = "" + normalHits;
            goodsText.text = goodHits.ToString();
            perfectsText.text = perfectHits.ToString();
            missesText.text = "" + missedHits;

            float totalHit = normalHits + goodHits + perfectHits;
            float percentHit = (totalHit / totalNotes) * 100f;

            percentHitText.text = percentHit.ToString("F1") + "%";

            string rankVal = "F";

            if(percentHit > 40)
            {
                rankVal = "D";
                if(percentHit > 55)
                {
                    rankVal = "C";
                    if(percentHit > 70)
                    {
                        rankVal = "B";
                        if(percentHit > 85)
                        {
                            rankVal = "A";
                            if(percentHit > 95)
                            {
                                rankVal = "S";
                            }
                        }
                    }
                }         
            }

            rankText.text = rankVal;

            finalScoreText.text = currentScore.ToString();
        }
    }

    if (currentStage == Stage.Stage1)
    {
        // Stage1이 끝나는 조건을 확인
        if (isStage1Complete)
        {
            TransitionToStage2();
        }
    }
    else if (currentStage == Stage.Stage2)
    {
        // Stage2의 게임 종료 조건을 확인
        if (pass false) // 원하는 조건을 추가하세요
        {
            EndGame();
        }
    }
}*/

