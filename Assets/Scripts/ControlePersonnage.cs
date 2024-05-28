using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlePersonnage : MonoBehaviour
{
    public float vitesseX; //vitesse horizontale actualle
    public float vitesseXMax; //vitesse horizontale d�sir�e
    public float vitesseY; //vitesse verticale 
    public float vitesseSaut; //vitesse de saut d�sir�e

    public bool partieTerminee; // variable pour la fin de la partie

    public bool attaquePossible; //variable pour l'attaque

    // D�claration de la variable public de l�objet son
    public AudioClip sonBlesse; 
    public AudioClip sonPickup; // son de r�cup�ration de l'oeuf
    AudioSource sourceAudio; //Audio

    
    //Image de la barre de vie
    public Image HealthBar;


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
        // d�placement vers la gauche
        if (partieTerminee == false)
        {

            if (Input.GetKey("a"))
            {
                vitesseX = -vitesseXMax;
                GetComponent<SpriteRenderer>().flipX = true; //retourne le sprite
            }
            else if (Input.GetKey("d")) // d�placement vers la droite
            {
                vitesseX = vitesseXMax;
                GetComponent<SpriteRenderer>().flipX = false; //retourne le sprite
            }
            else
            {
                vitesseX = GetComponent<Rigidbody2D>().velocity.x; //m�morise vitesse actualle en X
            }

            // sauter l'objet � l'aide de la touche "w"
            if (Input.GetKeyDown("w") && Physics2D.OverlapCircle(transform.position, 0.5f))
            {
                vitesseY = 30f;
                GetComponent<Animator>().SetBool("saute", true);
            }
            else
            {
                vitesseY = GetComponent<Rigidbody2D>().velocity.y; //m�morise vitesse actualle en Y
            }

            //Applique les vitesses en X et Y
            GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);

            //Gestions des animations de course et repos
            //Active l'animation de course si la vitesse en X est diff�rente de 0, sinon le repos sera jouer par Animator

            if (vitesseX > 0.1 || vitesseX < -0.1)
            {
                GetComponent<Animator>().SetBool("marche", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("marche", false);
            }

            //Gestion de l'attaque
            if (Input.GetKeyDown("space") && attaquePossible == true)
            {
                GetComponent<Animator>().SetBool("attaque", true);
                attaquePossible = false;
                Invoke("FinAttaque", 0.5f);
            } else if (Input.GetKeyUp("space") && attaquePossible == false)
            {
                GetComponent<Animator>().SetBool("attaque", false);
            }


        } 
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //D�sactive l'animation de saut si l'objet touche le sol
        if (Physics2D.OverlapCircle(transform.position, 0.5f))
        {
            GetComponent<Animator>().SetBool("saute", false);
        }

        //Activation de l'animation mort au contact avec un ennemi
        if (collision.gameObject.tag == "ennemis")
        {
            //Attaque l'ennemi
            //Si l'animation d'attaque est activ�e, l'ennemi est d�truit
                if (GetComponent<Animator>().GetBool("attaque") == true)
                {
                    Destroy(collision.gameObject);
                }
                else
                {
                   // Appliquer des d�g�ts au personnage
                   HealthBar.fillAmount -= 0.1f;
                  //Appliquer une couleur rouge au personnage puis il redevient normal
                  GetComponent<SpriteRenderer>().color = Color.red;
                  Invoke("RetourNormal", 0.1f);
                   //Joue le son de blessure
                   sourceAudio.PlayOneShot(sonBlesse,1f);


                    if ( HealthBar.fillAmount == 0)
                    {
                        //D�clenche l'animation de mort
                        GetComponent<Animator>().SetBool("mort", true);

                        //D�sactive les contr�les du personnage lorsqu'il est mort
                        if (transform.position.x > collision.transform.position.x)
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector2(10, 30);
                        }
                        else
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 30);
                        }

                        //Partie termin�e enregistr�e
                        partieTerminee = true;

                        //Fin de la partie, reload la scene
                        Invoke("Recommencer", 2f);
                    }
                }

                
        }

        //R�cup�re un oeuf
        if (collision.gameObject.tag == "Oeuf")
        {
            Destroy(collision.gameObject);

            //Son de r�cup�ration de l'oeuf
            sourceAudio.PlayOneShot(sonPickup, 1f);

            //Affiche le nombre d'oeufs r�cup�r�s
            //Pointage
            OeufsRetrouves.compteurOeufs += 1;

            //Si le nombre d'oeufs est �gal � 10, la partie est gagn�e
            if (OeufsRetrouves.compteurOeufs == 10)
            {
                //Oeuf sont remis � 0
                OeufsRetrouves.compteurOeufs = 0;
                //Change la sc�ne
                SceneManager.LoadScene("finGagne");

            }
        }

    }
    
    //Quand le personnage mange une graine, il gagne de la vie
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "food")
        {
            Destroy(collision.gameObject); //d�truire la graine
            GetComponent<SpriteRenderer>().color = Color.green;
            Invoke("RetourNormal", 0.1f);

            if (HealthBar.fillAmount < 1)
            {
                HealthBar.fillAmount += 0.1f;
            }
        }
    }

    //Recommen�er la partie
    void Recommencer()
    {
        //les oeufs sont remis � 0
        OeufsRetrouves.compteurOeufs = 0;
        //Change la sc�ne
        SceneManager.LoadScene("finMort");
    }

    //Retour � la couleur normale
    void RetourNormal()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    //Animation de fin d'attaque
    void FinAttaque()
    {
        GetComponent<Animator>().SetBool("attaque", false);
        attaquePossible = true;
    }


}