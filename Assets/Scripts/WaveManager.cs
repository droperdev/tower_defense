using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{   
    public List<WaveObject> waves = new List<WaveObject>();

    public bool isWaitingForNextWave;
    public bool wavesFinish;
    public int currentWave;
    public Transform initPosition;
    
    public TextMeshProUGUI counterText;
    public GameObject buttonNextWave;

    void Start()
    {
        StartCoroutine(ProcessWave());
    }

    // Update is called once per frame
    void Update()
    {
        CheckCounterAndShowButton();
        checkCounterForNextWave();
    }

    private void checkCounterForNextWave()
    {
        if(isWaitingForNextWave && !wavesFinish)
        {
            waves[currentWave].counterToNextWave -= 1 * Time.deltaTime;
            counterText.text = waves[currentWave].counterToNextWave.ToString("00");
            if(waves[currentWave].counterToNextWave <= 0)
            {
                ChangeWave();
                Debug.Log("Set Next Wave");
            }
        }
    }

    private IEnumerator ProcessWave()
    {
        if(wavesFinish)
        {
            yield break;
        }
        isWaitingForNextWave = false;
        waves[currentWave].counterToNextWave = waves[currentWave].timeForNextWave;
        for(int i = 0; i< waves[currentWave].enemys.Count; i++)
        {
            var enemyGo = Instantiate(waves[currentWave].enemys[i], initPosition.position, initPosition.rotation);
            yield return new WaitForSeconds(waves[currentWave].timePerCreation);
        }

        isWaitingForNextWave = true;
        if(currentWave >= waves.Count - 1) 
        {
            Debug.Log("Nivel Terminado");
            wavesFinish = true;
        }
    }

    private void CheckCounterAndShowButton()
    {
        if(!wavesFinish)
        {
            buttonNextWave.SetActive(isWaitingForNextWave);
            counterText.gameObject.SetActive(isWaitingForNextWave);
        }
    }

    public void ChangeWave()
    {
        if(wavesFinish) return;

        currentWave++;
        StartCoroutine(ProcessWave());
    }
}

[System.Serializable]
public class WaveObject
{
    public float timePerCreation = 1;
    public float timeForNextWave = 10;
    [HideInInspector] public float counterToNextWave = 0;
    public List<Enemy> enemys = new List<Enemy>();
}
