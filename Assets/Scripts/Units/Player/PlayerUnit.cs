using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Units.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerUnit : MonoBehaviour
    {

        public NavMeshAgent navAgent;
        public BasicUnit unitType;

        [HideInInspector]
        public UnitStatTypes.Base baseStats;
        public UnitStatDisplay statDisplay;

        private Collider[] rangeColliders;
        private Transform aggroTarget;
        public GameObject plannedBuilding;
        [SerializeField] private UnitStatDisplay aggroUnit;
       
        private float distance;
        private float nearestDistance;
        private float attackCooldown;
        private bool hasMoveOrder = false;

        [SerializeField] ParticleSystem attackVFX; 
        [SerializeField] AudioSource attackSound;
        [SerializeField] List<ParticleSystem> psProjectiles = new List<ParticleSystem>();

        private Units.Basic.Workers.ResourceMiner miningManager;
        private CannonAiming cannonAiming;
        private HighlightUnderCursor highlightUnderCursor;


        private void Awake() 
        {
            baseStats = unitType.baseStats;
            statDisplay.SetStatDisplayBasicUnit(baseStats, true);
            navAgent = GetComponent<NavMeshAgent>();  
            navAgent.speed = baseStats.speed;

            if(unitType.type == 0)
            {
                miningManager = GetComponent<Units.Basic.Workers.ResourceMiner>();
            }
            else
            {
                cannonAiming = GetComponentInChildren<CannonAiming>();
            }
        }

        private void Update() 
        {
            attackCooldown -= Time.deltaTime;

            if(aggroTarget != null && hasMoveOrder && distance > baseStats.attackRange)
            {
                SetAggroTarget(null);
            }

            if (aggroTarget == null)
            {
                FindClosestEnemyNearby();
            }  
              
            if (aggroTarget != null)
            {
                distance = Vector3.Distance(transform.position, aggroTarget.position);
                if(cannonAiming)
                {
                    cannonAiming.target = aggroTarget;
                }
                if(!hasMoveOrder)
                {
                    MoveToAggroTarget();
                } 
                Attack();
            }
            
            if(ReachedDestinationOrGaveUp()) 
            {
                hasMoveOrder = false;
                MoveOrderUnit(transform.position);
            }

            if(plannedBuilding != null && Vector3.Distance(transform.position, plannedBuilding.transform.position) < 8.5)
            {
                
                StartConstructingBuilding();
                MoveOrderUnit(transform.position);
                plannedBuilding = null;
            }
        }

        public void MoveToBuild()
        {
            MoveOrderUnit(plannedBuilding.transform.position);
        }

        public void StartConstructingBuilding()
        {
            plannedBuilding.GetComponent<Units.BuildingConstructing>().StartConstructing();
            highlightUnderCursor = plannedBuilding.AddComponent<HighlightUnderCursor>();            
        }

        public void CancelConstructingBuilding()
        {
            if(plannedBuilding)
            {
                UI.InGame.PlayerResources.PlayerBank.instance.DepositResources(
                    plannedBuilding.gameObject.GetComponent<Buildings.Player.PlayerBuilding>().baseStats.costMinerals,
                    plannedBuilding.gameObject.GetComponent<Buildings.Player.PlayerBuilding>().baseStats.costEnergium);
                Destroy(plannedBuilding);   
            }
        }
        
        public void MoveOrderUnit(Vector3 destination)
        {
            hasMoveOrder = true;
            navAgent.stoppingDistance = 0f;
            navAgent.SetDestination(destination);
            if(aggroTarget != null && distance > baseStats.attackRange)
            {
                SetAggroTarget(null);
            }
        }

        public void CommandMining(GameObject minetarget)
        {
            if(miningManager)
            {
                miningManager.StartWorking(minetarget);
            }
        }

        public void StopMining()
        {
            if (miningManager)
            {
                miningManager.StopWorking();
            }
        }

        public void SetAggroTarget(Transform x)
        {
            aggroTarget = x;
            if(aggroTarget == null)
            {
                aggroUnit = null;
                if(cannonAiming)
                {
                    cannonAiming.target = null;
                }
            }
            else
            {
                hasMoveOrder = false;
                aggroUnit = aggroTarget.gameObject.GetComponentInChildren<UnitStatDisplay>();
                if(cannonAiming)
                {
                    cannonAiming.target = aggroTarget;
                }
            }
        }

        private void MoveToAggroTarget()
        {
            if (distance > baseStats.attackRange)
            {
                navAgent.SetDestination(aggroTarget.position);
            }
            else
            {
                navAgent.SetDestination(transform.position);
            }
        }

        private bool ReachedDestinationOrGaveUp()
        {
            if (!navAgent.pathPending)
            {
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void FindClosestEnemyNearby()
        {
            rangeColliders = Physics.OverlapSphere(transform.position, baseStats.attackRange, UnitHandler.instance.eUnitLayer);
            if (rangeColliders.Length > 0)
            {
                int k = 0;
                nearestDistance = Vector3.Distance(transform.position, rangeColliders[0].transform.position);
                for (int i = 1;  i < rangeColliders.Length; i++)
                {
                    distance = Vector3.Distance(transform.position, rangeColliders[i].transform.position);
                    if(distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        k = i;
                    }
                }
                aggroTarget = rangeColliders[k].gameObject.transform;
                aggroUnit = aggroTarget.gameObject.GetComponentInChildren<UnitStatDisplay>();
            }
        }

        private void Attack()
        {
            if (attackCooldown <= 0 && distance <= baseStats.attackRange && cannonAiming.isAimed == true)
            {
                attackCooldown = baseStats.attackSpeed;
                if(attackVFX)
                {
                    attackVFX.Play();
                }
                attackSound.Play(0);
                if(psProjectiles.Count > 0)
                {
                    foreach(ParticleSystem particleSystem in psProjectiles)
                    {
                        particleSystem.Emit(1);
                    } 
                }
                else
                {
                    aggroUnit.TakeDamage(baseStats.attack);
                }
            }
        }
    }
}