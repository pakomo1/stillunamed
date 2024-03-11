using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
   public enum Scene
    {
        Scene,
        GameScene
    }

    private static Scene targetScene;

    public static void Load(Scene targetscene)
    {
        Loader.targetScene = targetscene;
        SceneManager.LoadScene(Loader.targetScene.ToString());
    }
    public static void LoadNetwrok(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }
}
