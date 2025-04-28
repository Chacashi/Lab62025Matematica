using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    [SerializeField] private Slider sliderLife;
    [SerializeField] private TMP_Text textPoints;
    [SerializeField] private PlaneController planeController;
    

    private void Start()
    {
        sliderLife.maxValue = planeController.MaxLife;
        sliderLife.value = planeController.MaxLife;
        SetTextPoints(0);
    }

    private void Update()
    {
        SetTextPoints(planeController.Points);
        ChangueValueSlider(planeController.Life);
    }


    void ChangueValueSlider( int value)
    {
        sliderLife.value = value;
    }
    void SetTextPoints(int points)
    {
        textPoints.text = "Puntos : " + points; 
    }



}
