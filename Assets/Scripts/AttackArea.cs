using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    
    
    
    //Si l'ennemi est dans la zone d'attaque, quand le joueur attaque, l'ennemi est détruit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ennemi")
        {
            //Faire jouer l'annimation d'attaque de DunDun
            GetComponent<Animator>().SetBool("attaque", true);

            Destroy(collision.gameObject);
        }
    }
    
}
