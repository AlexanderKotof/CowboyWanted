using UnityEngine;

public class CameraRigComponent : MonoBehaviour
{
    public PlayerComponent player;

    private float minDistance = 0.3f;
    public float moveSpeed = 2f;

    private void Update()
    {
        if (!player)
            return;

        if ((transform.position - player.transform.position).sqrMagnitude > minDistance * minDistance)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }
}
