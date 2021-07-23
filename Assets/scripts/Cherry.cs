using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    public void isGot()
    {
        FindObjectOfType<PlayerController>().CherryCount();
        gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}
