using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Cinemachine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MyPlayer : MonoBehaviourPun, IPunObservable
{
    public PhotonView view;
    public TMP_Text txtName;
    public TMP_Text txtScore;
    Animator animator;
    Move move;
    Gun gun;
    CapsuleCollider2D colPlayer;
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

    public TMP_Text txtAcucarScore;
    public TMP_Text txtOutonoScore;

    void Start()
    {
        txtName.text = view.Owner.NickName;

        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        move = GetComponent<Move>();
        gun = GetComponentInChildren<Gun>();
        colPlayer = GetComponent<CapsuleCollider2D>();
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

        
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("scoreAcucar", out tmp))
        {
            txtAcucarScore.text = tmp.ToString();
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("scoreOutono", out tmp))
        {
            txtOutonoScore.text = tmp.ToString();
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

        if (Input.GetKeyDown(KeyCode.M))
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "scoreAcucar", 3} });
        }

        txtAcucarScore.text = "X " + PhotonNetwork.CurrentRoom.CustomProperties["scoreAcucar"].ToString();
        txtOutonoScore.text = "X " + PhotonNetwork.CurrentRoom.CustomProperties["scoreOutono"].ToString();
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

    public void AddTeamAcucar(int value)
    {
        if (view.IsMine)
        {
            int scoreTmp = (int)PhotonNetwork.CurrentRoom.CustomProperties["scoreAcucar"];
            scoreTmp += value;

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "scoreAcucar", scoreTmp } });
        }
    }

    public void AddTeamOutono(int value)
    {
        if (view.IsMine)
        {
            int scoreTmp = (int)PhotonNetwork.CurrentRoom.CustomProperties["scoreOutono"];
            scoreTmp += value;

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "scoreOutono", scoreTmp } });
        }
    }

    public void Die()
    {
        animator.SetTrigger("die");
        move.enabled = false;
        gun.enabled = false;

        colPlayer.enabled = false;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       // throw new System.NotImplementedException();
    }
}
