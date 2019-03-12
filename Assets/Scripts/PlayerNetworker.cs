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
        if (stream.IsWriting)
        {
            //stream.SendNext(player.currentHealth);
            stream.SendNext(JsonUtility.ToJson(player));
        }
        else
        {
            // TODO: save some traffic data here (no need to send the whole DragonMain Script, how about sending a json)
            //player.currentHealth = (int)stream.ReceiveNext();
            string playerData = (string) stream.ReceiveNext();
            JsonUtility.FromJsonOverwrite(playerData, player);
            player.UpdateAttributes();
        }
        health = player.currentHealth;
    }
}
