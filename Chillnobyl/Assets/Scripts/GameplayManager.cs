using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] LossScreen lossScreen;
    bool isGameOver = false;

    private void Awake()
    {
        lossScreen.SetVisibility(false);
    }

    public void HandleGameLoss(ReactorParameterType losingParam, bool wasTooHigh)
    {
        //TODO
        // ??stop game tick
        // ??stop movement??
        // show loss reason - explained cheesily

        if (isGameOver)
            // ignore repeated loss signals (first wins)
            return;

        isGameOver = true;

        Debug.LogWarning($"Debug msg: loss reason: {losingParam.ToString()} , wasTooHigh: {wasTooHigh}");

        string lossReasonText = GetLossReasonText(losingParam,wasTooHigh);

        lossScreen.SetText(lossReasonText);
        lossScreen.SetVisibility(true);
    }


    private string GetLossReasonText(ReactorParameterType losingParam, bool wasTooHigh)
    {
        switch (losingParam)
        {
            case ReactorParameterType.Temperature:
                if(wasTooHigh)
                    return "The reactor has overheated due to lack of chill!";
                else
                    return "The reactor has chilled out too much.";

            default:
                return $"Generic loss reason. Bully the devs to make a proper one for this combo: {losingParam.ToString()} , wasTooHigh: {wasTooHigh}";
        }
    }
}
