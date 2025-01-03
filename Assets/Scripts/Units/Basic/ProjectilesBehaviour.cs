using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesBehaviour : MonoBehaviour
{
    GameObject projectilesSaviour;
    public float damage;
    private bool isSaved = false;
    ParticleSystem ps;

    void Start()
    {
        if (gameObject.layer == 8)
        {
            damage = transform.parent.parent.parent.parent.parent.GetComponent<Units.Player.PlayerUnit>().baseStats.Attack;
        }
        else if (gameObject.layer == 9)
        {
            damage = transform.parent.parent.parent.parent.parent.GetComponent<Units.Enemy.EnemyUnit>().baseStats.Attack;
        }
        ps = GetComponent<ParticleSystem>();
        projectilesSaviour = GameObject.Find("Projectiles Saviour");
    }

    private void Update() 
    {
        if(isSaved && ps.particleCount == 0)
        {
            Destroy(this.gameObject);
        }    
    }
    
    public void SaveProjectiles()
    {
        transform.SetParent(projectilesSaviour.transform);
        isSaved = true;
    }

}
