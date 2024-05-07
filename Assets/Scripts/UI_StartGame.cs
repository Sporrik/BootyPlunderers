using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_StartGame : MonoBehaviour
{
    [SerializeField] private string newGameLevel = "Level1";
    public void NewButton()
    { SceneManager.LoadScene(newGameLevel); }
}
