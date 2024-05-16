using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlePersonnage : MonoBehaviour
{
    public float vitesseX; //vitesse horizontale actualle
    public float vitesseXMax; //vitesse horizontale désirée
    public float vitesseY; //vitesse verticale 
    public float vitesseSaut; //vitesse de saut désirée

    public bool partieTerminee; // variable pour la fin de la partie

    public bool attaquePossible; //variable pour l'attaque

    AudioSource sourceAudio; //Audio

    public float health = 100f; //Vie du personnage
    private int MAX_HEALTH = 100; //Vie maximale du personnage

    // Start is called before the first frame update
    void Start()
    {
        sourceAudio = GetComponent<AudioSource>(); //Initialise le composant audio
        partieTerminee = false; //Initialise la variable partieTerminee
        attaquePossible = true; //Initialise la variable attaquePossible
    }

    // Update is called once per frame
    void Update()
    {
        // déplacement vers la gauche
        if (partieTerminee == false)
        {

            //Debug pour le dommage et la guérison
            if (Input.GetKeyDown(KeyCode.I))
            {
                Damage(10);
            }
            //guerison
            if (Input.GetKeyDown(KeyCode.O))
            {
                Heal(10);
            }





            if (Input.GetKey("a"))
            {
                vitesseX = -vitesseXMax;
                GetComponent<SpriteRenderer>().flipX = true; //retourne le sprite
            }
            else if (Input.GetKey("d")) // déplacement vers la droite
            {
                vitesseX = vitesseXMax;
                GetComponent<SpriteRenderer>().flipX = false; //retourne le sprite
            }
            else
            {
                vitesseX = GetComponent<Rigidbody2D>().velocity.x; //mémorise vitesse actualle en X
            }

            // sauter l'objet à l'aide de la touche "w"
            if (Input.GetKeyDown("w") && Physics2D.OverlapCircle(transform.position, 0.5f))
            {
                vitesseY = 30f;
                GetComponent<Animator>().SetBool("saute", true);
            }
            else
            {
                vitesseY = GetComponent<Rigidbody2D>().velocity.y; //mémorise vitesse actualle en Y
            }

            //Applique les vitesses en X et Y
            GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);

            //Gestions des animations de course et repos
            //Active l'animation de course si la vitesse en X est différente de 0, sinon le repos sera jouer par Animator

            if (vitesseX > 0.1 || vitesseX < -0.1)
            {
                GetComponent<Animator>().SetBool("marche", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("marche", false);
            }

            //Gestion de l'attaque
            if (Input.GetKeyDown(KeyCode.Space) && GetComponent<Animator>().GetBool("saute") == false)
            {
                GetComponent<Animator>().SetBool("attaque", true);
                attaquePossible = false;
                Invoke("AnnuleAttaque", 0.4f);
            }
            else
            {
                GetComponent<Animator>().SetBool("attaque", false);
            }
            if (attaquePossible == false && vitesseX <= vitesseXMax)
            {
                vitesseX = vitesseX + 5f;
            }

        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Désactive l'animation de saut si l'objet touche le sol
        if (Physics2D.OverlapCircle(transform.position, 0.5f))
        {
            GetComponent<Animator>().SetBool("saute", false);
        }

        //Activation de l'animation mort au contact avec un ennemi
        if (collision.gameObject.name == "Renard")
        {
            if ((GetComponent<Animator>().GetBool("attaque") == true))
            {
                //Détruire l'ennemi
                Destroy(collision.gameObject); 
            }


            if ((GetComponent<Animator>().GetBool("attaque") == false))
            {
                  //Déclenche l'animation de mort
                  GetComponent<Animator>().SetBool("mort", true);

                  //Désactive les contrôles du personnage lorsqu'il est mort
                  if (transform.position.x > collision.transform.position.x)
                  {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(10, 30);
                  }
                  else
                  {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 30);
                  }

                  //Partie terminée enregistrée
                  partieTerminee = true;

                  //Fin de la partie, reload la scene
                  Invoke("Recommencer", 2f);
            
            }
                
        }

        //Récupère un oeuf
        if (collision.gameObject.tag == "Oeuf")
        {
            Destroy(collision.gameObject);

            //Son de récupération de l'oeuf
            //GetComponent<AudioSource>().Play();

            //Affiche le nombre d'oeufs récupérés
            //Pointage
            OeufsRetrouves.compteurOeufs += 1;

            //Si le nombre d'oeufs est égal à 10, la partie est gagnée
            if (OeufsRetrouves.compteurOeufs == 10)
            {
                SceneManager.LoadScene("finGagne");
            }
        }

    }
    //Si le poulet tombe dans le vide
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Vide")
        {
            //Déclenche l'animation de mort
            GetComponent<Animator>().SetBool("mort", true);

            //Recommence la partie
            Invoke("Recommencer", 2f);
        }
    }

    //Recommençer la partie
    void Recommencer()
    {
        SceneManager.LoadScene("finMort");
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentException("Amount must be positive");
        }

        this.health -= amount;

        if (health <= 0)
        {
            Recommencer();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentException("Amount must be positive");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            this.health =  MAX_HEALTH;
        } else {
        this.health += amount; 
        }

        
    }


}