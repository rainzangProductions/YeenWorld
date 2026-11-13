using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObj : MonoBehaviour
{
    public int HP;
    int currentHP;
    public GameObject fracturedSelf;
    GameObject fracOBJ;
    public float breakForce;
    public float deleteTimer;
    SoundMaster mixer;
    public AudioClip breakSound;
    bool broken;

    public enum BreakType { Rock, NotRock }
    public BreakType objectType;

    void Start() {
        currentHP = HP;
        broken = false;
        mixer = FindObjectOfType<SoundMaster>();
    }
    void Update() {
        if(broken) {
            deleteTimer -= Time.deltaTime;
            if(deleteTimer <= 0f) {
                Destroy(fracOBJ);
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage() {
        currentHP--;
        if(currentHP <= 0) {
            if(objectType == BreakType.Rock) {
                Debug.LogError("ROCK");
                SplitOpen();
            }
        }
    }

    void SplitOpen() {
        mixer.PlaySFXAtPosition(breakSound, GameObject.FindGameObjectWithTag("Player").transform.position);
        
        GameObject frac = Instantiate(fracturedSelf, transform.position, transform.rotation);
        frac.transform.localScale = new Vector3(transform.localScale.x * 0.5f, transform.localScale.y * 0.5f, transform.localScale.z * 0.5f);
        fracOBJ = frac; //so I can reference this outside of the function

        foreach(Rigidbody rb in frac.GetComponentsInChildren<Rigidbody>()) {
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
            rb.AddForce(force);
        }
        //Script is attached to the parent obj so I disable mesh+collision, and then I deleted the instantiated object 1 line before deleting THIS transform
        transform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<MeshCollider>().enabled = false;

        broken = true;
    }
}