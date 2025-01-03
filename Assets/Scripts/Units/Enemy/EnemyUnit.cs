using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Units.Player;

namespace Units.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class EnemyUnit : MonoBehaviour
    {
        private NavMeshAgent navAgent;
        public BasicUnit unitType;

        [HideInInspector] public UnitStatTypes.Base baseStats;

        public UnitStatDisplay statDisplay;
        private Collider[] rangeColliders;
        private Transform aggroTarget;
        private UnitStatDisplay aggroUnit;

        private float distance;
        private float attackCooldown;
        private CannonAiming cannonAiming;
        [SerializeField] ParticleSystem attackVFX;
        [SerializeField] AudioSource attackSound;
        [SerializeField] List<ParticleSystem> psProjectiles = new List<ParticleSystem>();

        private void Start() 
        {
            baseStats = unitType.baseStats;
            statDisplay.SetStatDisplayBasicUnit(baseStats, false);
            navAgent = gameObject.GetComponent<NavMeshAgent>();
            cannonAiming = GetComponentInChildren<CannonAiming>();
        }
        
        private void Update() 
        {
             attackCooldown -= Time.deltaTime;

            if (aggroTarget == null)
            {
                CheckForEnemyTargets();
            }    
            else if (aggroTarget != null)
            {
                cannonAiming.target = aggroTarget;
                MoveToAggroTarget();
                Attack();
            }
        }

        private void CheckForEnemyTargets()
        {
   
            rangeColliders = Physics.OverlapSphere(transform.position, baseStats.AggroRange, UnitHandler.instance.pUnitLayer);

            for (int i = 0; i < rangeColliders.Length;)
            {
                aggroTarget = rangeColliders[i].gameObject.transform;
                aggroUnit = aggroTarget.gameObject.GetComponentInChildren<UnitStatDisplay>();
                break;
            }
        }

        private void Attack()
        {
            if (attackCooldown <= 0 && distance <= baseStats.AttackRange && cannonAiming.isAimed == true)
            {
                attackCooldown = baseStats.AttackSpeed;
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
                    aggroUnit.TakeDamage(baseStats.Attack);
                }
            }
        }
        
        private void MoveToAggroTarget()
        {
            if (aggroTarget == null)
            {
                navAgent.SetDestination(transform.position);
            }
            else
            {
                distance = Vector3.Distance(aggroTarget.position, transform.position);

                if (distance <= baseStats.AggroRange * 1.3 && distance >= baseStats.AttackRange)
                {
                    navAgent.SetDestination(aggroTarget.position);
                }
                if (distance <= baseStats.AttackRange)
                {
                    navAgent.SetDestination(transform.position);
                }
            }
        }
    }
}
