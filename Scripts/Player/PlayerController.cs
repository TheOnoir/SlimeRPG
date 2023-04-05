using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Changeable Stats")]

    [SerializeField] private float damage = 25f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float attackDelay = 2f;

    [Header("Stats")]

    [SerializeField] private float health = 100f;
    [SerializeField] private float speed = 6f;

    [Header("Ballistics")]

    [SerializeField] private float attackRange = 100.0f;
    [SerializeField] private float bulletSpeed = 5.0f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunTransform;

    private Vector3 fromGunToTarget;
    private Vector3 fromGunToTargetXZ;
    private float currAttackDelay = 0;
    private Vector3 aim;

    [Header("Player")]

    [SerializeField] private Slider healthBar;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private float angleInDegrees;
    [SerializeField] private float stoppingDistance;

    private Vector3 direction;

    [Header("Nearest Enemy")]

    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private GameObject[] enemies;

    private EnemyAI enemyAI;

    private void Start()
    {
        healthBar.value = health;
        healthBar.maxValue = maxHealth;
    }

    private void Update()
    {
        FindNearestEnemy();                                                        

        if (enemy != null)
        {
            enemyTransform = enemy.GetComponent<Transform>();
            enemyAI = enemyTransform.GetComponent<EnemyAI>();
            direction = enemyTransform.position - transform.position;
            AimDirection();
                           
            TurnToEnemy();
        }

        if (WayMoreStoppingDistance || (enemy == null))
        {
            MovingForward();                                                                          
        }
        else if (!WayMoreStoppingDistance && (currAttackDelay < 0 && aim.magnitude < attackRange))
        {
            Shot();                                                                                         
        }
    }

    private bool WayMoreStoppingDistance => direction.sqrMagnitude > stoppingDistance * stoppingDistance;

    public void UpgradeDamage(int intermediateDamage) => damage += intermediateDamage;

    public void UpgradeHealth(int intermediateHealth) => maxHealth += intermediateHealth;

    public void UpgradeattackSpeed(float intermediateAttackSpeed) => attackDelay -= intermediateAttackSpeed;

    public void TakeDamage(float damage)
    {
        healthBar.value = health;
        health -= damage;

        if (health <= 0)                   
        {
            Destroy(this.gameObject);
            healthBar.gameObject.SetActive(false);
        }
    }

    private GameObject FindNearestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject go in enemies)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                enemy = go;
                distance = curDistance;
            }
        }
        return enemy;
    }

    private void AimDirection()
    {
        currAttackDelay -= Time.deltaTime;
        aim = enemyTransform.position - transform.position;
        aim.y = 0;
        transform.forward = aim;
    }

    private void MovingForward()
    {
        health = maxHealth;
        healthBar.value = health;
        healthBar.maxValue = maxHealth;
        Vector3 originalDirection = new(transform.position.x, transform.position.y, 0f);
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, originalDirection, speed * Time.deltaTime), 
            Quaternion.LookRotation(fromGunToTargetXZ, Vector3.up));
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    private void TurnToEnemy()
    {
        fromGunToTarget = enemyTransform.position - gunTransform.position;
        fromGunToTargetXZ = new Vector3(fromGunToTarget.x, 0f, fromGunToTarget.z);
        gunTransform.localEulerAngles = new Vector3(-angleInDegrees, 0f, 0f);
        transform.rotation = Quaternion.LookRotation(fromGunToTargetXZ, Vector3.up);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Shot()
    {
        currAttackDelay = attackDelay;

        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.position = gunTransform.position;
        Vector3 hitPoint = GetHitPoint(enemyAI.EnemyAimPos, enemyAI.lastSpeed, transform.position, bulletSpeed, out float time);
        Vector3 aim = hitPoint - transform.position;
        aim.y = 0;

        float antiGravity = -Physics.gravity.y * time / 2;
        float deltaY = (hitPoint.y - newBullet.transform.position.y) / time;

        Vector3 arrowSpeed = aim.normalized * bulletSpeed;
        arrowSpeed.y = antiGravity + deltaY;

        newBullet.GetComponent<Bullet>().Go(arrowSpeed, hitPoint);
        newBullet.GetComponent<Bullet>().damage = damage;
    }

    Vector3 GetHitPoint(Vector3 targetPosition, Vector3 targetSpeed, Vector3 attackerPosition, float bulletSpeed, out float time)
    {
        Vector3 q = targetPosition - attackerPosition;
        //Пока игнорирую Y. Добавьте компенсацию гравитации позже, для более простой формулы и чистого игрового дизайна вокруг нее.
        q.y = 0;
        targetSpeed.y = 0;

        //Решение квадратичного уравнения из t * t (Vx*Vx + Vy*Vy - S*S) + 2 * t*(Vx*Qx)(Vy*Qy) + Qx*Qx + Qy*Qy = 0
        float a = Vector3.Dot(targetSpeed, targetSpeed) - (bulletSpeed * bulletSpeed); //Точка - это, в основном, (targetSpeed.x * targetSpeed.x) + (targetSpeed.y * targetSpeed.y)
        float b = 2 * Vector3.Dot(targetSpeed, q); //Точка - это, в основном, (targetSpeed.x * q.x) + (targetSpeed.y * q.y)
        float c = Vector3.Dot(q, q); //Точка - это, в основном, (q.x * q.x) + (q.y * q.y)

        float D = Mathf.Sqrt((b * b) - 4 * a * c);
        float t1 = (-b + D) / (2 * a);
        float t2 = (-b - D) / (2 * a);
        time = Mathf.Max(t1, t2);

        Vector3 ret = targetPosition + targetSpeed * time;
        return ret;
    }
}
