//게임 전반을 제어하는 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int score;
    public Text scoreDisplay;
    public Text readyAndStart;


    public GameObject enemyPrefab;
    public GameObject strangeEnemyPrefab;
    public GameObject itemBubblePrefab;

    public WallController[] walls;
    /*
     * walls[0] : L
     * walls[1] : R
     * walls[2] : T
     * walls[3] : B
     */

    private AudioSource audioSource;
    public AudioClip[] clips;
    /*
     * clips[0] : 적 사망 효과음
     * clips[1] : 게임오버 효과음
     * clips[2] : 카운트 다운 효과음
     * clips[3] : 게임 시작 효과음
     */
    public AudioMixer mixer;
    public AudioMixerSnapshot muteWhooshSnapshot;

    private int numOfItemBubble;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Time.timeScale = 0f;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clips[2];
        for(int t = 3; t > 0; t--)
        {
            readyAndStart.text = t.ToString();
            audioSource.Play();
            yield return new WaitForSecondsRealtime(1.0f);
        }

        audioSource.clip = clips[3];
        audioSource.Play();

        readyAndStart.text = "Start!";
        yield return new WaitForSecondsRealtime(0.8f);
        Time.timeScale = 1f;

        readyAndStart.text = "";

        audioSource.loop = true;
        audioSource.clip = clips[0];
        audioSource.Play();

        score = 0;
        numOfItemBubble = 0;
        StartCoroutine(EnemyManager());
        StartCoroutine(ItemManager());
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0f)
        {
            score += Mathf.CeilToInt(Time.deltaTime);
            scoreDisplay.text = score.ToString();
        }
    }

    //플레이어를 향해 이동하는 적을 num만큼 생성
    private void GenerateNormalEnemy(int num)
    {

        for (int i = 0; i < num; i++)
        {
            Vector3 enemyPosition = new Vector3(
                Random.Range(-PlayerPrefs.GetFloat("x-Coordinate"),
                PlayerPrefs.GetFloat("x-Coordinate")), PlayerPrefs.GetFloat("y-Coordinate") + 1f,
                0);
            GameObject enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
        }
    }

    //엉뚱한 곳을 타겟으로 잡고 이동하는 적을 num만큼 생성
    private void GenerateStrangeEnemy(int num)
    {
        int r;
        Vector3 enemyPosition;
        for (int i = 0; i < num; i++)
        {
            r = Random.Range(0, 4); //0부터 3까지 랜덤으로 r값 생성
            //위쪽에서 출현
            if(r == 0)
            {
                enemyPosition = new Vector3(
                    Random.Range(-PlayerPrefs.GetFloat("x-Coordinate"), PlayerPrefs.GetFloat("x-Coordinate")), 
                    PlayerPrefs.GetFloat("y-Coordinate") + 0.5f, 
                    0);
            }

            //왼쪽에서 출현
            else if(r == 1)
            {
                enemyPosition = new Vector3(
                    -PlayerPrefs.GetFloat("x-Coordinate") - 0.5f,
                    Random.Range(-PlayerPrefs.GetFloat("y-Coordinate"), PlayerPrefs.GetFloat("y-Coordinate")),
                    0);
            }

            //오른쪽에서 출현
            else
            {
                enemyPosition = new Vector3(
                    PlayerPrefs.GetFloat("x-Coordinate") + 0.5f,
                    Random.Range(-PlayerPrefs.GetFloat("y-Coordinate"), PlayerPrefs.GetFloat("y-Coordinate")),
                    0);
            }

            GameObject enemy = Instantiate(strangeEnemyPrefab, enemyPosition, Quaternion.identity);
        }
    }

    //적의 생성을 관리
    IEnumerator EnemyManager()
    {
        float frequency = 1.8f; //다음 적 오브젝트의 등장에 걸리는 시간
        int waveCount = 0; //enemyNumOfThisWave로 진행한 wave의 수
        int enemyNumOfThisWave = 5; //이번 wave에서 등장하는 적 오브젝트의 수
        while (true)
        {
            GenerateNormalEnemy(enemyNumOfThisWave);
            waveCount++;

            //enemyNumOfThisWave가 7이상이면 strangeEnemy도 함께 생성
            if (enemyNumOfThisWave > 7)
            {
                int r = Random.Range(0, 6);
                GenerateStrangeEnemy(r);
            }

            //같은 수의 적이 나오는 wave를 10번이상 버티면 난이도 상승
            if (waveCount == 10)
            {
                //한번에 나오는 normalEnemy 수의 최댓값은 10
                //10을 넘지 않으면 적의 수와 등장 빈도 증가
                if (enemyNumOfThisWave < 10)
                {
                    enemyNumOfThisWave++;
                    frequency -= 0.1f;
                }
                waveCount = 0;
            }

            yield return new WaitForSeconds(frequency);
        }
    }

    private void GenerateItem()
    {
        Vector3 itemBubblePosition = new Vector3(
            Random.Range(-PlayerPrefs.GetFloat("x-Coordinate"), PlayerPrefs.GetFloat("x-Coordinate")), 
            PlayerPrefs.GetFloat("y-Coordinate") + 0.5f,
            0);
        GameObject shuriken = Instantiate(itemBubblePrefab, itemBubblePosition, Quaternion.identity);
    }

    IEnumerator ItemManager()
    {
        
        while(true)
        {
            if (numOfItemBubble < 3)
            {
                GenerateItem();
                numOfItemBubble++;
            }
            yield return new WaitForSeconds(5f);
        }
       
    }

    //ItemContoller에서 아이템버블을 획득했을 경우 numOfItemBubble감소
    public void AcquireItem()
    {
        numOfItemBubble--;
    }

    public void GameOver()
    {
        audioSource.clip = clips[1];
        audioSource.loop = false;
        audioSource.Play();

        PlayerController controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        controller.isDied = true;

        Time.timeScale = 0f;
        PlayerPrefs.SetInt("CurrentScore",score);

        StartCoroutine(MoveToGameOverScene());
    }

    IEnumerator MoveToGameOverScene()
    {
        PlayerPrefs.SetInt("CurrentScore", score);

        audioSource.clip = clips[1];
        audioSource.loop = false;
        audioSource.Play();

        PlayerController controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        controller.isDied = true;

        Invoke("MoveToGameOverScene", 0.5f);

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;

        SceneManager.LoadScene("GameOver");
    }

}
