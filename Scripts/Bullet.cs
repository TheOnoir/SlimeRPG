using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Player Damage")]

    public float damage = 30f;

    [Space]

    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform target;
    [SerializeField] private float m_life = 5.0f;
    [SerializeField] private bool hit = false;

    private void Start()
    {
        target.parent = null;
    }

    private void Update()
    {
        if (!hit)
        {
            transform.forward = body.velocity;
        }

        if (hit)
        {
            m_life -= Time.deltaTime;
            if (m_life < 0)
            {
                Destroy(target.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hit)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                transform.parent = collision.collider.transform;
            }

            if (collision.transform.TryGetComponent(out EnemyAI enemyAI))
            {
                enemyAI.TakeDamage(damage);
            }

            hit = true;
            Destroy(GetComponent<BoxCollider>());
        }
    }

    public void Go(Vector3 speed, Vector3 targetPos)
    {
        body.velocity = speed;
        target.position = targetPos;
    }
}
