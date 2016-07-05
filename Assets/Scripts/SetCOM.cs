using UnityEngine;
using System.Collections;

public class SetCOM : MonoBehaviour {
    public Vector3 centerOfMass;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
	}
}
