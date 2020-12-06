using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("MoveToMainScene",3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
