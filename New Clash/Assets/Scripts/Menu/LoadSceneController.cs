
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadSceneController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float sliderMaxValue = 100;
    private float sliderCurrentValue = 0;
    private int increaseSpeed = 10;
    [SerializeField] private Text clueText;
    private bool changeSceneActive=false;
    private int sceneNumber;
    
    string[] clues = new string[]
    {
        "Elixir avantajını koru.",
        "Rakibin destelerini analiz et.",
        "Kartlarını dengeli kullan.",
        "Kuleleri koru.",
        "Farklı desteler dene.",
        "Zamana dikkat et..",
        "Ana kulen düşerse kaybedersin.",
        "Ana kuleyi ele geçirirsen kazanırsın.",
        "Sabırlı ol."
    };
    

    private void FixedUpdate()
    {
        SliderIncrease(sceneNumber);
    }
    
    public void LoadScene(int sceneNumber)
    {
        this.sceneNumber = sceneNumber;
        changeSceneActive = true;
        SetSliderProperties();
        StartCoroutine(ChangeSliderIncreaseSpeedByTimer(1));
        StartCoroutine(ChangeCluesByTimer(3));
    }

    #region Slider


    private void SetSliderProperties()
    {
        slider.value = sliderCurrentValue;
        slider.maxValue = sliderMaxValue;
    }

    private void SliderIncrease(int sceneNumber)
    {
        if (changeSceneActive != false)
        {
            sliderCurrentValue = Mathf.Clamp(sliderCurrentValue,0, 100);
            sliderCurrentValue += Time.deltaTime *increaseSpeed;
            slider.value = sliderCurrentValue;
            if (slider.value == sliderMaxValue)
            {
                SceneManager.LoadScene(sceneNumber);
            }
        }
    }

    private IEnumerator ChangeSliderIncreaseSpeedByTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        increaseSpeed = Random.Range(5, 40);
        StartCoroutine(ChangeSliderIncreaseSpeedByTimer(time));
    }
    #endregion

    #region Clue

    private void SetClue()
    {
        int randomIndex = Random.Range(0, clues.Length);
        clueText.text = clues[randomIndex];
    }

    private IEnumerator ChangeCluesByTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        SetClue();
    }
    

    #endregion
}
