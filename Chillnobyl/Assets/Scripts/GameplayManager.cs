using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] LossScreen lossScreen;
    bool isGameOver = false;
    [SerializeField] Reactor reactor;
    [SerializeField] GameObject pressToStartScreen;
    [SerializeField] GameObject loadingScreen;

    private void Awake()
    {
        lossScreen.SetVisibility(false);
        reactor = FindFirstObjectByType<Reactor>();
        if (pressToStartScreen == null)
            throw new System.Exception("Missing pressToStartScreen");

        if (loadingScreen == null)
            throw new System.Exception("Missing loadingScreen");


        loadingScreen.SetActive(true);
        pressToStartScreen.SetActive(false);
    }

    bool playerChoseToStart = false;

    private void Update()
    {
        // loading screen no longer needed (all stuff loaded in Awake(), Start() player can start with no lagspike)
        //switch to "press to start screen"
        pressToStartScreen.SetActive(true);
        loadingScreen.SetActive(false);

        if (playerChoseToStart)
            return;

        // start game when player choses to
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            pressToStartScreen.SetActive(false);
            enabled = false; /// turn off this logic
            reactor.enabled = true;
        }
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
        enabled = false;
        reactor.enabled = false;

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
                return $"Generic loss reason. \r\nBully the devs to make a proper one \r\nfor this combo: {losingParam.ToString()} , \r\nwasTooHigh: {wasTooHigh}";
        }
    }
}
