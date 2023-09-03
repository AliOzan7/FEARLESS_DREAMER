using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] float normalCameraSensitivity = 1.0f;
    [SerializeField] float aimedCameraSensitivity = 0.8f;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform targetTransform;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private bool targetIsGround = false;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        //Aimin through crosshair and making it independent of the screen
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            targetTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            targetIsGround = raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Ground");
            //Debug.Log("target is ground = " + targetIsGround);
        }

        //Debug.Log("target position = " + targetTransform.position);

        // aim camera already has higher priority so just enabling it is enough.
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetCameraSensitivity(aimedCameraSensitivity);
            thirdPersonController.SetRotateOnMove(false);


            // we want the character to look/rotate to the aimed point
            Debug.Log("aiming");
            Vector3 worldAimTarget = mouseWorldPosition;
            mouseWorldPosition.y = transform.position.y;

            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            
            //transform.LookAt(Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f));
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetCameraSensitivity(normalCameraSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }
    }

    public Transform GetTargetTransform()
    {
        return targetTransform;
    }

    public bool IsTargetGround()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            targetTransform.position = raycastHit.point;
            targetIsGround = raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Default");
        }
        return targetIsGround;
    }
}
