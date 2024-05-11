using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OeufsRetrouves : MonoBehaviour
{
    public Text nombreOeufsRetrouves;
    public static int compteurOeufs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nombreOeufsRetrouves.text = "Oeufs: " + Mathf.Round(compteurOeufs) + "/10";
    }

    public void Addscore(int score)
    {
        compteurOeufs += score;
    }
}
