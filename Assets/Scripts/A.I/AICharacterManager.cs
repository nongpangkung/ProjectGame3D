using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FN
{
    public class AICharacterManager : CharacterManager
    {
        public AICharacterBossManager aiCharacterBossManager;
        public AICharacterLocomotionManager aiCharacterLocomotionManager;
        public AICharacterAnimatorManager aiCharacterAnimatorManager;
        public AICharacterStatsManager aiCharacterStatsManager;
        public AICharacterEffectsManager aiCharacterEffectsManager;

        public State currentState;
        public CharacterManager currentTarget;
        public NavMeshAgent navmeshAgent;
        public Rigidbody enemyRigidBody;

        public bool isPreformingAction;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;
        public float maximumFarFromTarget = 50;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        //The higher, and lower, respectively these angles are, the greater detection FIELD OF VIEW (basically like eye sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float currentRecoveryTime = 0;
        public float stoppingDistance = 1.5f;

        [Header("A.I Combat Settings")]
        public bool allowAIToPerformCombos;
        public bool isPhaseShifting;
        public float comboLikelyHood;

        [Header("A.I Target Information")]
        public float distanceFromTarget;
        public Vector3 targetsDirection;
        public float viewableAngle;

        protected override void Awake()
        {
            base.Awake();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            aiCharacterBossManager = GetComponent<AICharacterBossManager>();
            aiCharacterAnimatorManager = GetComponent<AICharacterAnimatorManager>();
            aiCharacterStatsManager = GetComponent<AICharacterStatsManager>();
            aiCharacterEffectsManager = GetComponent<AICharacterEffectsManager>();
            enemyRigidBody = GetComponent<Rigidbody>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            navmeshAgent.enabled = false;
        }

        private void Start()
        {
            enemyRigidBody.isKinematic = false;
        }

        protected override void Update()
        {
            HandleRecoveryTimer();
            HandleStateMachine();

            isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
            isInteracting = animator.GetBool("isInteracting");
            isPhaseShifting = animator.GetBool("isPhaseShifting");
            isInvulnerable = animator.GetBool("isInvulnerable");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            animator.SetBool("isDead", isDead);

            if (currentTarget != null)
            {
                distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
                targetsDirection = currentTarget.transform.position - transform.position;
                viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            aiCharacterEffectsManager.HandleAllBuildUpEffects();
        }

        private void LateUpdate()
        {
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPreformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPreformingAction = false;
                }
            }
        }
    }
}
