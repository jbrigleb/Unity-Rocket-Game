using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

	[SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
	[SerializeField] float period = 2f;
	// todo remove from inspector later
	[Range(0,1)][SerializeField] float movementFactor;
	
	private Vector3 startPos;
	
	// Use this for initialization
	void Start () {
		
		startPos = transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		//todo protect against 0s
		if(period <= Mathf.Epsilon){
			return;
		}
		
		float cycles = Time.time / period; //grows continually
		
		const float tau = Mathf.PI * 2f;
		float rawSinWav = Mathf.Sin(cycles*tau); //goes from -1 to +1 
		
		movementFactor = (rawSinWav / 2f)+.5f; // shifts to movement from 0 to 1
		Vector3 offset = movementVector*movementFactor;
		transform.position = startPos + offset;
		
	}
}
