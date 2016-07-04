using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour {
    public GameObject explosion;
    public float boomDmg = 300;
    const float boomRad = 25f;

    void OnCollisionEnter(Collision col) {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject target in enemies)
            if ((transform.position - target.transform.position).magnitude <= boomRad) {
                target.GetComponent<DamageScript>().damage(boomDmg);
            }

        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
