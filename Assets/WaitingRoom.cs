using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject waitingRoomPanel;
    public Transform playerListContainer;
    public GameObject playerNamePrefab;
    public Button startButton;
    public GameObject gameplayRoot; // Phần gameplay tổng thể, có thể disable khi chờ (nếu muốn)

    private void Start()
    {
        waitingRoomPanel.SetActive(true);

        if (gameplayRoot != null)
            gameplayRoot.SetActive(false);  // Ẩn gameplay khi đang chờ

        UpdatePlayerList();
        CheckStartButton();
    }

    public  void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        CheckStartButton();
    }

    public  void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
        CheckStartButton();
    }

    void UpdatePlayerList()
    {
        foreach (Transform child in playerListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            GameObject obj = Instantiate(playerNamePrefab, playerListContainer);
            obj.GetComponent<TMP_Text>().text = player.NickName;
        }
    }

    void CheckStartButton()
    {
        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void OnClickStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartGameForAll", RpcTarget.All);
        }
    }

    [PunRPC]
    void StartGameForAll()
    {
        waitingRoomPanel.SetActive(false);

        if (gameplayRoot != null)
            gameplayRoot.SetActive(true);
    }

}
