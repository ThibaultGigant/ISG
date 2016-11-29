using UnityEngine;
using FYFY;
using FYFY_plugins.CollisionManager;
using FYFY_plugins.TriggerManager;

public class GameControllerSystem : FSystem
{

	Family controllers = FamilyManager.getFamily (new AllOfComponents (typeof(GameController)));

	Family collisions = FamilyManager.getFamily (new AllOfComponents (typeof(Rigidbody), typeof(InCollision3D)));
	Family explosives = FamilyManager.getFamily (new AnyOfTags ("Explosive"), new AllOfComponents (typeof(Rigidbody)));

	Family triggers = FamilyManager.getFamily (new AllOfComponents (typeof(Triggered3D)));


	Family failure = FamilyManager.getFamily (new AnyOfTags("Failure"));
	Family success = FamilyManager.getFamily (new AnyOfTags("Success"));

	int currentCheckPointIndex = 0;

	public GameControllerSystem ()
	{
		foreach (GameObject go in controllers) {
			GameController con = go.GetComponent<GameController> ();
			con.speedQueue = new LimitedQueue<float> (con.memory);
			con.accelerationQueue = new LimitedQueue<float> (con.memory);
			con.groundSpeedQueue = new LimitedQueue<float> (con.memory);
			con.altitudeQueue = new LimitedQueue<float> (con.memory);
			con.dragQueue = new LimitedQueue<float> (con.memory);
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

			updateCurrentCheckPoint (con);
			updateSpeedAndAcceleration (con);
			updateGroundSpeed (con);
			updateDrag (con);
			updateOrientations (con);
			updateAltitude (con);

			CheckGFailure (con);
			CheckDragFailure (con);
			CheckDistanceFailure (con);
			CheckCollision (con);

			CheckTriggers (con);
		}

		foreach (GameObject go in explosives) {
			Rigidbody rb = go.GetComponent<Rigidbody> ();
			rb.useGravity = false;
			rb.AddExplosionForce (rb.mass * rb.mass, rb.position, 1);
			GameObject explo = GameObjectManager.instantiatePrefab ("FireExplosion");
			explo.transform.position = rb.position;
			go.tag = "Untagged";
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


	protected void updateCurrentCheckPoint (GameController con)
	{
		int bestIndex = currentCheckPointIndex;

		float bestDistance = Vector3.Distance (con.generator.checkPoints [bestIndex].position, con.target.transform.position);
		int temp = Mathf.Min (bestIndex + 1, con.generator.checkPoints.Count - 1);
		float tempDistance = Vector3.Distance (con.generator.checkPoints [temp].position, con.target.transform.position);


		while (temp < con.generator.checkPoints.Count - 1 && tempDistance < bestDistance) {
			bestIndex = temp;
			bestDistance = tempDistance;
			temp++;
			tempDistance = Vector3.Distance (con.generator.checkPoints [temp].position, con.target.transform.position);
		}

		temp = (int)Mathf.Max (bestIndex - 1, 0);
		tempDistance = Vector3.Distance (con.generator.checkPoints [temp].position, con.target.transform.position);
		while (temp > 0 && tempDistance < bestDistance) {
			bestIndex = temp;
			bestDistance = tempDistance;
			temp++;
			tempDistance = Vector3.Distance (con.generator.checkPoints [temp].position, con.target.transform.position);
		}
		currentCheckPointIndex = bestIndex;

	}

	protected void updateSpeedAndAcceleration (GameController con)
	{
		Rigidbody rb = con.target.GetComponent<Rigidbody> ();
		con.speedQueue.Enqueue (rb.velocity.magnitude * 3.6f * 9.81f);
		con.accelerationQueue.Enqueue ((rb.velocity.magnitude * 3.6f * 9.81f - con.speed) );
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
		con.orientation = con.target.transform.rotation.x * 180f - con.generator.checkPoints [currentCheckPointIndex].orientation;
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
			Explode (con.target, con, "Too much G !   "+con.acceleration);
		}
	}

	protected void CheckDragFailure (GameController con)
	{
		if (con.drag > con.DragFailThreshold)
			Explode (con.target, con, "Too much drag !");
	}

	protected void CheckDistanceFailure (GameController con)
	{
		if (Vector3.Distance (con.target.transform.position, con.generator.checkPoints [currentCheckPointIndex].position) > con.maxTrajectoryDistance) {
			Explode (con.target, con, "You did not follow your flight plan.");
		}
	}

	protected void CheckCollision (GameController con)
	{
		foreach (GameObject go in collisions) {
			foreach(GameObject collided in go.GetComponent<InCollision3D> ().Targets){
				Rigidbody rb = collided.GetComponentInParent<Rigidbody> ();
				Debug.Log (rb.velocity.magnitude);
				if (rb.velocity.magnitude * 3.6 * 9.81f > con.MaxCollisionSpeed) {
					Explode (collided, con, "You were going too fast for landing !   "+(int)con.speed);

				}
			}
		}
	}

	protected void Explode (GameObject go, GameController con, string text)
	{
		foreach (Largable largable in go.GetComponentsInChildren<Largable> ()) {
			largable.toDrop = true;
			largable.gameObject.tag = "Explosive";
		}
		Failure (con,text);
	}

	protected void CheckTriggers (GameController con) 
	{
		foreach (GameObject go in triggers) 
		{
			WayPoint wp = go.GetComponent<WayPoint> ();
			if (con.lastWayPoint + 1 == wp.id) 
			{

				if (con.speed > wp.maxSpeed) {
					Failure (con,"You were going too fast.      " +(int)con.speed);
				} 
				else if(con.speed < wp.minSpeed) {
					Failure (con,"You were going too slow.      " +(int)con.speed);
				}
				else {
					con.lastWayPoint++;
				}
			}
			if (wp.last && con.lastWayPoint != 0) 
			{
				// Dernier waypoint, on vérifie si le joueur a passé tous les waypoints
				bool win = con.lastWayPoint == wp.id;
				if (win) {
					Success (con);
				} else {
					Failure (con,"You did not follow your flight plan.");
				}
			}
		}
	}

	public void Success(GameController con){
		if(con.done){
			return;
		}

		con.done = true;
		con.success = true;

		Debug.Log ("Success");
	}

	public void Failure(GameController con, string text){
		if(con.done){
			return;
		}
		con.failureText = text;
		con.done = true;
		Debug.Log ("Failure");
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

}