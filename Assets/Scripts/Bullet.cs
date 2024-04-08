using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        private float speed;
        private GameController gameController;
        private LayerMask mask;

        private Vector3 dir;


        public void Shoot(Vector3 position, Vector3 eulerAngles, float speed, GameController gameController,
            LayerMask mask)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(0f, 0f, eulerAngles.z);
            this.speed = speed;
            this.gameController = gameController;
            this.mask = mask;
        }

        public void Shoot(Transform transformParent, float speed, GameController gameController, LayerMask mask)
        {
            Shoot(transformParent.position, transformParent.eulerAngles, speed, gameController, mask);
        }

        //TODO: можно переписать на корутину
        void FixedUpdate()
        {
            var lastPos = transform.position;

            transform.Translate(transform.right * (gameController.GameSpeed * speed * Time.fixedDeltaTime),
                Space.World);

            if (Tools.FindObjectOnLine(lastPos, transform.position, mask, out var hitted))
                CollisionLogic(hitted);
        }



        void CollisionLogic(GameObject other)
        {
            Destroy(this.GameObject());
        }

        //TODO: можно переписать на расширения класса Physics2D
        // bool FindObjectOnLine(Vector3 startPosition, Vector3 endPosition, int mask, out GameObject result)
        // {
        //     result = Physics2D.Linecast(startPosition, endPosition, mask).transform.GameObject();
        //     return result is not null;
        // }
    }
}