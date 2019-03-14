using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    #region Public Fields

    public static MultiplayerManager inst;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        inst = this;
        
    }

    #endregion
    
    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        print("Loading Scene 0");
        SceneManager.LoadScene(0);
    }


    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        print("Trying to leave room");
        FireballScript[] fireballsOfPlayer = Game.player.GetComponent<Attack>().fireballList.ToArray();
        foreach (FireballScript fireballScript in fireballsOfPlayer)
        {
            Destroy(fireballScript.gameObject);
        }
        PhotonNetwork.Destroy(Game.player.gameObject);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("Player {0} entered the room", other.NickName); // not seen if you're the player connecting


        /*if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }*/
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("Player {0} left the room", other.NickName); // seen when other disconnects

        /*if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }*/
    }


    #endregion
}
