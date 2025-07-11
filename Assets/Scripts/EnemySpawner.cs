using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 2f;

    private Coroutine _spawnCoroutine;

    void Start()
    {
        CheckAndStartSpawning();
    }

    public override void OnJoinedRoom()
    {
        CheckAndStartSpawning();
    }

      public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        Debug.Log($"MasterClient switched! IsMasterClient: {PhotonNetwork.IsMasterClient}");
        CheckAndStartSpawning();
    }

    private void CheckAndStartSpawning()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_spawnCoroutine == null)
            {
                Debug.Log("This client is MasterClient. Start spawning enemies.");
                _spawnCoroutine = StartCoroutine(SpawnEnemyCoroutine());
            }
        }
        else
        {
            if (_spawnCoroutine != null)
            {
                Debug.Log("Lost MasterClient. Stop spawning enemies.");
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        Debug.Log("SpawnEnemyCoroutine Started");
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            GameObject enemyPrefab = enemies[Random.Range(0, enemies.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Debug.Log("Spawn: " + enemyPrefab.name + " at " + spawnPoint.position);
            PhotonNetwork.Instantiate(enemyPrefab.name, spawnPoint.position, Quaternion.identity);
        }
    }
}