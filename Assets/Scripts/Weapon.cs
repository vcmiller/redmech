using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
    protected bool firing;
    protected bool active;

    public void SetFiring(bool f) {
        if (f && !firing) {
            firing = true;
            OnBeginFiring();
        } else if (!f && firing) {
            firing = false;
            OnEndFiring();
        }
    }

    public void SetActive(bool a) {
        if (a && !active) {
            active = true;
            OnActivate();
        } else if (!a && active) {
            active = false;
            OnDeactivate();
        }
    }

	protected virtual void OnBeginFiring() {

    }

    protected virtual void OnEndFiring() {

    }

    protected virtual void OnActivate() {

    }

    protected virtual void OnDeactivate() {

    }
}
