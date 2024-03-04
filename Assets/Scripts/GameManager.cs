using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioClip spawnSfx; 
    private AudioSource audioSource;
    public GameObject docPrefab, folderPrefab, trashBinPrefab;
    public float totalTime = 5f; // Time interval to spawn docs
    private float timer = 0f;
    private int docCount = 0;
    private int folderCount = 0;
    public float minX = -960f, maxX = 960f, minY = -540f, maxY = 540f; // Spawn area

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > totalTime)
        {
            timer = 0f; // Reset the timer
            SpawnObject(docPrefab, 1);
            docCount++;
            if (docCount % 5 == 0)
            {
                SpawnObject(folderPrefab, 1);
                folderCount++;
                docCount = 0; // Reset doc count after spawning a folder
                if (folderCount % 5 == 0)
                {
                    SpawnObject(trashBinPrefab, 1);
                    folderCount = 0; // Reset folder count after spawning a trash bin
                }
            }
        }
    }

    void SpawnObject(GameObject spawnObject, int number)
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);
            Instantiate(spawnObject, position, Quaternion.identity);
            audioSource.PlayOneShot(spawnSfx);
        }
    }
}
