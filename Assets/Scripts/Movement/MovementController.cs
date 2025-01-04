using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameInput;
using UnityEngine.AI;

namespace DapperDino.Movement {
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class MovementController : MonoBehaviour {
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float speedSmoothTime = 0.1f;

        [SerializeField] private GameInput input;
        [SerializeField] ParticleSystem clickEffect;
        [SerializeField] LayerMask clickableLayers;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float playerRadius = .7f;
        [SerializeField] private float playerHeight = 2f;
        private NavMeshAgent navMeshAgent;
        private bool isWalking;

        private CharacterController controller = null;
        private Animator animator = null;
        private Transform mainCameraTransform = null;

        private float velocityY = 0f;
        private float speedSmoothVelocity = 0f;
        private float currentSpeed = 0f;

        private static readonly int hashSpeedPercentage = Animator.StringToHash("SpeedPercentage");

        private void Start() {
            input.OnMoveByMouseAction += GameInput_OnMoveByMouseAction;
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            mainCameraTransform = Camera.main.transform;
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

        private void Update() {
            FaceTarget();
            Move();
        }

        private void FaceTarget() {
            if (navMeshAgent.destination != Vector3.zero) {
                Vector3 direction = navMeshAgent.destination - transform.position;
                transform.forward = Vector3.Slerp(transform.forward, direction, rotationSpeed * Time.deltaTime);
            }
        }

        private void Move() {
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
            if (inputVector.magnitude > 0f) {
                animator.SetFloat(hashSpeedPercentage, 0.5f * inputVector.magnitude, speedSmoothTime, Time.deltaTime);
            } else {
                animator.SetFloat(hashSpeedPercentage, 0.25f * navMeshAgent.desiredVelocity.magnitude, speedSmoothTime, Time.deltaTime);
            }
        }
    }
}
