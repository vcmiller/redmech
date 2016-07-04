using UnityEngine;
using System.Collections;

public class Chainsaw : Weapon {
    public float frequency = 37;
    public float amplitude = 0.25f;
    public float dps = 150;

    private Vector3 sawPos;
	
	void Start () {
        sawPos = transform.localPosition;
	}
	
	void Update () {
        GetComponent<AudioSource>().enabled = firing;
        if (firing) {
            transform.localPosition = sawPos + new Vector3(amplitude * Mathf.Sin(Time.time * frequency * Mathf.PI), 0, 0);
        }
	}

    void OnTriggerStay(Collider other) {
        if (firing) {
            DamageScript dmg = other.GetComponentInParent<DamageScript>();
            if (dmg != null) {
                dmg.damage(dps * Time.deltaTime);
            }
        }
    }
}
