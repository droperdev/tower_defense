using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [HideInInspector] public List<Transform> waypoints = new List<Transform>();
    private int targetIndex = 1;
    public float movementSpeed = 4;
    public float rotationSpeed = 6;
    private Animator anim;

    [Header("Life")]
    public bool isDead;
    public float maxLife = 100;
    public float currentLife = 0;
    public Image fillLifeImage;
    private Transform canvasRoot;
    private Quaternion initLifeRotation;

    public int damage = 5;
    public int coin = 10;

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
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
      
        canvasRoot.transform.rotation = initLifeRotation;
        Movement();
        LookAt();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
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
        if (isDead)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetIndex].position, movementSpeed * Time.deltaTime);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rootTarget, rotationSpeed * Time.deltaTime);
    }

    #endregion

    public void TakeDamage(float damage)
    {
        var newLife = currentLife - damage;
        if (isDead)
        {
            return;
        }
        if (newLife <= 0)
        {
            OnDead();
        }

        currentLife = newLife;
        var fillValue = currentLife * 1 / maxLife;
        fillLifeImage.fillAmount = fillValue;
        //StartCoroutine(AnimationDamage());
    }

    private void OnDead()
    {
        isDead = true;
        anim.SetBool("IsDead", true);
        currentLife = 0;
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
