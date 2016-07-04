using UnityEngine;
using System.Collections;

public class Minigun : Weapon {
    public Transform barrels;
    public float fireRate = 30;
    public float spinMaxSpeed = 60;
    public AudioClip spinUp;
    public AudioClip spinDown;
    public float damage = 15;

    private ParticleSystem tracers;
    private CooldownTimer shoot;
    private float spinSpeed = 0;

    protected override void OnBeginFiring() {
        AudioSource.PlayClipAtPoint(spinUp, transform.position);
    }

    protected override void OnEndFiring() {
        AudioSource.PlayClipAtPoint(spinDown, transform.position);
    }

    void Start() {
        shoot = new CooldownTimer(1 / fireRate);
    }

    void Update() {
        if (firing && spinSpeed == spinMaxSpeed && shoot.Use) {
            GetComponent<ParticleSystem>().Emit(1);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit)) {
                DamageScript dmg = hit.transform.GetComponent<DamageScript>();
                if (dmg != null) {
                    dmg.damage(damage);
                }
            }
        }

        GetComponent<AudioSource>().enabled = firing && (spinSpeed == spinMaxSpeed);
        
        spinSpeed = Mathf.MoveTowards(spinSpeed, firing ? spinMaxSpeed : 0, spinMaxSpeed * Time.deltaTime * 2);
        barrels.transform.Rotate(spinSpeed * Time.deltaTime * 360, 0, 0);
    }
}
