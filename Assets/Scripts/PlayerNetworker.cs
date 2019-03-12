using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerNetworker : MonoBehaviourPunCallbacks, IPunObservable
{
    private DragonMain player;
    public int health = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponent<DragonMain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        print("Sending or receiving" + stream.IsWriting);
        if (stream.IsWriting)
        {
            //stream.SendNext(player.currentHealth);
            stream.SendNext(player);
        }
        else
        {
            // TODO: save some traffic data here (no need to send the whole DragonMain Script, how about sending a json)
            //player.currentHealth = (int)stream.ReceiveNext();
            DragonMain newPlayer = (DragonMain) stream.ReceiveNext();
            player.currentHealth = newPlayer.currentHealth;
            player.currentTail = newPlayer.currentTail;
            player.currentHorns = newPlayer.currentHorns;
            player.currentWings = newPlayer.currentWings;
            player.currentColour = newPlayer.currentColour;
            player.UpdateAttributes();
        }
        health = player.currentHealth;
    }
}
