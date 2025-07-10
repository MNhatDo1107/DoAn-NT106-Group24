using UnityEngine;
using TMPro;
using Photon.Pun;
public class UserName : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject UserNamePage;
    public TMP_Text MyUsername;
    void Start()
    {
        if (PlayerPrefs.GetString("UserName") == "" || PlayerPrefs.GetString("UserName") == null)
        {
            UserNamePage.SetActive(false);
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("UserName");

            MyUsername.text = PlayerPrefs.GetString("UserName");

            UserNamePage.SetActive(false);
        }
    }

    public void SaveUserName()
    {
        PhotonNetwork.NickName = inputField.text;

        PlayerPrefs.SetString("UserName", inputField.text);

        MyUsername.text = inputField.text;

        UserNamePage.SetActive(false);
    }
}
