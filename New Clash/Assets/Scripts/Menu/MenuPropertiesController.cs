using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPropertiesController : MonoBehaviour
{
    [SerializeField] private GameObject[] menu;
    

    public void SetView(int menuValue)
    {
        for (int i = 0; i < menu.Length; i++)
        {
            menu[i].SetActive(false);
        }
        menu[menuValue].SetActive(true);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
