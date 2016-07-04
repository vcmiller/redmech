using UnityEngine;
using System.Collections;

public class DamagePlayerOnHit : MonoBehaviour {
    public float damage;

    void OnCollisionEnter(Collision col) {
        Mech mech = col.transform.GetComponent<Mech>();
        if (mech != null) {
            mech.Damage(damage);
        }
    }
}
