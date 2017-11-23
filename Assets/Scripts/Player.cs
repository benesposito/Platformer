using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof (Controller2D))]
public class Player : MonoBehaviour {
	public float jumpHeight = 3;
	public float timeToJumpApex = .4f;
	public float moveSpeed = 5;

	float gravity;
	float jumpVelocity;
	float acceleration;
	bool doubleJump;
	long wallTimer;
	Vector3 velocity;

	Controller2D controller;
	
	void Start () {
		controller = GetComponent<Controller2D>();

		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		acceleration = 0;
		doubleJump = false;
		wallTimer = 501;

		controller.transform.position = new Vector3(GameObject.Find("Spawn").transform.position.x, GameObject.Find("Spawn").transform.position.y, 0);
	}
	
	void Update () {
		if(controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		if(Input.GetKey(KeyCode.D)) {
			acceleration = (acceleration < 0)? acceleration + 0.2f : acceleration + 0.1f;
		} else if(Input.GetKey(KeyCode.A)) {
			acceleration = (acceleration > 0)? acceleration - 0.2f : acceleration - 0.1f;
		} else {
			acceleration = (acceleration > 0.2) ? acceleration - 0.15f : (acceleration < -0.2) ? acceleration + 0.15f : 0;
		}

		acceleration = (acceleration > 3) ? 3 : (acceleration < -3) ? -3 : acceleration;
		velocity.x = acceleration * moveSpeed;

		if (controller.collisions.below || controller.collisions.left || controller.collisions.right)
			doubleJump = true;

		if (controller.collisions.left || controller.collisions.right && wallTimer < System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond - 500)
			acceleration = 0;

		if (Input.GetKeyDown(KeyCode.W) && controller.collisions.below)
			velocity.y = jumpVelocity;
		else if (Input.GetKeyDown(KeyCode.W) && controller.collisions.left) {
			acceleration = 3;
			velocity.y = jumpVelocity;
			wallTimer = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
		}
		else if (Input.GetKeyDown(KeyCode.W) && controller.collisions.right) {
			acceleration = -3;
			velocity.y = jumpVelocity;
			wallTimer = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
		}
		else if (Input.GetKeyDown(KeyCode.W) && doubleJump) {
			velocity.y = jumpVelocity;
			doubleJump = false;
		}

		if (Input.GetKey(KeyCode.W) && controller.collisions.above)
			velocity.y = 0.1f;
		else
			velocity.y += gravity * Time.deltaTime;
		
		foreach (Collider2D colliding in controller.getColliding()) {
			if (colliding.tag == "Lava")
				controller.transform.position = new Vector3(GameObject.Find("Spawn").transform.position.x, GameObject.Find("Spawn").transform.position.y, 0);
			else if(colliding.tag == "Finish") {
				print("Switching to scene 2");
				nextScene();
			}
		}

		controller.Move(velocity * Time.deltaTime);
	}

	void nextScene() {
		string scene = (SceneManager.GetActiveScene().name == "Level 1") ? "Level 2" :
			(SceneManager.GetActiveScene().name == "Level 2") ? "End" : "";

		SceneManager.LoadScene(scene);
	}
}
