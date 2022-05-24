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

    Vector3 initialPosition;

    bool respawn = true;

    private void Awake()
    {
        network2 = GameObject.FindObjectOfType<NetWork2>();

        GameObject existingFlag = GameObject.FindGameObjectWithTag(gameObject.tag);

        if (existingFlag != null && existingFlag != gameObject)
        {
            respawn = false;
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        spawnAcucar = GameObject.FindGameObjectsWithTag("SpawnAcucarFlag");
        spawnOutono = GameObject.FindGameObjectsWithTag("SpawnOutonoFlag");

        if (gameObject.tag == "FlagAcucar")
        {
            transform.position = spawnAcucar[Random.Range(0, spawnAcucar.Length)].transform.position;
            initialPosition = spawnAcucar[Random.Range(0, spawnAcucar.Length)].transform.position;
        }
        else if (gameObject.tag == "FlagOutono")
        {
            transform.position = spawnOutono[Random.Range(0, spawnOutono.Length)].transform.position;
            initialPosition = spawnOutono[Random.Range(0, spawnOutono.Length)].transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MyPlayer player = collision.gameObject.GetComponent<MyPlayer>();

            if ((gameObject.tag == "FlagAcucar" && player.playerTeam == MyPlayer.Team.TeamAcucar) || (gameObject.tag == "FlagOutono" && player.playerTeam == MyPlayer.Team.TeamOutono))
            {
                ReturnToInitialPosition();
            }
            else
            {
                if (!player.hasFlag)
                {
                    transform.SetParent(collision.transform);
                    transform.localPosition = new Vector3(0.5f, 0.5f, 0);

                    transform.rotation = Quaternion.Euler(0, 0, 270);

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
    }

    private void OnDestroy()
    {
        if (gameObject.tag == "FlagAcucar" && respawn)
        {
            network2.CreateFlagAcucar();
        }
        else if (gameObject.tag == "FlagOutono" && respawn)
        {
            network2.CreateFlagOutono();
        }
    }

    public void ReturnToInitialPosition()
    {
        if (pickedUp)
        {
            currentOwner.hasFlag = false;
        }

        Destroy(gameObject);
    }
}
