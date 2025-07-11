using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    [Header("Kéo prefab nhân vật vào đây (nằm trong Resources/)")]
    public GameObject playerPrefab;

    [Header("Vị trí spawn người chơi")]
    public Transform[] spawnPositions; // Kéo các vị trí spawn vào đây trong Inspector

    private void Start()
    {
        Debug.Log("🔌 Đang kết nối đến Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("✅ Kết nối thành công. Tham gia lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("✅ Vào lobby thành công. Tạo hoặc tham gia phòng...");
        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("MyRoom", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("✅ Đã vào phòng!");
        Debug.Log("⭐ MasterClient: " + PhotonNetwork.IsMasterClient);

        // Tính vị trí spawn theo số người trong phòng
        int playerIndex = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        Vector3 spawnPos = (spawnPositions != null && playerIndex < spawnPositions.Length)
            ? spawnPositions[playerIndex].position
            : Vector3.zero;

        // Spawn nhân vật
        if (playerPrefab != null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogError("❌ Chưa kéo prefab nhân vật vào PhotonLauncher!");
        }
    }
}
