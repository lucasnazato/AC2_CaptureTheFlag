using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Cinemachine;

public class MyPlayer : MonoBehaviour
{
    public PhotonView view;
    public TMP_Text txtName;
    public TMP_Text txtScore;
    Animator animator;
    Move move;
    Gun gun;
    CapsuleCollider2D collider;
    Rigidbody2D rb;

    CinemachineVirtualCamera cam;

    NetWork2 netWork;

    GameObject[] spawnAcucar;
    GameObject[] spawnOutono;
    public List<RuntimeAnimatorController> lstAnimators;

    public enum Team { TeamAcucar, TeamOutono };

    public Team playerTeam;

    public bool hasFlag = false;

    public GameObject flag;

    void Start()
    {
        txtName.text = view.Owner.NickName;

        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        move = GetComponent<Move>();
        gun = GetComponentInChildren<Gun>();
        collider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        cam = FindObjectOfType<CinemachineVirtualCamera>();

        netWork = FindObjectOfType<NetWork2>();

        spawnAcucar = GameObject.FindGameObjectsWithTag("SpawnAcucarPlayer");
        spawnOutono = GameObject.FindGameObjectsWithTag("SpawnOutonoPlayer");

        object tmp;
        if (view.Owner.CustomProperties.TryGetValue("score", out tmp))
        {
            txtScore.text = tmp.ToString();
        }

        animator.runtimeAnimatorController = lstAnimators[view.Owner.ActorNumber - 1];

        if (view.Owner.ActorNumber == 1 || view.Owner.ActorNumber == 3)
        {
            playerTeam = Team.TeamAcucar;
            transform.position = spawnAcucar[Random.Range(0, spawnAcucar.Length)].transform.position;
        }
        else
        {
            playerTeam = Team.TeamOutono;
            transform.position = spawnOutono[Random.Range(0, spawnOutono.Length)].transform.position;
        }

        if (view.IsMine)
        {
            cam.Follow = transform;
            cam.LookAt = transform;
        }
    }

    private void Update()
    {
        if (!view.IsMine) return;

        if (Input.GetKeyDown(KeyCode.F) && hasFlag)
        {
            flag.transform.parent = null;
            flag.GetComponent<Flag>().pickedUp = false;
            StartCoroutine(ExecuteAfterTime(2));
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            view.RPC("AddScore", RpcTarget.All);
        }
    }

    public void AddScore(int value)
    {
        if (view.IsMine)
        {
            view.RPC("AddScoreNet", RpcTarget.All, value);
        }
    }

    [PunRPC]

    private void AddScoreNet(int value)
    {
        int scoreTmp = (int) view.Owner.CustomProperties["score"];
        scoreTmp += value;

        view.Owner.CustomProperties["score"] = scoreTmp;
        txtScore.text = view.Owner.CustomProperties["score"].ToString();
    }

    public void Die()
    {
        animator.SetTrigger("die");
        move.enabled = false;
        gun.enabled = false;

        collider.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;

        Destroy(gameObject, 2);
    }

    private void OnDestroy()
    {
        if (view.IsMine)
        {
            netWork.CreatePlayer();
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        hasFlag = false;
    }
}
