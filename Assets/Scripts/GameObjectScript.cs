using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectScript : MonoBehaviour
{
    public AudioClip despawnSfx;
    private AudioSource audioSource;
    public float force = 20f;
    public string eatenBy;
    public string eatenBy2;
    public bool isBin = false;
    public bool isFolder = false; 
    public bool isDoc = false; 
    private GameObject targetObject; // Target for folders to move towards
    private float totalTime = 10f; // Total time for bin before disappearing
    private float timer = 0f;
    private Rigidbody2D rig;
    private enum State { Start, Idle, Die }
    private State currentState = State.Start;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.gravityScale = 0; 
        rig.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * force);
        audioSource = GetComponent<AudioSource>();
    }
    void MoveTowardsTarget(GameObject target)
    {
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            rig.AddForce(direction * force);
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Start:
                if (isDoc)
                {
                    RandomMovement();
                }
                currentState = State.Idle;
                break;
            case State.Idle:
                if (isFolder)
                {
                    MoveTowardsTarget(); 
                }
                if (isBin)
                {
                    GameObject nearestFolder = FindNearestFolder();
                    if (nearestFolder != null)
                    {
                        MoveTowardsTarget(nearestFolder);
                    }
                }
                break;
            case State.Die:
                //audioSource.PlayOneShot(despawnSfx);
                //Destroy(gameObject, 1f); Destroy object after 1 second
                ChangeStateToDie();
                break;
        }

        if (isBin)
        {
            timer += Time.deltaTime;
            if (timer > totalTime)
            {
                currentState = State.Die;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == eatenBy)
        {
            currentState = State.Die;
        }
        if (collision.gameObject.tag == eatenBy2)
        {
            currentState = State.Die;
        }
    }

    void RandomMovement()
    {
        // Apply a random force to the Rigidbody2D component to move the doc
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        rig.AddForce(randomDirection * force);
    }

    void MoveTowardsTarget()
    {
        if (targetObject == null)
        {
            FindNearestTarget("Doc"); // Ensure your docs have the "Doc" tag
        }
        else
        {
            Vector2 direction = (targetObject.transform.position - transform.position).normalized;
            rig.AddForce(direction * force);
        }
    }

    void FindNearestTarget(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (GameObject potentialTarget in targets)
        {
            float distance = Vector2.Distance(transform.position, potentialTarget.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = potentialTarget;
            }
        }

        targetObject = closestTarget;
    }

    void ChangeStateToDie()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(despawnSfx); // Play despawn sound effect
        }
        Destroy(gameObject, 0.2f); // Wait for 1 second to let the sound play before destroying the object
    }
    GameObject FindNearestFolder()
    {
        GameObject[] folders = GameObject.FindGameObjectsWithTag("Folder"); // Ensure your folders have the "Folder" tag.
        GameObject nearestFolder = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject folder in folders)
        {
            float distance = Vector2.Distance(transform.position, folder.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestFolder = folder;
            }
        }

        return nearestFolder;
    }


}