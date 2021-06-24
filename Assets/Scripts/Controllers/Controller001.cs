using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public class ControllerLocker
    {
        public ControllerLocker()
        {
            _isLocked = false;
        }
        public void LockFrom(object from)
        {
            lastOperator = from;
            _isLocked = true;
        }
        public void UnLockFrom(object from)
        {
            lastOperator = from;
            _isLocked = false;
        }
        public bool Value
        {
            get { return _isLocked; }
        }

        private object lastOperator;
        private bool _isLocked;
    }

    public enum DirectionState
    {
        MoveRight,
        MoveLeft,
    };

    public class Controller001 : MonoBehaviour
    {
        public ControllerLocker locker;
        private bool _onGround = true;

        public GameObject character;
        private Rigidbody2D character_rigidbody2D;
        public float speed;
        public float force;

        private MainCharacterDominantor _mainCharacterDominantor;

        void Start()
        {
            locker = new ControllerLocker();
            character_rigidbody2D = character.GetComponent<Rigidbody2D>();
            _mainCharacterDominantor = gameObject.GetComponent<MainCharacterDominantor>();
        }

        void Update()
        {
            if (!locker.Value)
            {
                if (_mainCharacterDominantor.taskStack.Count > 0 && _mainCharacterDominantor.taskStack.Top().Count == 0)
                    _mainCharacterDominantor.taskStack.PopTop();

                if (_onGround && Input.GetKeyDown(KeyCode.W))
                    character_rigidbody2D.AddForce(new Vector2(0f, force));
                if (Input.GetKey(KeyCode.A))
                    character.gameObject.transform.Translate(new Vector2(-speed, 0f));

                if (Input.GetKey(KeyCode.S))
                { }//

                if (Input.GetKey(KeyCode.D))
                    character.gameObject.transform.Translate(new Vector2(speed, 0f));
                if (Input.GetKeyDown(KeyCode.J) && _mainCharacterDominantor.taskStack.Count > 0)
                {
                    Debug.Log("1");
                    _mainCharacterDominantor.taskStack.Top().Execute();
                }
            }

            else// locker.Value == true;
            {
                if (Input.GetKeyDown(KeyCode.J) && _mainCharacterDominantor.taskStack.Top().isExecuting == false)
                {
                    if (_mainCharacterDominantor.taskStack.Count > 0 && _mainCharacterDominantor.taskStack.Top().Count > 0)
                    {
                        Debug.Log("2");
                        _mainCharacterDominantor.taskStack.Top().Execute();
                    }
                    else 
                    {
                        throw new System.Exception("This ChildQueue is Empty but latest task didn't give back the locker.Value | Input.GetKeyDown(KeyCode.J)");
                    }
                }
            }
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
