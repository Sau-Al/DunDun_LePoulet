using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlePersonnage : MonoBehaviour
{
    public float vitesseX; //vitesse horizontale actualle
    public float vitesseXMax; //vitesse horizontale d�sir�e
    public float vitesseY; //vitesse verticale 
    public float vitesseSaut; //vitesse de saut d�sir�e

    //D�claration des variables pour le texte
    public TextMeshProUGUI textFinDuJeu;
    public TextMeshProUGUI textPointage; // texte du pointage
    float compteur;

    public bool partieTerminee; // variable pour la fin de la partie

    public bool attaquePossible; //variable pour l'attaque

    AudioSource sourceAudio; //Audio

    // Start is called before the first frame update
    void Start()
    {
        sourceAudio = GetComponent<AudioSource>(); //Initialise le composant audio
        partieTerminee = false; //Initialise la variable partieTerminee
        attaquePossible = true; //Initialise la variable attaquePossible
        textFinDuJeu.GetComponent<TextMeshProUGUI>().fontSize = 0; //va chercher le texte
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
            //D�sactive l'animation de saut si l'objet touche le sol
            if (Physics2D.OverlapCircle(transform.position, 0.5f))
            {
                GetComponent<Animator>().SetBool("saute", false);
            }

            //Activation de l'animation mort au contact avec un ennemi
            if (collision.gameObject.name == "Renard")
            {
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

            //R�cup�re un oeuf
            if (collision.gameObject.name == "Oeuf")
            {
                Destroy(collision.gameObject);

            //Son de r�cup�ration de l'oeuf
            //GetComponent<AudioSource>().Play();

            //Affiche le nombre d'oeufs r�cup�r�s
            //Pointage
            compteur = compteur + 1f;
            UpdatePointage();
        }

        }
        //Recommen�er la partie
        void Recommencer()
        {
            SceneManager.LoadScene("finMort");
        }

        //Compteur des oeufs
        void UpdatePointage()
        {
        textPointage.text = "Pointage: " + compteur.ToString();
        }

        //Si le poulet tombe dans le vide
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Vide")
            {
                //D�clenche l'animation de mort
                GetComponent<Animator>().SetBool("mort", true);

                //Recommence la partie
                Invoke("Recommencer", 2f);
            }
        }
}   
