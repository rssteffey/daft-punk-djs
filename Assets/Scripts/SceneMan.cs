using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (FindObjectsOfType<SceneMan>().Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")){
            SceneManager.LoadScene("DJ_Booth");
        }
        if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene("secondary");
        }
    }
}
