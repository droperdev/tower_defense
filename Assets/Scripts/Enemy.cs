using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyObject enemyObject;

    [Header("Movement")]
    [HideInInspector] public List<Transform> waypoints = new List<Transform>();
    private int targetIndex = 1;
    
    private Animator anim;

   
    public Image fillLifeImage;
    private Transform canvasRoot;
    private Quaternion initLifeRotation;

    public int damage = 5;
    public int coin = 10;

    private bool isFrozen = false;
    private bool isBurn = false;

    void Awake()
    {
        canvasRoot = fillLifeImage.transform.parent.parent;
        initLifeRotation = canvasRoot.rotation;
        anim = GetComponent<Animator>();
        anim.SetBool("IsJump", true);
        SetWaypoints();
    }

    void Start()
    {
        enemyObject.currentLife = enemyObject.maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        canvasRoot.transform.rotation = initLifeRotation;
        Movement();
        LookAt();
    }
    private void SetWaypoints()
    {
        waypoints.Clear();
        var rootWaypoints = GameObject.Find("WayPointsContainer").transform;
        for (int i = 0; i < rootWaypoints.childCount; i++)
        {
            waypoints.Add(rootWaypoints.GetChild(i));
        }
    }

    #region Movement &Rotations
    private void Movement()
    {
        if (enemyObject.isDead)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetIndex].position, enemyObject.movementSpeed * Time.deltaTime);
        var distance = Vector3.Distance(transform.position, waypoints[targetIndex].position);
        if (distance <= 0.1f)
        {
            if (targetIndex >= waypoints.Count - 1)
            {
                Destroy(gameObject);
                GameManager.inst.playerObject.DrecreaseLife(damage);
                //targetIndex = 0;
                return;
            }
            targetIndex++;
        }
    }

    private void LookAt()
    {
        //transform.LookAt(waypoints[targetIndex]);

        var dir = waypoints[targetIndex].position - transform.position;
        var rootTarget = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rootTarget, enemyObject.rotationSpeed * Time.deltaTime);
    }

    #endregion

    public void TakeDamage(float damage)
    {
        var newLife = enemyObject.currentLife - damage;
        if (enemyObject.isDead)
        {
            return;
        }
        if (newLife <= 0)
        {
            OnDead();
        }

        enemyObject.currentLife = newLife;
        var fillValue = enemyObject.currentLife * 1 / enemyObject.maxLife;
        fillLifeImage.fillAmount = fillValue;
        //StartCoroutine(AnimationDamage());
    }

    public void Freeze()
    {
        if (!isFrozen)
        {
            isFrozen = true;
            StartCoroutine(Defrost());
        }
        else
        {
            timeFrozen = 3.0f;
        }
    }

    public float timeFrozen = 3.0f;
    IEnumerator Defrost()
    {
        float newMovementSpeed = enemyObject.movementSpeed / 2;
        float nextFreezeTick = Time.time + 0.1f;
        while (timeFrozen > 0.0f)
        {
            if (Time.time > nextFreezeTick)
            {
                enemyObject.movementSpeed = newMovementSpeed;
                nextFreezeTick += 0.1f;
            }

            yield return null;
            timeFrozen -= Time.deltaTime;
        }

        enemyObject.movementSpeed = newMovementSpeed * 2;
        isFrozen = false;
        timeFrozen = 3.0f;
    }

    public float timeRemaining = 3.0f;
    public void Burn()
    {
        if (!isBurn)
        {
            isBurn = true;
            StartCoroutine(StopBurning());
        }
        else
        {
            this.timeRemaining = 3.0f;
        }
    }

    IEnumerator StopBurning()
    {
        float nextBurnTick = Time.time + 0.1f;
        while (timeRemaining > 0.0f)
        {
            if (Time.time > nextBurnTick)
            {
                TakeDamage(0.25f);
                nextBurnTick += 0.5f;
            }

            yield return null;
            timeRemaining -= Time.deltaTime;
        }
        isBurn = false;
        timeRemaining = 3.0f;
    }

    private void OnDead()
    {
        enemyObject.isDead = true;
        anim.SetBool("IsDead", true);
        enemyObject.currentLife = 0;
        fillLifeImage.fillAmount = 0;
        GameManager.inst.playerObject.AddCoin(coin);
        StartCoroutine(OnDeadEffect());
    }

    private IEnumerator AnimationDamage()
    {
        anim.SetBool("IsDead", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("IsDead", false);
    }

    private IEnumerator OnDeadEffect()
    {
        anim.SetBool("IsDead", true);
        yield return new WaitForSeconds(1f);
        var finalPositionY = transform.position.y - 3;
        Vector3 target = new Vector3(transform.position.x, finalPositionY, transform.position.z);
        while (transform.position.y != finalPositionY)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 1.5f * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);

    }

}

[System.Serializable]
public class EnemyObject
{
    public float movementSpeed = 4;
    public float rotationSpeed = 6;

    [Header("Life")]
    public bool isDead;
    public float maxLife = 100;
    public float currentLife = 0;
    
}
