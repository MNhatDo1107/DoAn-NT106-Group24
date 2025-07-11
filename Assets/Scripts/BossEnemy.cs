using Photon.Pun;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedDanThuong = 20f;
    [SerializeField] private float speedDanVongTron = 10f;
    [SerializeField] private float hpValue = 100f;
    [SerializeField] private GameObject miniEnemy;
    [SerializeField] private float skillCooldown = 2f;
    private float nextSkillTime = 0f;
    [SerializeField] private GameObject usbPrefasb;

    protected override void Update()
    {
        base.Update();
        if (PhotonNetwork.IsMasterClient && Time.time >= nextSkillTime)
        {
            SuDungSkill();
        }
    }

    protected override void Die()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(usbPrefasb.name, transform.position, Quaternion.identity);
        }
        base.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(enterDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(stayDamage);
        }
    }

    private void BanDanThuong()
    {
        if (player != null && PhotonNetwork.IsMasterClient)
        {
            Vector3 directionToPlayer = player.transform.position - firePoint.position;
            directionToPlayer.Normalize();
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefabs.name, firePoint.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            enemyBullet.SetMovementDirection(directionToPlayer * speedDanThuong);
        }
    }

    private void BanDanVongTron()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefabs.name, transform.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            enemyBullet.SetMovementDirection(bulletDirection * speedDanVongTron);
        }
    }

    private void HoiMau(float hpAmount)
    {
        currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
        UpdateHpBar();
    }

    private void SinhMiniEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate(miniEnemy.name, transform.position, Quaternion.identity);
    }

    private void DichChuyen()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }

    private void ChonSkillNgauNhien()
    {
        int randomSkill = Random.Range(0, 5);
        switch (randomSkill)
        {
            case 0:
                BanDanThuong();
                break;
            case 1:
                BanDanVongTron();
                break;
            case 2:
                HoiMau(hpValue);
                break;
            case 3:
                SinhMiniEnemy();
                break;
            case 4:
                DichChuyen();
                break;
        }
    }

    private void SuDungSkill()
    {
        nextSkillTime = Time.time + skillCooldown;
        ChonSkillNgauNhien();
    }
}