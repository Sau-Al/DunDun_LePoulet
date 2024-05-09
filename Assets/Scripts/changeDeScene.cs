using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeDeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Si le joueur appuie sur "space", la scène "DunDun" est chargée
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("DunDun");
        }

        //Si le jouer appuie sur "escape", le jeu se ferme
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
