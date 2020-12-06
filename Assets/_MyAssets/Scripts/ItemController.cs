//아이템 버블을 제어하는 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject target; //플레이어
    public GameObject shurikenPrefab;
    public float speed;

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //아이템 버블이 플레이어에 닿으면 수리검을 생성하고 자신은 사라짐
        if (collision.gameObject.tag == "Player")
        {
            GameObject shuriken = Instantiate(shurikenPrefab, transform.position, Quaternion.identity);
            gameController.AcquireItem();
            Destroy(gameObject);
        }
        //벽에 닿으면 다시 플레이어를 향해 다가감
        else if(collision.gameObject.tag=="Walls")
        {
            SetTarget();
        }
    }

    private void SetTarget()
    {
        //플레이어 방향으로 날아옴
        target = GameObject.FindWithTag("Player");
        Vector3 targetVector = target.transform.position - transform.position;

        Rigidbody2D rbd = GetComponent<Rigidbody2D>();
        rbd.velocity = new Vector2(targetVector.normalized.x * speed, targetVector.normalized.y * speed);
    }
}
