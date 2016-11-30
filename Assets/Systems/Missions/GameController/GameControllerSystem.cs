using UnityEngine;
using FYFY;
using FYFY_plugins.CollisionManager;
using FYFY_plugins.TriggerManager;
using System;

public class GameControllerSystem : FSystem
{

	Family controllers = FamilyManager.getFamily (new AllOfComponents (typeof(GameController)));

	Family collisions = FamilyManager.getFamily (new AllOfComponents (typeof(InCollision3D)), new AnyOfTags("Orientable"));
	Family explosives = FamilyManager.getFamily (new AnyOfTags ("Explosive"), new AllOfComponents (typeof(Rigidbody)));

	Family triggers = FamilyManager.getFamily (new AllOfComponents (typeof(Triggered3D)));


	Family failure = FamilyManager.getFamily (new AnyOfTags("Failure"));
	Family success = FamilyManager.getFamily (new AnyOfTags("Success"));

	public static GameController controller;

	public GameControllerSystem ()
	{
		foreach (GameObject go in controllers) {
			GameController con = go.GetComponent<GameController> ();
			con.speedQueue = new LimitedQueue<float> (con.memory);
			con.accelerationQueue = new LimitedQueue<float> (con.memory);
			con.groundSpeedQueue = new LimitedQueue<float> (con.memory);
			con.altitudeQueue = new LimitedQueue<float> (con.memory);
			con.dragQueue = new LimitedQueue<float> (con.memory);
			controller = con;
		}


	}

	protected override void onPause (int currentFrame)
	{
	}

	protected override void onResume (int currentFrame)
	{
	}

	protected override void onProcess (int familiesUpdateCount)
	{
		foreach (GameObject go in explosives) {
			Rigidbody rb = go.GetComponent<Rigidbody> ();
			//rb.useGravity = false;
			rb.AddExplosionForce (rb.mass * rb.mass, rb.position, 1);
			GameObject explo = GameObjectManager.instantiatePrefab ("FireExplosion");
			explo.transform.position = rb.position;
			go.tag = "Untagged";
		}

		foreach (GameObject go in controllers) {
			GameController con = go.GetComponent<GameController> ();
			GameObject target = con.target;

			if(con.done){
				con.timer += Time.fixedDeltaTime;
			}

			if (con.timer > con.delay){
				EndGame (con);
			}

			if(con.done){
				break;
			}
			updateSpeedAndAcceleration (con);
			updateGroundSpeed (con);
			updateDrag (con);
			updateOrientations (con);
			updateAltitude (con);

			CheckGFailure (con);
			CheckDragFailure (con);
			CheckCollision (con);

			CheckTriggers (con);
		}


	}

	void EndGame(GameController con){
	

		Time.timeScale = 0f;

		if(con.success){

			foreach(GameObject go in success){
				PopUpComponent pop = go.GetComponent<PopUpComponent> ();
				pop.display = true;
			}


		}else{
			foreach(GameObject go in failure){
				PopUpComponent pop = go.GetComponent<PopUpComponent> ();
				pop.display = true;
				pop.text = con.failureText;
			}
		}


		// App.Load(Menu);
	}

	protected void updateSpeedAndAcceleration (GameController con)
	{
		Rigidbody rb = con.target.GetComponent<Rigidbody> ();
		float newSpeed = rb.velocity.magnitude * 3.6f * 9.81f;
		con.speedQueue.Enqueue (newSpeed);
		con.accelerationQueue.Enqueue ((newSpeed - con.speed) );
		con.speed = getQueueMean (con.speedQueue);
		con.acceleration = getQueueMean (con.accelerationQueue);

	}

	protected void updateGroundSpeed (GameController con)
	{
		Rigidbody rb = con.target.GetComponent<Rigidbody> ();
		con.groundSpeedQueue.Enqueue (0f);
		con.groundSpeed = getQueueMean (con.groundSpeedQueue);
	}

	protected void updateG (GameController con)
	{
		Rigidbody rb = con.target.GetComponent<Rigidbody> ();

		con.accelerationQueue.Enqueue (rb.velocity.magnitude);
		con.speed = getQueueMean (con.speedQueue);
	}

