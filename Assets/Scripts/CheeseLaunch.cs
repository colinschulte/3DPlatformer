using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseLaunch : MonoBehaviour
{
    public GameObject glove;
    public GameObject iceCheese;
    public GameObject burner;
    
    public void Punch()
    {
        Vector3 glovePosition = glove.transform.position;
        glovePosition.x += 1.5f;
        glove.transform.position = glovePosition;
        Vector3 cheesePosition = burner.transform.position;
        cheesePosition.y += 2f;
        iceCheese.transform.position = cheesePosition;
        iceCheese.GetComponent<MeshRenderer>().enabled = false;
        iceCheese.GetComponent<BoxCollider>().enabled = false;
        GameObject cheese = iceCheese.transform.GetChild(0).gameObject;
        cheese.GetComponent<Animator>().enabled = true;
        cheese.GetComponent<SphereCollider>().enabled = true;

    }
}
