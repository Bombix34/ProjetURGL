using Mirror;
using UnityEngine;

public class IAMovement : NetworkBehaviour
{
    public Vector2 speed = new Vector2(50, 50);

    private void Update()
    {
        if (!isServer)
        {
            return;
        }

        float inputX = UnityEngine.Random.Range(-1f, 1f);
        float inputY = UnityEngine.Random.Range(-1f, 1f);

        Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);
        movement *= Time.deltaTime;

        transform.Translate(movement);
    }
}
