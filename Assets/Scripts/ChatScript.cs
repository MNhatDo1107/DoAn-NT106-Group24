using UnityEngine;
using Photon.Pun;
using TMPro;

public class ChatManager : MonoBehaviourPun
{
    public TMP_InputField chatInput;
    public GameObject messagePrefab;  // Prefab with a TMP_Text component
    public Transform contentTransform;

    private bool isTyping = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isTyping)
        {
            chatInput.ActivateInputField();
            isTyping = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return) && isTyping)
        {
            if (!string.IsNullOrWhiteSpace(chatInput.text))
            {
                SendChatMessage(chatInput.text.Trim());
            }
            chatInput.text = "";
            chatInput.DeactivateInputField();
            isTyping = false;
        }
    }

    void SendChatMessage(string message)
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Not connected to Photon. Cannot send message.");
            return;
        }

        photonView.RPC("ReceiveChatMessage", RpcTarget.All, PhotonNetwork.NickName, message);
    }


    [PunRPC]
    void ReceiveChatMessage(string sender, string message)
    {
        GameObject msgObj = Instantiate(messagePrefab, contentTransform);
        TMP_Text msgText = msgObj.GetComponent<TMP_Text>();

        if (sender == PhotonNetwork.NickName)
            msgText.text = $"<color=#00FF00>{sender}:</color> {message}";
        else
            msgText.text = $"<color=#FFFFFF>{sender}:</color> {message}";
    }
}
