using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                            print("Team Acucar Won");
                        }
                    }

                    break;
                case "TeamOutonoArea":

                    if (player.playerTeam == MyPlayer.Team.TeamOutono && player.hasFlag)
                    {
                        if (player.flag.CompareTag("FlagAcucar"))
                        {
                            print("Team Outono Won");
                        }
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