	protected void updateDrag (GameController con)
	{
		Rigidbody rb = con.target.GetComponent<Rigidbody> ();
		float drag = .5f * rb.drag * Mathf.Pow (rb.velocity.magnitude, 2f) * (1f + Vector3.Angle (rb.velocity, rb.transform.up) / 10f);
		con.dragQueue.Enqueue (drag);
		con.drag = getQueueMean (con.dragQueue);
	}

	protected void updateOrientations (GameController con)
	{
		Rigidbody rb = con.target.GetComponent<Rigidbody> ();
		Vector3 dirGravity = -con.target.transform.position.normalized;
		Vector3 dirShuttle = con.target.transform.up.normalized;
		float angle = Vector3.Angle (dirShuttle, dirGravity) * Mathf.Sign (Vector3.Cross (dirGravity, dirShuttle).x) + 180f;
		con.earthOrientation = (angle > 180f) ? (angle - 360) : angle;
	}

	protected void updateAltitude (GameController con)
	{
		con.altitude = PhysicsConstants.GetAltitude (con.target.transform.position);
	}

	protected void CheckGFailure (GameController con)
	{

		if (con.acceleration>con.GFailThreshold){
			con.timerG += 1f / con.GFailThresholdDuration * Time.fixedDeltaTime;
		}else{
			con.timerG = 0f;
		}

		if (con.timerG > 1f) {
			Explode (con.target, "Too much acceleration ! Your G counter was: "+ String.Format ("{0:0.00}", con.acceleration));
		}
	}

	protected void CheckDragFailure (GameController con)
	{
		if (con.drag > con.DragFailThreshold)
			Explode (con.target, "Too much drag !");
	}

	protected void CheckCollision (GameController con)
	{
		foreach (GameObject go in collisions) {
			foreach(GameObject collided in go.GetComponent<InCollision3D> ().Targets){
				Rigidbody rb = collided.GetComponentInParent<Rigidbody> ();
				if (rb.velocity.magnitude * 3.6 * 9.81f > con.MaxCollisionSpeed) {
					Explode (collided, "You were going too fast for landing !   "+String.Format("{0:0.00}", con.speed));

				}
			}
		}
	}

	public static void Explode (GameObject go, string text)
	{
		Debug.Log ("Explosion de : " + go.gameObject.name);
		go.tag = "Explosive";
		foreach (Largable largable in go.GetComponentsInChildren<Largable> ()) {
			largable.toDrop = true;
			largable.gameObject.tag = "Explosive";
		}
		Failure (getGameController() ,text);
	}

	protected void CheckTriggers (GameController con) 
	{
		foreach (GameObject go in triggers) 
		{
			WayPoint wp = go.GetComponent<WayPoint> ();
			Triggered3D trig = wp.GetComponent<Triggered3D> ();
			bool orientable = false;
			foreach (GameObject test in trig.Targets) {
				if(test.CompareTag("Orientable") || test.CompareTag("Trigger"))
				{
					orientable = true;
				}
			}

			if (orientable) {
				if (con.lastWayPoint + 1 == wp.id) {

					if (con.speed > wp.maxSpeed) {
						Failure (con, "You were going too fast. " + (int)con.speed);
					} else if (con.speed < wp.minSpeed) {
						Failure (con, "You were going too slow. " + (int)con.speed);
					} else {
						con.lastWayPoint++;
					}
				}
				if (wp.last && con.lastWayPoint != 0) {
					// Dernier waypoint, on vérifie si le joueur a passé tous les waypoints
					bool win = con.lastWayPoint == wp.id;
					if (win) {
						Success (con);
					} else {
						Failure (con, "You did not follow your flight plan.");
					}
				}
			}
		}
	}

	public static void Success(GameController con){
		if(con.done){
			return;
		}

		con.done = true;
		con.success = true;

		Debug.Log ("Success");
	}

	public static void Failure(GameController con, string text){
		if(con.done){
			return;
		}
		con.failureText = text;
		con.done = true;
		Debug.Log ("Failure : " + text);
	}


	public float getQueueMean (LimitedQueue<float> q)
	{
		if (q.Count == 0)
			return 0;

		float res = 0f;
		foreach (float o in q) {
			res += (float)o;
		}

		return res /= q.Count;
	}

	public static GameController getGameController()
	{
		return controller;
	}

}