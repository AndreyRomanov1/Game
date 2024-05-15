using UnityEngine;

public class GameScript : MonoBehaviour
{
    private void Start()
    {
        Model.Game = this;
        Model.StartGame();
    }

    public static GameObject LoadByName(string name0) => Resources.Load<GameObject>(name0);
    public static GameObject[] LoadAllByName(string name0) => Resources.LoadAll<GameObject>(name0);
    public static GameObject CreateByGameObject(GameObject obj) => Instantiate(obj);
    public static GameObject CreateGrid() => CreateByGameObject(LoadByName("Grid"));
    public static GameObject CreatePlayer() => CreateByGameObject(LoadByName("Player/Player"));
}