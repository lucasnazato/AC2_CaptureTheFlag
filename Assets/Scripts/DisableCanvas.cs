using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class DisableCanvas : MonoBehaviour
{
    PhotonView view;

    public GameObject scoreCanvas;

    private void Start()
    {
        view = GetComponentInParent<PhotonView>();

        if (!view.IsMine)
        {
            scoreCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0, 180, 0));
    }
}
