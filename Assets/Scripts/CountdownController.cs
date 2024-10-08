using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private float countdown = 3f;

    void Update()
    {
        countdown -= Time.deltaTime;
        switch (countdown)
        {
            case > 0f:
                countdownText.text = countdown.ToString("0.0");
                break;
            case > -1f:
                countdownText.text = "GO";
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
