using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform cameraMainTransform;

    private void Start()
    {
        GameObject cameraMain = GameObject.FindGameObjectWithTag("CM vcam1");
        cameraMainTransform = cameraMain.GetComponent<Transform>();
    }

    void LateUpdate()
    {
        transform.LookAt(cameraMainTransform);
    }
}
