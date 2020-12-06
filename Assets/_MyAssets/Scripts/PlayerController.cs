//플레이어의 조작에 따라 이동과 회전을 담당하는 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    [HideInInspector] public bool isDied;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDied)
        {
            Vector2 vector;

            //에디터 환경과 안드로이드 환경 구분
            if (Application.isEditor)
            {
                speed = 2f;
                vector.x = Input.GetAxis("Horizontal");
                vector.y = Input.GetAxis("Vertical");
            }

            else
            {
                speed = 6f;
                vector.x = Input.acceleration.x;
                vector.y = Input.acceleration.y;
            }

            Rigidbody2D rbd = GetComponent<Rigidbody2D>();

            //입력에 의해 얻어진 벡터에 speed를 곱하여 velocity에 대입
            rbd.velocity = vector * speed;

            //transform.up에 vector을 대입하여 진행방향으로 회전시킴
            transform.up = vector.normalized;
        }

    }

}
