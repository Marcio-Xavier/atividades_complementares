using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
       //Player player = collision.gameObject.GetComponent<Player>();
       // if(!player) return;

        if (collision.gameObject.CompareTag("Caixa") && Player.isAttaking )
        {

           ++Player.BoxCount;
            Destroy(collision.gameObject);

        }
    }
}
