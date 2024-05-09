using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAvecLimites : MonoBehaviour
{
    public GameObject cibleSuivre;

    public float limiteGauche;
    public float limiteDroite;
    public float limiteHaut;
    public float limiteBas;

    // Update is called once per frame
    void Update()
    {
        Vector3 laPosition = cibleSuivre.transform.position;

        if (laPosition.x < limiteGauche)
        {
            laPosition.x = limiteGauche;
        }

        if (laPosition.x > limiteDroite)
        {
            laPosition.x = limiteDroite;
        }

        if (laPosition.y < limiteBas)
        {
            laPosition.y = limiteBas;
        }

        if (laPosition.y > limiteHaut)
        {
            laPosition.y = limiteHaut;
        }

        laPosition.z = -10f;

        transform.position = laPosition;
    }
}
