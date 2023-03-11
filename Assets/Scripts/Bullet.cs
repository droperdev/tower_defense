using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletObject bulletObject;
    public Vector3 target;

    public void SetTarget(Vector3 targetPosition){
        target = targetPosition;
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, bulletObject.speed * Time.deltaTime);

      
        if (transform.position == target)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {   
            Enemy enemy = other.GetComponent<Enemy>();
            switch(gameObject.tag){
                case "ColdBullet":
                    enemy.Freeze();
                    break;
                case "FireBullet":
                    enemy.Burn();
                    break;
                default:
                    enemy.TakeDamage(bulletObject.damage);
                    break;

            }
           
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class BulletObject {
    public float speed = 10f;
    public float damage = 5;
}