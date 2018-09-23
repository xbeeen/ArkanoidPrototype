using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
[DisallowMultipleComponent]
public class BallMover : MonoBehaviour
{

	[SerializeField, Range(0f, 50f)] private float _maxVelocity = 25f;
	[SerializeField, Range(0f, 10f)] private float _acceleration = 2f;
	[SerializeField] private UnityEvent _onPaddleCollided;
	[SerializeField] private UnityEvent _onLossWallCollided;
	
	private Rigidbody _rigidbody;
	private float _radius;
	private float _currentVelocity;
	private Vector3 _currentDirection;
	
	private void Awake()
	{
		
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.maxAngularVelocity = float.MaxValue;
		
		_radius = GetComponent<SphereCollider>().radius;

	}
	
	private void Start()
	{
		SetValidDirection(Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * Vector3.forward);
	}
	
#if UNITY_EDITOR
	
	private void FixedUpdate()
	{
		if (_maxVelocity > 0 && (_currentVelocity > 0 || _acceleration > 0))
		{
			AssignVelocity(_acceleration * Time.fixedDeltaTime);
		}
		else if (_currentVelocity > 0)
		{
			_currentVelocity = 0;
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.Sleep();
		}
	}
	
#else

	private float? _calculatedAcceleration;
	
	private void FixedUpdate()
	{
	
		if (_calculatedAcceleration == null)
			_calculatedAcceleration = _acceleration * Time.fixedDeltaTime;

		AssignVelocity(_calculatedAcceleration.Value);

	}

#endif
	
	private void AssignVelocity(float acceleration)
	{
		
		_currentVelocity = _currentVelocity < _maxVelocity
			? Mathf.Clamp(_currentVelocity + acceleration, 0, _maxVelocity)
			: _maxVelocity;
		
		_rigidbody.velocity = _currentDirection * _currentVelocity;
		_rigidbody.angularVelocity = Vector3.Cross(Vector3.up, _currentDirection).normalized * _currentVelocity / _radius;

	}

	private void SetValidDirection(Vector3 direction)
	{
		_currentDirection = new Vector3(direction.x, 0, direction.z).normalized;
	}
	
	private void ReflectDirection(Collision collision)
	{
		SetValidDirection(Vector3.Reflect(_currentDirection, collision.contacts[0].normal));
	}

	private void ReflectDirectionWithAdditionalAngle(Collision collision)
	{
		
		var contact = collision.contacts[0];
		var contactNormal = contact.normal;
		var contactPoint = contact.point;
		var bounds = collision.collider.bounds;
		var centerPoint = bounds.center;
		var crossProduct = Vector3.Cross(Vector3.up, contactNormal);
		var dotProduct = Vector3.Dot(crossProduct, (contactPoint - centerPoint).normalized);
		var sign = Mathf.Sign(dotProduct);
		var reflectedDirection = Vector3.Slerp(contactNormal, sign * crossProduct, Mathf.Abs(dotProduct));
		var recalculatedContactNormal = (reflectedDirection - _currentDirection).normalized;
		
		SetValidDirection(Vector3.Reflect(_currentDirection, recalculatedContactNormal));

	}
	
	private void OnCollisionEnter(Collision collision)
	{
		
		var collidedObject = collision.gameObject;
		
		if (collidedObject.CompareTag(Tags.LossWall))
		{
			ReflectDirection(collision);
			_onLossWallCollided?.Invoke();
		}
		else if (collidedObject.CompareTag(Tags.Paddle))
		{
			ReflectDirectionWithAdditionalAngle(collision);
			_onPaddleCollided?.Invoke();
		}
		else
		{
			ReflectDirection(collision);
		}
		
	}
	
}