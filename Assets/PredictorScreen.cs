using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictorScreen : MonoBehaviour
{
	int[] dose = { 14, 23, 12, 20, 11, 8, 9, 9, 10, 13,
		14, 12, 8, 4, 5, 8, 9, 7, 3, 10, 11,
		9, 8, 7, 6, 4, 5, 8, 7, 3, 6, 9, 7
	};

	void Start()
	{
			Debug.Log("ascacs  "+PredictPattern());	
	}

	public float PredictPattern()
	{
		float avg1, avg2, sum1 = 0, sum2 = 0, diff, avgScore, dayScore, weekScore, alcoholScore, lapseScore = 100, feedbackScore, totalScore;
		float pSum = 0, nSum = 0;

		for (int i = dose.Length - 2; i > dose.Length - 9; i--)
			{
				sum1 += dose[i];
			}
		avg1 = sum1 / 7;

		for (int i = dose.Length - 9; i > dose.Length - 16; i--)
			{
				sum2 += dose[i];
			}
		avg2 = sum2 / 7;

        

		if (avg1 < avg2)
			{
				diff = avg2 - avg1;		
				if (diff > 0 && diff <= 1)
					{
						avgScore = 25;
					} else if (diff > 1 && diff <= 2)
					{
						avgScore = 50;
					} else if (diff > 2 && diff <= 3)
					{
						avgScore = 75;
					} else
					avgScore = 100;
			} else if (avg1 > avg2)
			{
				diff = avg1 - avg2;
				if (diff > 0 && diff <= 1)
					{
						avgScore = -25;
					} else if (diff > 1 && diff <= 2)
					{
						avgScore = -50;
					} else if (diff > 2 && diff <= 3)
					{
						avgScore = -75;
					} else
					avgScore = -100;
			} else
			avgScore = 0;
		avgScore = avgScore + 100;

			if ((dose.Length - 2) > 3 && (dose.Length - 2) < 4)
			{
				dayScore = 25;
			} else if (dose.Length - 2 > 2 && dose.Length - 2 <= 3)
			{
				dayScore = 50;
			} else if (dose.Length - 2 > 1 && dose.Length - 2 <= 2)
			{
				dayScore = 75;
			} else if (dose.Length - 2 >= 0 && dose.Length - 2 <= 1)
			{
				dayScore = 100;
			} else if (dose.Length - 2 >= 4 && dose.Length - 2 <= 5)
			{
				dayScore = -25;
			} else if (dose.Length - 2 > 5 && dose.Length - 2 <= 6)
			{
				dayScore = -50;
			} else if (dose.Length - 2 > 6 && dose.Length - 2 <= 7)
			{
				dayScore = -75;
			} else if (dose.Length - 2 >= 7)
			{
				dayScore = -100;
			} else
			dayScore = 0;
		dayScore = dayScore + 100;

		if (sum1 > 10 && sum1 < 14)
			{
				weekScore = 25;
			} else if (sum1 > 7 && sum1 <= 10)
			{
				weekScore = 50;
			} else if (sum1 > 4 && sum1 <= 7)
			{
				weekScore = 75;
			} else if (sum1 > 0 && sum1 <= 4)
			{
				weekScore = 100;
			} else if (sum1 >= 14 && sum1 <= 17)
			{
				weekScore = -25;
			} else if (sum1 > 17 && sum1 <= 21)
			{
				weekScore = -50;
			} else if (sum1 > 21 && sum1 <= 25)
			{
				weekScore = -75;
			} else if (sum1 > 25)
			{
				weekScore = -100;
			} else
			weekScore = 0;
		weekScore = weekScore + 100;

		alcoholScore = (float)(0.5 * avgScore + 0.15 * dayScore + 0.35 * weekScore) / 2;

		if (AppManager.Instance.lastFeedback == null)
			{
				//AppManager.Instance.lastFeedback = System.DateTime.Today;
				lapseScore = 100;
			} else
			{
				System.DateTime yesterday = System.DateTime.Now.Date.AddDays(-1);
				if (yesterday == AppManager.Instance.lastFeedback)
					{
						lapseScore = lapseScore + 5;
						if (lapseScore > 100)
							{
								lapseScore = 100;
							}

					} else
					{
						if (yesterday > AppManager.Instance.lastFeedback)
							{
								lapseScore = lapseScore - 5;
							}
					}
					
			}
		feedbackScore = (AppManager.Instance.latestFeedbackScore + lapseScore) / 2;
		totalScore = (float)(0.75 * alcoholScore + 0.25 * feedbackScore) / 2;
		return totalScore;
	}
}
