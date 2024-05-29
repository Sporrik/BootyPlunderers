using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_StartGame : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Island");
    }
}
