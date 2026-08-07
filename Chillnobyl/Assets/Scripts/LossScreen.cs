using System;
using UnityEngine;

public class LossScreen : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text lossText;

    public void SetText(string newLossReasonText)
    {
        lossText.SetText(newLossReasonText);
    }

    public void SetVisibility(bool visible)
    {
        // TODO some cool fadeout or sth
        gameObject.SetActive(visible);
    }
}
