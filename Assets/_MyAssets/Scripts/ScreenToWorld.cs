//해상도를 기준으로 너비와 폭을 월드좌표계로 변환해둠

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenToWorld : MonoBehaviour
{
    public Camera cam;
    public Vector3 worldCoordinate;

    // Start is called before the first frame update
    void Awake()
    {
        worldCoordinate = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        PlayerPrefs.SetFloat("x-Coordinate", worldCoordinate.x);
        PlayerPrefs.SetFloat("y-Coordinate", worldCoordinate.y);
    }
}
