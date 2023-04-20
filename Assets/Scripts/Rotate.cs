using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rotate : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Rotate360());
    }

    private IEnumerator Rotate360()
    {
        float   elapsedTime   = 0f;
        Vector3 startRotation = Vector3.zero;
        Vector3 endRotation   = new Vector3(0f, 0f, -360f);
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime / 2f;
            Vector3 lerpedRotation = Vector3.Lerp(startRotation, endRotation, elapsedTime);
            transform.rotation = Quaternion.Euler(lerpedRotation);
            
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("Gameplay");
    }
}
