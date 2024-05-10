using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    public Scene SceneToLoad;
    public float TransitionWaitTime = 1;
    public Animator Transition ; 
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneToLoad));
    }

    IEnumerator LoadLevel(Scene ChosenLvl)
    {
        //Play aniamtion
         Transition.SetTrigger("start");

        //Wait for animation to stop
        yield return new WaitForSeconds(TransitionWaitTime);

        //Load Scene
        SceneManager.LoadScene(ChosenLvl.name);
    }
}
