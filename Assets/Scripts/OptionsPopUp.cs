using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsPopUp : MonoBehaviour
{
    [SerializeField]
    public GameObject optionsMenu;

    public static bool isActive = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isActive)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        optionsMenu.SetActive(false);
        Time.timeScale = 1f;
        isActive = false;
    }

    public void Pause()
    {
        optionsMenu.SetActive(true);
        Time.timeScale = 0f;
        isActive = true;
    }
}
