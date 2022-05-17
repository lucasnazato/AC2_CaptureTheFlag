using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Jump : MonoBehaviour
{
    public float force;
    public GameObject foot;
    public LayerMask mask;

    Rigidbody2D rb;
    PhotonView view;
    Animator animator;

    RaycastHit2D hit;
    public bool isGrounded = false;
    public float maxDistance = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!view.IsMine)
        {
            return;
        }

        hit = Physics2D.CircleCast(foot.transform.position, .6f, Vector2.down, maxDistance);

        if (hit.collider && !isGrounded)
        {
            isGrounded = true;
        } 
        else if (!hit.collider && isGrounded)
        {
            isGrounded = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(transform.up * force);
        }
    }

    private void OnDrawGizmos()
    {
        hit = Physics2D.CircleCast(foot.transform.position, .6f, Vector2.down, maxDistance);

        if (hit.collider)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(foot.transform.position + (Vector3.down) * hit.distance, .6f);
        }
        
    }
}
