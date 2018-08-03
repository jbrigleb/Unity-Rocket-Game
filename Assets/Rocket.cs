using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;
	private Rigidbody rigidBody;
	private AudioSource audioSource;
	
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
		Thrust();
		Rotate();
		
	}
	
	void OnCollisionEnter(Collision collision){
		
		switch ( collision.gameObject.tag ){
			
			case "Friendly":
				//do nothing
				Debug.Log("friendly collision");
				break;
			
			default:
				Debug.Log("you're dead");
				break;
			
			
			
		}
	}
	
	private void Rotate(){
		//get rotation
		
		rigidBody.freezeRotation = true; //take manual control of rotation
		
		float rotationScaler = rcsThrust * Time.deltaTime;
		
		if(Input.GetKey(KeyCode.A)){
			transform.Rotate(Vector3.forward*rotationScaler);
		}else if(Input.GetKey(KeyCode.D)){
			transform.Rotate(-Vector3.forward*rotationScaler);
		}
		
		rigidBody.freezeRotation = false; //resume physics control
	}
	private void Thrust(){
		//get thrust
		
		float thrustScaler = mainThrust * Time.deltaTime;
		if(Input.GetKey(KeyCode.Space)){
			rigidBody.AddRelativeForce(Vector3.up * mainThrust);
			if(!audioSource.isPlaying){
				audioSource.Play();
			}
		}
		else{
			audioSource.Stop();
		}
	}
}
