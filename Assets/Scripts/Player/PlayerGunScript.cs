using UnityEngine;

public class PlayerGunScript : MonoBehaviour
{
    private GameObject triggerPrefab;
    private GameObject currentGun;
    private GameObject gunPosition;

    private void Start()
    {
        gunPosition = transform
            .Find("bone_1").Find("bone_9")
            .Find("Pivot")
            .Find("GG плечо").Find("bone_1").Find("GG локоть").Find("bone_1")
            .Find("Gun position")
            .gameObject;

        triggerPrefab = Resources.Load<GameObject>("Other Elements/CollectionTrigger");
    }

    public void SetGun(GameObject gun)
    {
        if (currentGun is not null)
        {
            currentGun.GetComponent<BaseGunScript>().SetMode(WeaponStateEnum.Nothing);
            Instantiate(triggerPrefab, CurrentGame.CurrentGameObject.transform).GetComponent<CollectionTriggerScript>()
                .CreateTrigger(currentGun);
        }

        while (gunPosition.transform.childCount > 0)
            Destroy(gunPosition.transform.GetChild(0));

        currentGun = gun;
        currentGun.transform.parent = gunPosition.transform;
        currentGun.transform.localPosition = Vector3.zero;
        currentGun.transform.localEulerAngles = Vector3.zero;
        currentGun.GetComponent<BaseGunScript>().SetMode(WeaponStateEnum.Player);
    }
}