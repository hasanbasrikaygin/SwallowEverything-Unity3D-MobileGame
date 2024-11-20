using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class returnhome : MonoBehaviour
{
    public Button butonl;
    void Start()
    {
        butonl.onClick.AddListener(OnTurnHomeButtonDown);

    }
    void OnTurnHomeButtonDown()
    {
        SceneManager.LoadScene("Home");
    }
}
