using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    public void OnResetScene(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            SceneManager.LoadScene(0);
    }
}
