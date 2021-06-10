using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject loadingScreen;
    public RectTransform progressBar;

    public Text text;
    public Text tipsText;
    public CanvasGroup alphaCanvas;
    public string[] tips;

    public bool doneLoading = false;

    private ChunkRendering chunkRendering;

    private void Awake()
    {
        loadingScreen.SetActive(false);

        instance = this;

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive); //main menu
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadGame()
    {
        loadingScreen.SetActive(true);

        StartCoroutine(GenerateTips());

        scenesLoading.Add(SceneManager.UnloadSceneAsync(1));
        scenesLoading.Add(SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive)); //game

        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadMenu()
    {
        scenesLoading.Add(SceneManager.UnloadSceneAsync(2));
        scenesLoading.Add(SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive)); //menu
    }

    float totalSceneProgress;
    float totalChunkProgress;
    public IEnumerator GetSceneLoadProgress()
    {
        for(int k = 0; k < scenesLoading.Count; k++)
        {
            while (!scenesLoading[k].isDone)
            {
                totalSceneProgress = 0;

                foreach(AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                text.text = string.Format("Loading world: {0}%", totalSceneProgress);

                progressBar.localScale = new Vector3(totalSceneProgress / 2, 1);

                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();

        StartCoroutine(GetTotalProgress());
    }

    public IEnumerator GetTotalProgress()
    {
        float totalProgress = 0;

        chunkRendering = ChunkRendering.instance;

        while (!chunkRendering.isDone)
        {
            totalChunkProgress = Mathf.Round(chunkRendering.progress * 100);
            text.text = string.Format("Loading chunks: {0}%", totalChunkProgress);

            totalProgress = (totalSceneProgress + totalChunkProgress) / 2f;

            progressBar.localScale = new Vector3(totalProgress, 1);
            yield return null;
        }

        loadingScreen.SetActive(false);
        doneLoading = true;
    }

    public int tipIndex = 0;
    private int prevTipIndex = 0;
    public IEnumerator GenerateTips()
    {
        tipsText.text = tips[Random.Range(0, tips.Length)];

        while (loadingScreen.activeInHierarchy)
        {
            while(tipIndex == prevTipIndex)
            {
                tipIndex = Random.Range(0, tips.Length);
            }

            prevTipIndex = tipIndex;

            yield return new WaitForSeconds(3f);

            for (float k = 1f; k >= 0f; k -= 0.01f)
            {
                alphaCanvas.alpha = k;

                yield return new WaitForSeconds(0.01f);
            }

            tipsText.text = tips[tipIndex];
            for (float k = 0f; k <= 1f; k += 0.01f)
            {
                alphaCanvas.alpha = k;

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
