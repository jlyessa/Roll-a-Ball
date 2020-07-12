using UnityEngine;

public class ObjCubes : MonoBehaviour
{
	void FixedUpdate ()
    {
        transform.Rotate(new Vector3(20, 40, 20) * Time.deltaTime);
	}
}
