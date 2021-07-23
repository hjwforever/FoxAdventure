using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnDialog : MonoBehaviour
{
    public GameObject returnDialog;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            returnDialog.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            returnDialog.SetActive(false);
        }
    }
}
