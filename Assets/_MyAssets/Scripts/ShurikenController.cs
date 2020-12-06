using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyShuriken", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        //게임오버시 잠시 timeScale을 0으로 만드는 연출이 있는데,
        //이때 수리검의 출현이 끝나지 않았을 경우 효과음이 계속 재생되는 것을 막기 위해 mute처리
        if(Time.timeScale == 0)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.mute = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyController control = collision.gameObject.GetComponent<EnemyController>();
            control.isSlain = true;
        }
    }

    private void DestroyShuriken()
    {
        Destroy(gameObject);
    }
}
