using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Move : MonoBehaviourPun, IPunObservable
{
    public float speed;
 
    Rigidbody2D rb;
    PhotonView view;
    Animator animator;
    SpriteRenderer rend;

    float hor, ver;
    Vector2 direction;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!view.IsMine)
        {
            return;
        }

        hor = Input.GetAxis("Horizontal");
        direction = new Vector2(hor * speed, rb.velocity.y);

        if (hor > 0)
        {
            rend.flipX = false;
        } else if (hor < 0)
        {
            rend.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = direction;
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rend.flipX);
        }
        else
        {
            rend.flipX = (bool)stream.ReceiveNext();
        }
    }
}
