using UnityEngine;
using System.Collections;

public class Flamer : Weapon {
    private ParticleSystem.EmissionModule flames;

    void Start() {
        flames = GetComponent<ParticleSystem>().emission;
    }

    void Update() {
        GetComponent<AudioSource>().enabled = firing;
    }

    protected override void OnBeginFiring() {
        flames.enabled = true;
    }

    protected override void OnEndFiring() {
        flames.enabled = false;
    }
}
