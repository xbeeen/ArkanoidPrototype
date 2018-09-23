using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class PaddleDragging : MonoBehaviour
{
	
	[SerializeField, Range(0f, 50f)] private float _sensitivity = 40f;
	
	private Camera _camera;
	private Transform _transform;
	private Rigidbody _rigidbody;
	private float _distanceFromCamera;
	private Coroutine _dragging;

	private void Awake()
	{
		_camera = Camera.main;
		_transform = GetComponent<Transform>();
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		if (_camera != null)
		{
			_distanceFromCamera = Vector3.Distance(_transform.position, _camera.transform.position);
		}
	}

	private void Update()
	{
		
		if (Input.GetMouseButtonDown(0) && _dragging == null)
			_dragging = StartCoroutine(Drag(new WaitForFixedUpdate()));
		
		if (Input.GetMouseButtonUp(0))
			Reset();                             
		
	}

	private IEnumerator Drag(YieldInstruction instruction)
	{

		while (true)
		{
			
			var mousePosition = Input.mousePosition;

			mousePosition.z = _distanceFromCamera;

			var direction = new Vector3(_camera.ScreenToWorldPoint(mousePosition).x - _transform.position.x, 0, 0);

			_rigidbody.velocity = direction * _sensitivity;

			yield return instruction;

		}
		
	}

	private void Reset()
	{
		
		StopCoroutine(_dragging);
		_dragging = null;
			
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.Sleep();
		
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus && _dragging != null)
		{
			Reset();
		}
	}

}