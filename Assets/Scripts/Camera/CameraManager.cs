using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CameraConfigScriptableObject cameraConfig = null;
    private Transform playerTransform;
    private BaseCameraMovements cameraMovement;

    private void Start()
    {
        this.cameraMovement = new InitialCameraMovements(transform, cameraConfig.Smoothness, cameraConfig.OffsetZ, cameraConfig.IntroStartPosition);
    }

    public FieldOfView FieldOfView { get; set; }

    private void Awake()
    {
        FieldOfView = GetComponentInChildren<FieldOfView>();
    }

    private void Update()
    {
        cameraMovement.Move();
    }
    
    public void Init(Transform transform)
    {
        this.playerTransform = transform;
    }

    public void StartIntro()
    {
        var valuableItemsPositions = FindObjectsOfType<ValuableItem>().Select(q => (Vector2)q.transform.position).ToList();
        this.cameraMovement = new IntroCameraMovements(transform, cameraConfig.IntroSmoothness, cameraConfig.OffsetZ, valuableItemsPositions, cameraConfig.IntroDurationOnValuableObject, this.StartGame);
    }

    public void StartGame()
    {
        this.cameraMovement = new GameCameraMovements(transform, cameraConfig.Smoothness, cameraConfig.OffsetZ, this.playerTransform);
    }
}
