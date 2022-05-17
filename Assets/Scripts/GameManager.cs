using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject flagAcucarPrefab;
    public GameObject flagOutonoPrefab;
    GameObject flagAcucar;
    GameObject flagOutono;

    private void Start()
    {
        PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity);

        flagAcucar = GameObject.FindGameObjectWithTag("FlagAcucar");
        flagOutono = GameObject.FindGameObjectWithTag("FlagOutono");

        if (flagAcucar == null)
        {
            PhotonNetwork.Instantiate(flagAcucarPrefab.name, new Vector3(0, -3, 0), Quaternion.identity);
        }

        if (flagOutono == null)
        {
            PhotonNetwork.Instantiate(flagOutonoPrefab.name, new Vector3(0, -3, 0), Quaternion.identity);
        }
    }
}
