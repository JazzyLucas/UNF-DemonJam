using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void GoToDiff()
    {
        SceneManager.LoadScene("Difficulty");
    }

    public void GoToOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void QuitG()
    {
        Application.Quit();
    }

    public void Back()
    {
        SceneManager.LoadScene("Start");
    }
}
