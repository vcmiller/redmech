using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class DamageScript : MonoBehaviour {

    public float health;
    public GameObject Explosion;
    public AudioClip deathCry;
    public GameObject Can;
    public bool DestroyObject = false;
    private bool Exploded = false;
    // Use this for initialization
    void Start() {
        if (health <= 0)
            health = 1;
        if (GetComponent<CarAIControl>() != null)
            GetComponent<CarAIControl>().SetTarget(GameObject.FindGameObjectWithTag("Driver").transform);
    }

    // Update is called once per frame
    void Update() {
        checkLiving();
    }

    void checkLiving() {
        if (health <= 0)
            Die();
    }

    public void damage(float dmg) {
        health -= dmg;
        if (health < 0) {
            health = 0;
        }
    }

    void Die() {
        if (!Exploded) {
            Exploded = true;
            if (Can != null) {
                Instantiate(Can, transform.position, Quaternion.identity);
            }
            Instantiate(Explosion, transform.position, Quaternion.identity);
            if (deathCry != null) {
                AudioSource.PlayClipAtPoint(deathCry, transform.position);
                /*AudioSource deathKnell = gameObject.AddComponent<AudioSource>();
                deathKnell.clip = deathCry;
                deathKnell.pitch = Random.Range(2,7);
                deathKnell.volume = 50;
                deathKnell.Play();
                Destroy(deathKnell);
                print("MEMES");*/
            }

            if (DestroyObject) {
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Driver") {
            damage(50);
        }
    }
}
