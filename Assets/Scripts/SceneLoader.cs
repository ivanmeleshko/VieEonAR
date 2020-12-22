using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{

    [SerializeField]
    int SceneIndex;
    private GameObject SceneSpace;


    public void LoadScene(int sceneIndex)
    {

        if (sceneIndex == 10)
        {
            SceneManager.LoadSceneAsync(10);
            return;
        }

        if (SceneManager.GetAllScenes().Length > 1)
        {
            foreach (Scene s in SceneManager.GetAllScenes())
            {
                if (!s.name.Equals("Anchors"))
                {
                    Settings.instance.previousScene = s.name;
                }
            }
        }
        else
        {
            Settings.instance.previousScene = "Anchors";
        }

    
        if ((sceneIndex == 1) && Settings.instance.previousScene.Equals("Anchors"))
        {
            SceneSpace = GameObject.Find("SceneSpace");
            SceneSpace.SetActive(false);
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            Settings.instance.currentScene = SceneManager.GetSceneByBuildIndex(sceneIndex).name;
            //CloseMenu();
        }
        else
        {
            foreach (Scene s in SceneManager.GetAllScenes())
            {
                if (!s.name.Equals("Anchors"))
                {
                    SceneManager.UnloadSceneAsync(s);
                }
            }

            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            Settings.instance.currentScene = SceneManager.GetSceneByBuildIndex(sceneIndex).name;
        }
    }


    public void OnMouseUp()
    {
        LoadScene(SceneIndex);
    }


    public void Back()
    {
        Settings.instance.back = true;
        foreach (Scene s in SceneManager.GetAllScenes())
            if (!s.name.Equals("Anchors"))
                SceneManager.UnloadSceneAsync(s);
        GameObject sceneContainer = GameObject.Find("SceneContainer");
        SceneSpace = FindObject(sceneContainer, "SceneSpace");
        SceneSpace.SetActive(true);
    }


    public GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

}

