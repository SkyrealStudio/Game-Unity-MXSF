using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public enum DirectionState
    {
        MoveRight,
        MoveLeft,
    };

    public class Controller001 : MonoBehaviour
    {
        private bool _onGround = true;

        public GameObject character;
        private Rigidbody2D character_rigidbody2D;
        public float speed;
        public float force;

        private MainCharacterDominantor _mainCharacterDominantor;

        void Start()
        {
            character_rigidbody2D = character.GetComponent<Rigidbody2D>();
            _mainCharacterDominantor = gameObject.GetComponent<MainCharacterDominantor>();
        }

        void Update()
        {
            if (_onGround && Input.GetKeyDown(KeyCode.W))
                character_rigidbody2D.AddForce(new Vector2(0f, force));
            if (Input.GetKey(KeyCode.A))
                character.gameObject.transform.Translate(new Vector2(-speed, 0f));

            if (Input.GetKey(KeyCode.S))
                {}//

            if(Input.GetKey(KeyCode.D))
                character.gameObject.transform.Translate(new Vector2(speed, 0f));
            if (Input.GetKey(KeyCode.J) && _mainCharacterDominantor.generalTaskStack.Count>0)
                { _mainCharacterDominantor.generalTaskStack.Peek().Execute(); }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "Ground")
                _onGround = true;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "Ground")
                _onGround = false;
        }
    }
}
