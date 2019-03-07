using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Player playerScript;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if(playerScript != null)
            {
              playerScript.DamagePlayer();
            }
        }
        if (collision.CompareTag("Water"))
        {
            Debug.Log("agua");
            playerScript.DamagePlayerWater();
        }


    }
}
