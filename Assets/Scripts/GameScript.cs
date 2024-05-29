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
    public static GameObject CreateByGameObject(GameObject obj) => CreateByGameObject(obj, Model.Game.gameObject);
    public static GameObject CreateByGameObjectInCurrentGame(GameObject obj) => CreateByGameObject(obj, CurrentGame.CurrentGameObject);
    public static GameObject CreateByGameObject(GameObject obj, GameObject parent) => Instantiate(obj, parent.transform);
    public static GameObject CreateGrid() => CreateByGameObjectInCurrentGame(LoadByName("Grid"));
    public static GameObject CreatePlayer() => CreateByGameObjectInCurrentGame(LoadByName("Player/Player"));
}