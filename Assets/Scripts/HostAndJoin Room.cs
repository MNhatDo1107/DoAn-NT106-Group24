using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class HostAndJoinRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField input_Host;
    public TMP_InputField input_Join;

    private bool isReady = false;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to Master Server, joining lobby...");
    }

    public override void OnJoinedLobby()
    {
        isReady = true;
        Debug.Log("Joined Lobby, ready to create or join rooms.");
    }

    public void HostRoom()
    {
        if (!isReady)
        {
            Debug.LogError("Not ready to create room. Wait for lobby connection.");
            return;
        }

        PhotonNetwork.CreateRoom(input_Host.text, new RoomOptions() { MaxPlayers = 2, IsVisible = true, IsOpen = true });
    }

    public void JoinRoom()
    {
        if (!isReady)
        {
            Debug.LogError("Not ready to join room. Wait for lobby connection.");
            return;
        }

        PhotonNetwork.JoinRoom(input_Join.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room Successfully, loading Lobby scene...");
        SceneManager.LoadScene("Lobby");
    }
}
