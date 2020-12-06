//플레이어를 향해 다가오는 적 오브젝트를 제어하는 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isNormal;

    private GameObject target; //플레이어
    [HideInInspector] public bool isSlain; //죽었는지 판단

    // Start is called before the first frame update
    void Start()
    {

        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if(isSlain)
        {
            StartCoroutine(EnemyDie());
        }
    }

    private void SetTarget()
    {
        if(isNormal)
        {
            float speed = Random.Range(0.8f, 1.2f);


            //플레이어를 타겟으로 설정하고 추격함
            target = GameObject.FindWithTag("Player");

            Vector3 targetVector = target.transform.position - transform.position;

            Rigidbody2D rbd = GetComponent<Rigidbody2D>();
            rbd.velocity = new Vector2(targetVector.normalized.x * speed, targetVector.normalized.y * speed);

            //플레이어를 바라봄
            Vector3 len = transform.position - target.transform.position;
            float angle = Mathf.Atan2(len.x, len.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, -angle);
        }

        else
        {
            float speed = Random.Range(1f, 1.4f);

            //의미없는 곳을 타겟으로 설정하고 추격함
            target = GameObject.FindWithTag("Player");

            Vector3 targetPosition = new Vector3(
                Random.Range(-PlayerPrefs.GetFloat("x-Coordinate"), PlayerPrefs.GetFloat("x-Coordinate")),
                Random.Range(-PlayerPrefs.GetFloat("y-Coordinate"), PlayerPrefs.GetFloat("y-Coordinate")),
                0);

            Vector3 targetVector = targetPosition - transform.position;
            Rigidbody2D rbd = GetComponent<Rigidbody2D>();
            rbd.velocity = new Vector2(targetVector.normalized.x * speed, targetVector.normalized.y * speed);

            //타겟을 바라봄
            Vector3 len = transform.position - targetPosition;
            float angle = Mathf.Atan2(len.x, len.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, -angle);
        }

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    IEnumerator EnemyDie()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        Rigidbody2D rbd = GetComponent<Rigidbody2D>();
        rbd.velocity = new Vector2(-rbd.velocity.x, -rbd.velocity.y);

        CircleCollider2D col = GetComponent<CircleCollider2D>();
        col.enabled = false;
        Animator anim = GetComponent<Animator>();

        anim.SetTrigger("Die");
        isSlain = false;

        yield return new WaitForSeconds(0.25f);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameController controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            controller.GameOver();
        }
    }
}