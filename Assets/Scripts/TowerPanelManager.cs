using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TowerPanelManager : MonoBehaviour
{
    private Animator anim;
    public static TowerPanelManager instance;
    public GameObject prefab;
    
    private void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(instance);
        
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if(prefab == null) return;

        Tower tower = prefab.GetComponent<Tower>();
        if(GameManager.inst.playerObject.Coins < tower.towerObject.buyPrice) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("AllowedNode"))
            {
                Collider zona = hit.collider;
                Node node = zona.gameObject.GetComponent<Node>();
                if (!node.getIsThereATower())
                {
                    Vector3 allowedNodeCenter = zona.bounds.center;
                    Instantiate(prefab, allowedNodeCenter + (Vector3.up * 0.6f), Quaternion.identity);
                    node.setIsThereATower(true);
                    GameManager.inst.playerObject.DecreaseCoin(tower.towerObject.buyPrice);
                }

            }
        }
    }

    public void OnOpenTowerPanel()
    {
        //anim.SetBool("IsOpen", true);
    }

    public void OnCloseTowerPanel()
    {
        //anim.SetBool("IsOpen", false);
    }


}
