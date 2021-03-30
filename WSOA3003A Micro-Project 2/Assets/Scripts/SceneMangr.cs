using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMangr : MonoBehaviour
{
    public void LoadNewScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game Scene");
    }
}
