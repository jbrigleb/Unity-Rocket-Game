﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;
	
	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip deathSound;
	[SerializeField] AudioClip levelClearSound;
	
	private Rigidbody rigidBody;
	private AudioSource audioSource;
	
	enum State { Alive, Dying, Transcending };
	State state = State.Alive;
	
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.Alive){
			RespondToThrustInput();
			RespondToRotateInput();
		}
	}
	
	void OnCollisionEnter(Collision collision){
		
		if(state != State.Alive){
			return;
		}
		
		switch ( collision.gameObject.tag ){
			
			case "Friendly":
				//do nothing
				break;
				
			case "Finish":
				//Landing Pad Reached
				//End Level; Load next
				StartSuccessSequence();
				break;
			
			default:
				//fail state; character has died
				//Load first level
				StartDeathSequence();
				break;		
		}
	}
	
	private void StartSuccessSequence(){
		
		state = State.Transcending;
		audioSource.Stop();
		audioSource.PlayOneShot(levelClearSound,.5f);		
		
		Invoke("LoadNextLevel",1f);
	}
	
	private void StartDeathSequence(){
		
		state = State.Dying;
		audioSource.Stop();
		audioSource.PlayOneShot(deathSound,.5f);

		Invoke("LoadFirstLevel",1f);

	}
	
	private void LoadNextLevel(){
		SceneManager.LoadScene(1);
	}
	
	private void LoadFirstLevel(){
		SceneManager.LoadScene(0);	
	}
	
	private void RespondToRotateInput(){
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
	private void RespondToThrustInput(){
		//get thrust
		
		//float thrustScaler = mainThrust * Time.deltaTime;
		if(Input.GetKey(KeyCode.Space)){
			ApplyThrust();
		}
		else{
			audioSource.Stop();
		}
	}
	
	private void ApplyThrust(){
		
		rigidBody.AddRelativeForce(Vector3.up * mainThrust);
		if(!audioSource.isPlaying){
			audioSource.PlayOneShot(mainEngine);
		}
		
	}
	
}
