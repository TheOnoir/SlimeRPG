using UnityEngine;
using UnityEngine.UI;

public class OutgoingDamage : MonoBehaviour
{
    private bool move;
    private Vector2 randomVector;

    private void Update()
    {
        if (!move) return;
        transform.Translate(randomVector * Time.deltaTime);
    }

    public void StartMotion(int damage)
    {
        transform.localPosition = Vector2.zero;
        GetComponent<Text>().text = "-" + damage;
        randomVector = new Vector2(Random.Range(-5, 5), Random.Range(0, 0));
        move = true;
        GetComponent<Animation>().Play();
    }
}
