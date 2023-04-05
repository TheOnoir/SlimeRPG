using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [Header("Changeable Stats")]

    [SerializeField] private float damage = 20f;
    [SerializeField] private float health;

    [Header("Stats")]

    [SerializeField] private float stoppingDistance;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float speed;

    [Header("Enemy")]

    public Vector3 lastSpeed = new Vector3();
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform enemyAim;
    [SerializeField] private GameObject parentOutDamage, prefabOutDamage;
    [SerializeField] private EnemyAttack enemyAttack;
    public bool attack;

    private PlayerUpgrade playerUpgrade;
    private Transform playerTransform;
    private Vector3 playerVector;
    private int outDamageNum;
    private OutgoingDamage[] outDamagePool = new OutgoingDamage[15];

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        playerUpgrade = player.GetComponent<PlayerUpgrade>();
        damage += playerUpgrade.enemyDamage;
        health += playerUpgrade.enemyHealth;
        healthBar.maxValue = health;

        for (int i = 0; i < outDamagePool.Length; i++)
        {
            outDamagePool[i] = Instantiate(prefabOutDamage, parentOutDamage.transform).GetComponent<OutgoingDamage>();
        }
    }

    private void Update()
    {
        enemyAttack.damage = damage;
        healthBar.value = health;
        Vector3 aim = playerTransform.position - transform.position;
        playerVector = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);
        var direction = playerTransform.position - transform.position;

        if (direction.sqrMagnitude > stoppingDistance * stoppingDistance)
        {
            lastSpeed = aim.normalized * speed;
            float step = speed * Time.deltaTime;
            Vector3 currentToTarget = playerVector - this.transform.position;
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, playerVector, step),
                Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentToTarget), Time.deltaTime * rotationSpeed));
        }
        else
        {
            attack = true;
        }
    }

    public Vector3 EnemyAimPos => enemyAim.position;

    public void TakeDamage(float damage)
    {
        health -= damage;
        outDamagePool[outDamageNum].StartMotion((int)damage);
        outDamageNum = outDamageNum == outDamagePool.Length - 1 ? 0 : outDamageNum + 1;

        if (health <= 0)                    //я убил этого противника - выйграл
        {
            Destroy(this.gameObject);
            healthBar.gameObject.SetActive(false);
            playerUpgrade.CoinForKill();
            playerUpgrade.BoostEnemyStats();
        }
    }
}
