using UnityEngine;
using Photon.Pun;

public class PlayerCollision : MonoBehaviourPun
{
    [SerializeField] private GameManager gameManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Chỉ xử lý nếu đây là nhân vật của người chơi đang điều khiển (local player)
        if (!photonView.IsMine) return;

        if (collision.collider.CompareTag("EnemyBullet"))
        {
            Player player = GetComponent<Player>();
            player.TakeDamage(10f);
        }
        else if (collision.collider.CompareTag("Usb"))
        {
            Debug.Log("Collected USB");
            Destroy(collision.collider.gameObject);
        }
        else if (collision.collider.CompareTag("Energy"))
        {
            gameManager.AddEnergy();
            Destroy(collision.collider.gameObject);
        }
    }
}
