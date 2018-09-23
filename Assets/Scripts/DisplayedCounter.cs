using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[DisallowMultipleComponent]
public class DisplayedCounter : MonoBehaviour
{

	[NonEditable, SerializeField] private int _count;

	private Text _text;
	private string _prefix;

	private void Awake()
	{
		
		_text = GetComponent<Text>();
		_prefix = string.Concat(_text.text.Split(Chars.Colon).FirstOrDefault(), Chars.Colon, Chars.Space);
		
		DisplayCount(_count);
		
	}

	public void IncrementCount()
	{
		DisplayCount(++_count);
	}

	private void DisplayCount(int count)
	{
		_text.text = string.Concat(_prefix, count.ToString());
	}

}