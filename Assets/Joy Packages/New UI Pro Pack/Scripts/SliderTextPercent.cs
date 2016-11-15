
namespace NewUIProPack
{
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Displays the value of a slider as a percentage.
	/// </summary>
	public class SliderTextPercent : MonoBehaviour
	{
		[SerializeField] Text sliderText;

		public void UpdateSliderText(float value)
		{
			sliderText.text = Mathf.RoundToInt(value * 100).ToString();
		}
	}
}