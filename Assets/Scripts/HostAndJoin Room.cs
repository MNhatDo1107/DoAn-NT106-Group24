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
        // Gán NickName nếu chưa có
        if (string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            string savedName = PlayerPrefs.GetString("UserName", "");
            if (!string.IsNullOrEmpty(savedName))
            {
                PhotonNetwork.NickName = savedName;
            }
            else
            {
                Debug.LogWarning("Username not set. Please enter your username first.");
            }
        }

        // Bắt đầu kết nối Photon
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("✅ Connected to Master Server, joining lobby...");
    }

    public override void OnJoinedLobby()
    {
        isReady = true;
        Debug.Log("✅ Joined Lobby, ready to create or join rooms.");
    }

    public void HostRoom()
    {
        if (!isReady)
        {
            Debug.LogError("❌ Not ready to create room. Wait for lobby connection.");
            return;
        }

        if (string.IsNullOrEmpty(input_Host.text))
        {
            Debug.LogWarning("Please enter a room name.");
            return;
        }

        PhotonNetwork.CreateRoom(input_Host.text, new RoomOptions() { MaxPlayers = 2, IsVisible = true, IsOpen = true });
    }

    public void JoinRoom()
    {
        if (!isReady)
        {
            Debug.LogError("❌ Not ready to join room. Wait for lobby connection.");
            return;
        }

        if (string.IsNullOrEmpty(input_Join.text))
        {
            Debug.LogWarning("Please enter a room name to join.");
            return;
        }

        PhotonNetwork.JoinRoom(input_Join.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"✅ Joined Room: {PhotonNetwork.CurrentRoom.Name} as {PhotonNetwork.NickName}, loading Lobby scene...");
        SceneManager.LoadScene("Lobby");
    }
}
