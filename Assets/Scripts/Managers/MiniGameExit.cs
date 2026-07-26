using UnityEngine;

public class MiniGameExit : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.ReturnToSalon();
            }
            else
            {
                Debug.LogError("SceneLoader bulunamadı! Salonu ArcadeSalon'dan başlatman gerek.");
            }
        }
    }
}