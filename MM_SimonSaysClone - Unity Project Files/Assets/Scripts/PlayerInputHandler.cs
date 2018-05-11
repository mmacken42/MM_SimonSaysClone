using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private bool canType;
    private bool canClick;

    private Ray raycast;
    private RaycastHit raycastHit;

    private Camera mainCamera;

    private SimonSaysGameBoard simonGameBoard;
    private SimonSaysCube lastSimonCubeHit;

    public LayerMask CollisionMask;

    private void Awake()
    {
        mainCamera = Camera.main;
        simonGameBoard = GetComponent<SimonSaysGameBoard>();
    }

    private void Update()
    {
        if (canClick && Input.GetMouseButtonDown(0))
        {
            raycast = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(raycast, out raycastHit, 10f, CollisionMask.value, QueryTriggerInteraction.Collide))
            {
                lastSimonCubeHit = raycastHit.collider.GetComponent<SimonSaysCube>();

                if (lastSimonCubeHit != null)
                {
                    lastSimonCubeHit.PlayerSelect();
                }
            }
        }

        if (canType && Input.GetKeyDown(KeyCode.Space))
        {
            simonGameBoard.StartNewGame();
        }
    }

    public void SetCanClick(bool setting)
    {
        canClick = setting;
    }

    public void SetCanType(bool setting)
    {
        canType = setting;
    }
}
