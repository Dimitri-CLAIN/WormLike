 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class js : MonoBehaviour
{
    public Material firstStade;
    public Material secondStade;
    public Material thirdStade;
    public int stade;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Explosion")
            return;
        if (stade == 0) {
            GetComponent<Renderer>().material = firstStade;
            stade++;
        } else if (stade == 1) {
            GetComponent<Renderer>().material = secondStade;
            stade++;
        } else if (stade == 2) {
            GetComponent<Renderer>().material = thirdStade;
            stade++;
        } else {
            Destroy(gameObject);
        }
    }
}