using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinVolume : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MyPlayer player = collision.gameObject.GetComponent<MyPlayer>();
            switch (gameObject.tag)
            {
                case "TeamAcucarArea":

                    if (player.playerTeam == MyPlayer.Team.TeamAcucar && player.hasFlag)
                    {
                        if (player.flag.CompareTag("FlagOutono")) 
                        {
                            player.AddTeamAcucar(1);
                            player.flag.transform.parent = null;
                            player.flag.GetComponent<Flag>().ReturnToInitialPosition();
                            player.hasFlag = false;

                            print("Acucar");
                        }
                    }

                    break;
                case "TeamOutonoArea":

                    if (player.playerTeam == MyPlayer.Team.TeamOutono && player.hasFlag)
                    {
                        if (player.flag.CompareTag("FlagAcucar"))
                        {
                            player.AddTeamOutono(1);
                            player.flag.transform.parent = null;
                            player.flag.GetComponent<Flag>().ReturnToInitialPosition();
                            player.hasFlag = false;

                            print("Outono");
                        }
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
