using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public class UnitStatDisplay : MonoBehaviour
    {
        public float maxHealth, armor, currentHealth, supplyCost;
        [SerializeField] private Image healthBarAmount;
        private bool isPlayerUnit = false;
        [SerializeField] private GameObject deathFX;
        [SerializeField] List<ProjectilesBehaviour> projectilesBehaviour = new List<ProjectilesBehaviour>();
        
        public void SetStatDisplayBasicUnit(UnitStatTypes.Base stats, bool isPlayer)
        {
            maxHealth = stats.Health;
            armor = stats.Armor;
            isPlayerUnit = isPlayer;
            currentHealth = maxHealth;
            supplyCost = stats.CostSupply;
        }

        public void SetStatDisplayBasicBuilding(Buildings.BuildingStatTypes.Base stats, bool isPlayer)
        {
            maxHealth = stats.health;
            armor = stats.armor;
            isPlayerUnit = isPlayer;
            currentHealth = maxHealth;
        }

        void Update()
        {
            HandleHealth();
        }
        
        public void TakeDamage(float damage)
        {
            float totalDamage = damage - armor;
            if (totalDamage >= 0)
            {
                currentHealth -= totalDamage;
            }
        }

        public void HandleHealth()
        {
            Camera camera = Camera.main;
            gameObject.transform.LookAt(gameObject.transform.position + 
                camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);

            healthBarAmount.fillAmount = currentHealth / maxHealth;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            if(isPlayerUnit)
            {
                InputManager.InputHandler.instance.selectedUnits.Remove(gameObject.transform.parent);
                if(projectilesBehaviour.Count > 0)
                {
                    foreach(ProjectilesBehaviour projBeh in projectilesBehaviour)
                    {
                        projBeh.SaveProjectiles();
                    }
                }
                Explode();
                Destroy(gameObject.transform.parent.gameObject);
                
                UI.InGame.PlayerResources.PlayerBank.instance.ManageSupply(-supplyCost, 0);
            }
            else
            {
                if(projectilesBehaviour.Count > 0)
                {
                    foreach(ProjectilesBehaviour projBeh in projectilesBehaviour)
                    {
                        projBeh.SaveProjectiles();
                    }
                }
                Explode();
                Destroy(gameObject.transform.parent.gameObject);
            } 
        }

        private void Explode()
        {
            GameObject vfx = Instantiate(deathFX, new Vector3(transform.position.x, transform.position.y / 3,transform.position.z), Quaternion.identity);
            vfx.transform.parent = gameObject.transform.parent.gameObject.transform.parent.transform;
            Destroy(vfx, 2f);
        }
    }
}
