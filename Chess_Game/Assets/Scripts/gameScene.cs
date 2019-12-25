using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This Script is used for loading Scenes from Game Scene.

public class gameScene : MonoBehaviour
{
    public void quitGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
