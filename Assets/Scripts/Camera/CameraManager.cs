using System;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField]
    private CameraConfigScriptableObject cameraConfig = null;
    private Transform playerTransform;
    private BaseCameraMovements cameraMovement;
    public FieldOfView FieldOfViewManager { get; private set; }

    private void Start()
    {
        FieldOfViewManager = GetComponentInChildren<FieldOfView>();
        CameraManager.Instance = this;
        this.cameraMovement = new InitialCameraMovements(transform, cameraConfig.Smoothness, cameraConfig.OffsetZ, cameraConfig.IntroStartPosition);
    }

    public FieldOfView FieldOfView { get; set; }

    private void Awake()
    {
        FieldOfView = GetComponentInChildren<FieldOfView>();
    }
    private void Update()
    {
        cameraMovement.Update();
    }
    private void FixedUpdate()
    {
        cameraMovement.Move();
    }
    
    public void Init(Transform transform)
    {
        this.playerTransform = transform;
    }

    public void StartIntro(Action callback)
    {
        var valuableItemsPositions = FindObjectsOfType<NetworkValuableItem>().OrderBy(q => q.name).Select(q => (Vector2)q.transform.position).ToList();
        this.cameraMovement = new IntroCameraMovements(transform, cameraConfig.IntroSmoothness, cameraConfig.OffsetZ, valuableItemsPositions, cameraConfig.IntroDurationOnValuableObject, callback);
    }

    public void StartPlaying()
    {
        this.cameraMovement = new GameCameraMovements(transform, cameraConfig.Smoothness, cameraConfig.OffsetZ, this.playerTransform);
    }

    public void StartFollowPlayerCameraMovements(PlayerType? playerType = null)
    {
        this.cameraMovement = new FollowPlayerCameraMovements(transform, cameraConfig.Smoothness, cameraConfig.OffsetZ, playerType);
    }
}
