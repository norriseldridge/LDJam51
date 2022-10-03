using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class KillEnemyTouch : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.TakeDamage(1);

            var proj = GetComponent<Projectile>();
            proj.Blowup();
        }
    }
}
