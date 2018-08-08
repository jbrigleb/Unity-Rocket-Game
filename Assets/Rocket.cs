using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;
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
			Thrust();
			Rotate();
		}
	}
	
	void OnCollisionEnter(Collision collision){
		
		if(state != State.Alive){
			return;
		}
		
		switch ( collision.gameObject.tag ){
			
			case "Friendly":
				//do nothing
				Debug.Log("friendly collision");
				break;
				
			case "Finish":
				//Landing Pad Reached
				//End Level; Load next
				
				state = State.Transcending;
				Invoke("LoadNextLevel",1f);
				
				break;
			
			default:
				//fail state; character has died
				//Load first level
				Debug.Log("you're dead");
				
				state = State.Dying;
				Invoke("LoadFirstLevel",1f);

				break;
			
			
			
		}
	}
	
	private void LoadNextLevel(){
		SceneManager.LoadScene(1);
	}
	
	private void LoadFirstLevel(){
		SceneManager.LoadScene(0);	
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
