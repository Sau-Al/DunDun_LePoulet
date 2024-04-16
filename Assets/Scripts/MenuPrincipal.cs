using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipale : MonoBehaviour
{
   public void Jouer()
    {
        SceneManager.LoadScene("DunDun");
    }

    public void Quitter()
    {
        Application.Quit();
    }
}
