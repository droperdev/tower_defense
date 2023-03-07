using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 5;

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
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

      
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
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

}
