using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string sceneName = "OutdoorsScene"; 
    public void OpenScene()
    {
        SceneManager.LoadScene(sceneName); 
    }
}
