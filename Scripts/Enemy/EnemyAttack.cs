using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Enemy Damage")]

    public float damage;

    [Space]

    [SerializeField] private Animator animator;
    [SerializeField] private EnemyAI enemyAI;

    private void Update()
    {
        if (enemyAI.attack == true)
        {
            animator.SetBool("attack", true);
        }
        else
        {
            animator.SetBool("attack", false);
        }
    }

    private void OnTriggerEnter(Collider other)                 //Противник меня атакует
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            playerController.TakeDamage(damage);
        }
    }
}
