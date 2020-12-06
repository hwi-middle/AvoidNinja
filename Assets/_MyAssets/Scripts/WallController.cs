//모든 해상도에서 상하좌우의 콜라이더를 배치할 수 있도록 하는 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public char wallType;  /* 어느 위치의 벽인지 지정
                            * L: 왼쪽 벽
                            * R: 오른쪽 벽
                            * T: 위쪽 벽
                            * B: 아래쪽 벽
                            */

    // Start is called before the first frame update
    void Start()
    {
        AdjustWalls();
    }

    private void AdjustWalls()
    {
        BoxCollider2D bcd = GetComponent<BoxCollider2D>();

        //각자의 위치로 이동
        if (wallType == 'L')
        {
            transform.position = new Vector3((-PlayerPrefs.GetFloat("x-Coordinate")) - (bcd.size.x / 2), 0, 0);
        }

        else if (wallType == 'R')
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("x-Coordinate") + (bcd.size.x / 2), 0, 0);
        }

        else if (wallType == 'T')
        {
            transform.position = new Vector3(0, PlayerPrefs.GetFloat("y-Coordinate") + (bcd.size.y / 2), 0);
        }

        else if (wallType == 'B')
        {
            transform.position = new Vector3(0, (-PlayerPrefs.GetFloat("y-Coordinate")) - (bcd.size.y / 2), 0);
        }

        //콜라이더 크기 조절
        if (wallType == 'L' || wallType == 'R')
        {
            bcd.size = new Vector2(0.16f, 2 * PlayerPrefs.GetFloat("y-Coordinate"));
        }

        else if (wallType == 'T' || wallType == 'B')
        {
            bcd.size = new Vector2(2 * PlayerPrefs.GetFloat("x-Coordinate"), 0.16f);
        }

        bcd.enabled = true;
    }

}
