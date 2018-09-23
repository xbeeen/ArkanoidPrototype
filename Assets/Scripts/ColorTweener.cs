using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[DisallowMultipleComponent]
public class ColorTweener : MonoBehaviour
{
	
	[SerializeField] private Color _targetColor = Color.white;
	[SerializeField, Range(0f, 5f)] private float _duration = 0.5f;
	[SerializeField, Range(0, 3)] private int _repeatCount = 1;
	
	private Material _material;
	private Color _originalColor;
	private Coroutine _coroutine;
	private float _time;
	
	private bool Enabled => _material != null && _duration > 0 && _repeatCount > 0;
	
	private void Awake()
	{
		
		_material = GetComponent<Renderer>().material;
		
		if (_material != null)
		{
			_originalColor = _material.color;
		}
		else
		{
			Debug.Log("The Material has not been set");
		}
		
	}
	
	public void StartTweening()
	{
		
		if (Enabled)
		{
			
			if (_coroutine != null)
				StopTweening();
			
			_coroutine = StartCoroutine(UpdateColor());
				
		}
		
	}

	private void StopTweening()
	{
		
		StopCoroutine(_coroutine);
		_coroutine = null;

		Reset();

	}

	private void Reset()
	{
		
		_time = 0;
		
		if (_material != null) 
			_material.color = _originalColor;
		
	}
	
	private IEnumerator UpdateColor()
	{
		
		while (Enabled)
		{
			
			_time += Time.deltaTime;

			if (_time < _duration)
			{
				
				_material.color
					= Color.Lerp(_originalColor, _targetColor, Mathf.PingPong(_time, 0.5f * _duration / _repeatCount));
				
				yield return null;
				
			}
			else
			{
				break;
			}
			
		}
		
		StopTweening();
		
	}

}