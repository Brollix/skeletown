using UnityEngine;

public class StartButtonProxy : MonoBehaviour
{
    public void StartTheGame()
    {
        GameFlowManager.Instance.StartGame();
    }
}
