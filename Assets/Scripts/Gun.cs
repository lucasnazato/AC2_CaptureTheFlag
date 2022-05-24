using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public float bulletForce = 300;

    PhotonView view;
    SpriteRenderer render;

    MyPlayer player;

    public float timeToShoot = 0.35f;
    float time = 0;

    private void Start()
    {
        view = GetComponentInParent<PhotonView>();
        render = view.GetComponentInParent<SpriteRenderer>();
        player = view.GetComponentInParent<MyPlayer>();
    }

    private void Update()
    {
        if (!view.IsMine)
        {
            return;
        }

        time += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && time > timeToShoot)
        {
            view.RPC("InstatiateBullet", RpcTarget.All);
            time = 0;
        }
    }

    [PunRPC]

    private void InstatiateBullet()
    {
        int dir = (render.flipX) ? -1 : 1;
        Vector2 pos = (render.flipX) ? transform.position - new Vector3(1.8f, 0, 0) : transform.position;

        GameObject tempBullet = Instantiate(bullet, pos, transform.rotation);
        tempBullet.GetComponent<Damage>().player = player;
        tempBullet.GetComponent<Rigidbody2D>().AddForce(dir * transform.right * bulletForce);
    }
}
