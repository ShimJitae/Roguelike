using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GaugeViewer : MonoBehaviour
{
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();

    }
}
