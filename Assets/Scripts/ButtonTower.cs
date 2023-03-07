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
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }


    }

    void OnClick()
    {
        TowerPanelManager towerPanel = TowerPanelManager.instance;
        towerPanel.prefab = prefab;
    }
}
