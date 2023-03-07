using UnityEngine;

public class Node : MonoBehaviour
{
    public Color normalColor;
    public Color highlightColor;
    private Renderer rend;
    
    private bool isThereATower = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = normalColor;
    }

    void OnMouseEnter()
    {   
        if(isThereATower) return;
        rend.material.color = highlightColor; 
    }

    void OnMouseExit()
    {
        rend.material.color = normalColor; 
    }

    public void setIsThereATower(bool isThereATower)
    {
        this.isThereATower = isThereATower;
    }

    public bool getIsThereATower()
    {
       return isThereATower;
    }

}
