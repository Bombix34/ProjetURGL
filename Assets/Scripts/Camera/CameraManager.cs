using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CameraConfigScriptableObject cameraConfig;
    private Transform playerTransform;
    private BaseCameraMovements cameraMovement;

    private void Start()
    {
        this.cameraMovement = new InitialCameraMovements(transform, cameraConfig.Smoothness, cameraConfig.OffsetZ, cameraConfig.IntroStartPosition);
        //this.transform.position = cameraConfig.IntroStartPosition.ToVector3(cameraConfig.OffsetZ);
    }

    private void Update()
    {
        cameraMovement.Move();
        //if(cameraState == CameraState.WAITING)
        //{
        //    return;
        //}


        //if (target == null )
        //    return;
        //Vector3 newPosition = this.GetNewPosition();

        //this.transform.position = Vector2.Lerp(target.transform.position, newPosition, cameraConfig.Smoothness).ToVector3(cameraConfig.OffsetZ);
    }
    
    public void Init(Transform transform)
    {
        this.playerTransform = transform;
    }

    public void StartIntro()
    {
        var test = FindObjectsOfType<ValuableItem>();
        var test2 = FindObjectsOfType<ValuableItem>().Select(q => q.transform.position).ToList();
        var test3 = FindObjectsOfType<ValuableItem>().Select(q => (Vector2)q.transform.position).ToList();
        var valuableItemsPositions = FindObjectsOfType<ValuableItem>().Select(q => (Vector2)q.transform.position).ToList();
        this.cameraMovement = new IntroCameraMovements(transform, cameraConfig.IntroSmoothness, cameraConfig.OffsetZ, valuableItemsPositions, cameraConfig.IntroDurationOnValuableObject, this.StartGame);
    }

    public void StartGame()
    {
        this.cameraMovement = new GameCameraMovements(transform, cameraConfig.Smoothness, cameraConfig.OffsetZ, this.playerTransform);
    }

    //private Vector3 GetNewPosition()
    //{
    //    switch (this.cameraState)
    //    {
    //        case CameraMovementsState.INTRO:
    //            return new Vector3(target.position.x, target.position.y, cameraConfig.OffsetZ);
    //        case CameraMovementsState.GAME:
    //            return target.position.ToVector3WithNewZ(cameraConfig.OffsetZ);
    //        case CameraMovementsState.INITIAL:
    //            return cameraConfig.IntroStartPosition;
    //        default:
    //            throw new NotImplementedException($"The state {cameraState} is not implemented");
    //    }
    //}

    //public void SetTarget(Transform target)
    //{
    //    this.target = target;
    //}
}
