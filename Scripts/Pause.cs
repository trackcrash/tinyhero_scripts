using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseui;
    //sound
    public AudioSource audioSource;
    public GameObject SelectPanel;
    public GameManager GameManager;
    public GameObject PausePanel;
    

    public void PauseGame()
    {
        pauseui.SetActive(true);
        audioSource.Pause();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseui.SetActive(false);
        audioSource.Play();
        Time.timeScale = 1;
    }
    public void ShowSelect(){
        SelectPanel.SetActive(true);
        PausePanel.SetActive(false);
    }
    public void BackHome(){
        LoadingManager.Instance.LoadScene("Home");
    }
    public void DontsaveHome(){
        GameManager.health = 0;
        GameManager.DeleteSave();
        LoadingManager.Instance.LoadScene("Home");
    }
    public void cancle(){
        SelectPanel.SetActive(false);
        PausePanel.SetActive(true);
    }
    public void TutorialBackHome(){
        LoadingManager.Instance.LoadScene("Home");
        PlayerPrefs.SetInt("Tutorial", 1);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
