using UnityEngine;


[AddComponentMenu("Camera/Smooth Mouse Look ")]
public class FreeCamera : MonoBehaviour
{ 
	public enum CamMode
		{
		free,
		target

		}

	public CamMode mode = CamMode.free;
	public bool Control = true;

	Vector2 _mouseAbsolute;
	Vector2 _smoothMouse;

	public Vector2 clampInDegrees = new Vector2(360, 180);
	public bool lockCursor;
	public Vector2 sensitivity = new Vector2(2, 2);
	public Vector2 smoothing = new Vector2(3, 3);
	public Vector2 targetDirection;
	public Vector2 targetCharacterDirection;


	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	public float distanceMin = .5f;
	public float distanceMax = 15f;
	public float moveSpd = 0.5f;
	
	float x = 0.0f;
	float y = 0.0f;

    bool enable = true;
	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		// Set target direction to the camera's initial orientation.
		targetDirection = transform.localRotation.eulerAngles;

	}

    void OnEnable()
    {
        GameState.OnStateChanged += StateChanged;
        GameState.OnCircuitSubstateChanged += SubstateChanged;
    }

    void OnDisable()
    {
        GameState.OnStateChanged -= StateChanged;
        GameState.OnCircuitSubstateChanged -= SubstateChanged;
    }

    void StateChanged()
    {

    }

    void SubstateChanged()
    {
        if (GameState.Instance.CurrentCircuitSubstate != GameState.CircuitEditSubstate.EditOptions)
        {
            enable = true;
        }
        else
        {
            enable = false;

        }
    }

	void LateUpdate()
	{

        if (enable)
        {
            if (Input.GetMouseButton(1))
            {

                var targetOrientation = Quaternion.Euler(targetDirection);
                var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

                // Get raw mouse input for a cleaner reading on more sensitive mice.
                var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

                // Scale input against the sensitivity setting and multiply that against the smoothing value.
                mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

                // Interpolate mouse movement over time to apply smoothing delta.
                _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
                _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

                // Find the absolute mouse movement value from point zero.
                _mouseAbsolute += _smoothMouse;

                // Clamp and apply the local x value first, so as not to be affected by world transforms.
                if (clampInDegrees.x < 360)
                    _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

                var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
                transform.localRotation = xRotation;

                // Then clamp and apply the global y value.
                if (clampInDegrees.y < 360)
                    _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

                transform.localRotation *= targetOrientation;

                var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
                transform.localRotation *= yRotation;
            }


            if (Input.GetKey("w"))
            {
                transform.Translate(0f, 0f, moveSpd);
            }

            if (Input.GetKey("s"))
            {
                transform.Translate(0f, 0f, -moveSpd);
            }

            if (Input.GetKey("a"))
            {
                transform.Translate(-moveSpd, 0f, 0f);
            }

            if (Input.GetKey("d"))
            {
                transform.Translate(moveSpd, 0f, 0f);
            }


        }
   }
    	
	
}