using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using static GameInput;

public class Player : MonoBehaviour, IDataPersistence {
    public static Player Instance { get; private set; }
    private NavMeshAgent navMeshAgent;
    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private float playerRadius = .7f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private GameInput input;

    private bool isWalking;
    private Vector3 lastInteractionDirection;

    private void Awake() {
        if (Instance != null) {
            Debug.LogWarning("Player already exists");
            return;
        }
        Instance = this;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start() {
        input.OnMoveByMouseAction += GameInput_OnMoveByMouseAction;
    }

    private void Update() {
        FaceTarget();
        HandleMovement();
    }
    private void GameInput_OnMoveByMouseAction(object sender, OnMoveByMouseEventArgs e) {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers)) {
            navMeshAgent.destination = hit.point;
            if (clickEffect != null) {
                Instantiate(clickEffect, hit.point += new Vector3(0f, 0.1f, 0f), clickEffect.transform.rotation);
            }
        }
    }

    public bool IsWalking() {
        return isWalking || navMeshAgent.destination != transform.position;
    }

    private void FaceTarget() {
        if (navMeshAgent.destination != Vector3.zero) {
            Vector3 direction = navMeshAgent.destination - transform.position;
            transform.forward = Vector3.Slerp(transform.forward, direction, rotationSpeed * Time.deltaTime);
        }
    }

    public void LoadData(GameData data) {
        transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data) {
        data.playerPosition = transform.position;
    }

    private void HandleMovement() {
        Vector2 inputVector = input.GetNormalizedMovementVector();
        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);

        if (direction != Vector3.zero) {
            navMeshAgent.ResetPath();
        }

        float moveDistance = Time.deltaTime * movementSpeed;

        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            direction,
            moveDistance
        );
        if (!canMove) {
            Vector3 dirX = new Vector3(direction.x, 0f, 0f).normalized;
            canMove = direction.x != 0 && !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                dirX,
                moveDistance
            );
            if (canMove) {
                direction = dirX;
            } else {
                Vector3 dirZ = new Vector3(0f, 0f, direction.z).normalized;
                canMove = direction.z != 0 && !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    dirZ,
                    moveDistance
                );
                if (canMove) {
                    direction = dirZ;
                }
            }
        }

        if (canMove) {
            transform.position += direction * moveDistance;
        }

        isWalking = direction != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, direction, rotationSpeed * Time.deltaTime);
    }
}
