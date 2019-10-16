using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateButton : MonoBehaviour
{
	

	public int quesno;
	public int Value;
	public BTType button_Type;
	Image btm_img;

	Button _button;

	// Use this for initialization
	void Start()
	{
		_button = this.GetComponent<Button>();
		btm_img = this.GetComponent<Image>();
		_button.onClick.AddListener(UpdateValue);
	}
	
	// Update is called once per frame
	void UpdateValue()
	{
		AppManager.Instance.ToggleButton(button_Type, quesno);
		AppManager.Instance.ColorButton(btm_img);
		switch (button_Type)
			{
			case BTType.neg:
				{
					AppManager.Instance.negativeQues[quesno - 1] = Value;	
				}
				break;
			case BTType.pos:
				{
					AppManager.Instance.positiveQues[quesno - 1] = Value;			
				}
				break;
			}
			
	}
}

public enum BTType
{
	none=0,
	pos=1,
	neg=2,
	emotionOpen=3,
	emotionClose=4
}

;
