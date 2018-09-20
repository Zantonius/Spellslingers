using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDirectionControl : MonoBehaviour {

    public bool useRelativeRotation = true;

    private Quaternion relativeRotation;

	// Use this for initialization
	void Start () {
        relativeRotation = transform.parent.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (useRelativeRotation)
            transform.rotation = relativeRotation;
	}
}
