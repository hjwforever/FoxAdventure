using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public void isGot()
    {
        FindObjectOfType<PlayerController>().GemCount();
        gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}
