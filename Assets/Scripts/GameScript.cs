using UnityEngine;

public class GameScript : MonoBehaviour
{
    private void Start()
    {
        Model.Game = this;
        Model.StartGame();
    }

    public GameObject LoadByName(string name0) => Resources.Load<GameObject>(name0);
    public GameObject CreateByGameObject(GameObject obj) => Instantiate(obj);
    public GameObject CreateGrid() => CreateByGameObject(LoadByName("Grid"));
    public GameObject CreatePlayer() => CreateByGameObject(LoadByName("Player"));
    public void Destroy(GameObject gObject) => Object.Destroy(gObject);
}