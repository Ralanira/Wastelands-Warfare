using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private AudioSource audioData;

    public void LoadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            audioData.Play(0);   
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));       
        }
        else 
        {
            audioData.Play(0);
            StartCoroutine(LoadLevel(0));
        }
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

}
