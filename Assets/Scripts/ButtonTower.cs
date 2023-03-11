using UnityEngine;
using UnityEngine.UI;

public class ButtonTower : MonoBehaviour
{
    public GameObject prefab;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void Update()
    {
        if (GameManager.inst.playerObject.GetCoin() < prefab.GetComponent<Tower>().towerObject.buyPrice)
        {
         /*    ColorBlock colors = button.colors;
            colors.disabledColor = disabledColor;
            button.colors  = colors; */
            button.interactable = false;
            //button.enabled = false;
        }
        else
        {
            button.interactable = true;
            //button.enabled = true;
        }


    }

    void OnClick()
    {
        TowerPanelManager towerPanel = TowerPanelManager.instance;
        towerPanel.prefab = prefab;
    }
}
