using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLoadScene : MonoBehaviour
{
    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("MenuPrincipal");
        }
    }
}
