using UnityEngine;

public class StartPosition : MonoBehaviour
{
    [SerializeField]
    private PlayerType playerType;

    public PlayerType PlayerType { get => playerType; }

    public Vector3 Position => transform.position;
}
