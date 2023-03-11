
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;
    public TowerObject towerObject;

    private List<Collider> colliders = new List<Collider>();
    void Start()
    {
        InvokeRepeating("Attack", 0f, towerObject.attackInterval);
    }

    void Attack()
    {
        colliders.Clear();
        //Debug.Log("Atacando");
        List<Collider> hitColliders = Physics.OverlapSphere(gameObject.transform.GetChild(0).position, towerObject.attackRadius).ToList();
        if(hitColliders.Count == 0) return;

        hitColliders.Sort(delegate (Collider a, Collider b)
        {
            return Vector3.Distance(a.transform.position, transform.GetChild(0).position).CompareTo(Vector3.Distance(b.transform.position, transform.GetChild(0).position));
        });
        foreach (Collider hitCollider in hitColliders)
        {
            colliders.Add(hitCollider);
        }

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (collider.CompareTag("Enemy") && !enemy.enemyObject.isDead)
            {
                GameObject bulletObject = Instantiate(bulletPrefab, transform.GetChild(0).position, Quaternion.identity);
                Bullet bullet = bulletObject.GetComponent<Bullet>();
                bullet.target = collider.bounds.center;
                break;
            }

        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.position;
        Gizmos.DrawWireSphere(position, towerObject.attackRadius);
    }
}


[System.Serializable]
public class TowerObject
{
    public string towerName;
    public string towerDescription;
    public int buyPrice;
    public int sellPrice;
    public float attackRadius;
    public float attackInterval;
}