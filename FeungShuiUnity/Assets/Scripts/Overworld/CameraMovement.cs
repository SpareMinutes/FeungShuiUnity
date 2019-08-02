using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject toFollow;

    void Update () {
        gameObject.transform.position = new Vector3(toFollow.transform.position.x, toFollow.transform.position.y, -10);
    }
}
