using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    public int currentEnergy;

    [SerializeField] private int energyThreshold = 3;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject enemySpawner;
    private bool bossCalled = false;

    void Start()
    {
        boss.SetActive(false);
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void AddEnergy()
    {
        if (bossCalled)
        {
            return;
        }

        currentEnergy += 1;

        if (currentEnergy == energyThreshold)
        {
            photonView.RPC("CallBossRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    private void CallBossRPC()
    {
        bossCalled = true;
        boss.SetActive(true);
        enemySpawner.SetActive(false);
    }
}