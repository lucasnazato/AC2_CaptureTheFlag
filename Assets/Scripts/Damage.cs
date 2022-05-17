using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Damage : MonoBehaviour
{
    public MyPlayer player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health;

        if (collision.TryGetComponent<Health>(out health))
        {
            player.AddScore(1);
            health.UpdateHealth(-10);
            Destroy(gameObject);
        }
    }
}
