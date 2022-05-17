using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Flag : MonoBehaviour
{
    GameObject[] spawnAcucar;
    GameObject[] spawnOutono;

    public bool pickedUp = false;
    MyPlayer currentOwner;

    NetWork2 network2;

    private void Start()
    {
        spawnAcucar = GameObject.FindGameObjectsWithTag("SpawnAcucarFlag");
        spawnOutono = GameObject.FindGameObjectsWithTag("SpawnOutonoFlag");

        if (gameObject.tag == "FlagAcucar")
        {
            transform.position = spawnAcucar[Random.Range(0, spawnAcucar.Length)].transform.position;
        }
        else if (gameObject.tag == "FlagOutono")
        {
            transform.position = spawnOutono[Random.Range(0, spawnOutono.Length)].transform.position;
        }

        network2 = GameObject.FindObjectOfType<NetWork2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<MyPlayer>().hasFlag)
            {
                transform.SetParent(collision.transform);
                transform.localPosition = new Vector3(0, 1f, 0);

                if (pickedUp)
                {
                    currentOwner.hasFlag = false;
                }

                pickedUp = true;
                currentOwner = collision.gameObject.GetComponent<MyPlayer>();
                currentOwner.hasFlag = true;
                currentOwner.flag = gameObject;
            }
        }
    }

    private void OnDestroy()
    {
        if (gameObject.tag == "FlagAcucar")
        {
            network2.CreateFlagAcucar();
        }
        else if (gameObject.tag == "FlagOutono")
        {
            network2.CreateFlagOutono();
        }
    }
}
