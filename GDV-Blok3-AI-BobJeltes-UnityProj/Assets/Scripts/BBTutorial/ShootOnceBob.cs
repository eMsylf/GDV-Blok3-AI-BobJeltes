using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus

[Action("MyActions/ShootOnceBob")]
[Help("FUCK JOU BEHAVIOR BRICKS")]
public class ShootOnceBob : BBUnity.Actions.GOAction
{
    // Define the input parameter "shootPoint".
    [InParam("shootPoint")]
    public Transform shootPoint;

    // Define the input parameter "bullet" (the prefab to be cloned).
    [InParam("bullet")]
    public GameObject bullet;

    // Define the input parameter velocity, and provide a default
    // value of 30.0 when used as CONSTANT in the editor.
    [InParam("velocity", DefaultValue = 30f)]
    public float velocity;

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        if (shootPoint == null)
        {
            shootPoint = gameObject.transform.Find("shootPoint");
            if (shootPoint == null)
            {
                Debug.LogWarning("shoot point not specified. ShootOnce will not work " +
                                 "for " + gameObject.name);
                return TaskStatus.FAILED;
            }
        }

        if (shootPoint == null)
        {
            Debug.Log("Shoot Point is null");
            return TaskStatus.FAILED;
        }
        if (bullet == null)
        {
            Debug.Log("Bullet is null");
            return TaskStatus.FAILED;
        }

        // Instantiate the bullet prefab.
        GameObject newBullet = Object.Instantiate(
                                    bullet, shootPoint.position,
                                    shootPoint.rotation * bullet.transform.rotation
                                ) as GameObject;
        // Give it a velocity
        if (newBullet.GetComponent<Rigidbody>() == null)
            // Safeguard test, although the rigid body should be provided by the
            // prefab to set its weight.
            newBullet.AddComponent<Rigidbody>();

        newBullet.GetComponent<Rigidbody>().velocity = velocity * shootPoint.forward;
        // The action is completed. We must inform the execution engine.
        return TaskStatus.COMPLETED;

    } // OnUpdate

} // class ShootOnce