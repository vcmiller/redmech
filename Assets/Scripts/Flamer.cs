using UnityEngine;
using System.Collections;

public class Flamer : Weapon {
    private ParticleSystem.EmissionModule flames;
    public float dps = 80;

    void Start() {
        flames = GetComponent<ParticleSystem>().emission;
    }

    void Update() {
        GetComponent<AudioSource>().enabled = firing;

        if (firing) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10.0f)) {
                DamageScript s = hit.transform.GetComponentInParent<DamageScript>();
                if (s != null) {
                    s.damage(dps * Time.deltaTime);
                }
            }
        }
    }

    protected override void OnBeginFiring() {
        flames.enabled = true;
    }

    protected override void OnEndFiring() {
        flames.enabled = false;
    }
}
