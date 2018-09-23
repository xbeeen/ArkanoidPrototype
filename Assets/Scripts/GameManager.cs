using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{

	[SerializeField] private BallMover _mover;
	[SerializeField] private DisplayedCounter _rebounds;
	[SerializeField] private DisplayedCounter _losses;
	[SerializeField] private ColorTweener _paddleTweener;
	[SerializeField] private ColorTweener _lossWallTweener;
	
	private void Awake()
	{
		StartCoroutine(StartWhenMouseClicked());
	}

	private IEnumerator StartWhenMouseClicked()
	{
		
		yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
		
		if (_mover != null)
		{
			_mover.enabled = true;
		}
		else
		{
			Debug.LogError($"The {typeof(BallMover).Name} component has not been attached");
		}
		
	}

	public void IncrementReboundsCount()
	{
		
		if (_rebounds != null) 
			_rebounds.IncrementCount();

		if (_paddleTweener != null)
			_paddleTweener.StartTweening();

	}
	
	public void IncrementLossCount()
	{
		
		if (_losses != null) 
			_losses.IncrementCount();
		
		if (_lossWallTweener != null)
			_lossWallTweener.StartTweening();
		
	}

}