using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This is the Script used to load Scene from Menu to Gameplay Scene.
public class menuScene : MonoBehaviour
{
   public void playStart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
