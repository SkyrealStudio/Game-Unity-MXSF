using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public class ControllerLocker
    {
        public enum ControllerLockerState
        {
            Unlocked,

            OnlyMove,
            OnlyInteract,
            OnlyNum,
            Interact_And_Num,
        }
        public int numLimit;
        
        public ControllerLocker()
        {
            _state = ControllerLockerState.Unlocked;
        }

        public void LockFrom(object from, ControllerLockerState state,int numLimit = -1)
        {
            if (numLimit == -1 && state == ControllerLockerState.OnlyNum) throw new System.Exception("no numLimit | LockFrom : " + from.ToString() );
            this.numLimit = numLimit;
            lastOperator = from;
            _state = state;
        }
        public void UnLockFrom(object from)
        {
            lastOperator = from;
            _state = ControllerLockerState.Unlocked;
        }

        public ControllerLockerState Value
        {
            get { return _state; }
        }

        private object lastOperator;
        private ControllerLockerState _state;
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

        public LongLifeObjectManager longLifeObjectManager;

        private MainCharacterDominantor _mainCharacterDominantor;

        void Start()
        {
            locker = new ControllerLocker();
            character_rigidbody2D = character.GetComponent<Rigidbody2D>();
            _mainCharacterDominantor = gameObject.GetComponent<MainCharacterDominantor>();
        }

        void Update()
        {
            switch (locker.Value)
            {
                case ControllerLocker.ControllerLockerState.Unlocked:
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
                        //Debug.Log("1");
                        _mainCharacterDominantor.taskStack.Top().Execute();
                    }
                    break;
                case ControllerLocker.ControllerLockerState.OnlyInteract:
                    if (Input.GetKeyDown(KeyCode.J) && _mainCharacterDominantor.taskStack.Top().isExecuting == false)
                    {
                        if (_mainCharacterDominantor.taskStack.Count > 0 && _mainCharacterDominantor.taskStack.Top().Count > 0)
                        {
                            //Debug.Log("2");
                            _mainCharacterDominantor.taskStack.Top().Execute();
                        }
                        else
                        {
                            throw new System.Exception("This ChildQueue is Empty but latest task didn't give back the locker.Value | Input.GetKeyDown(KeyCode.J)");
                        }
                    }
                    break;
                case ControllerLocker.ControllerLockerState.OnlyNum:
                    if(Input.GetKeyDown(KeyCode.Alpha1) && locker.numLimit >= 0)
                    {
                        if(!_mainCharacterDominantor.taskStack.Top().isExecuting)
                            longLifeObjectManager.tipTextBoxBranch.LightUP(0);/**/
                        //_mainCharacterDominantor.taskStack.Top().
                        _mainCharacterDominantor.taskStack.Top().ExecuteVariableTask_Path001(0);
                        Debug.Log("Ex0");
                    }
                    else if(Input.GetKeyDown(KeyCode.Alpha2) && locker.numLimit >= 1)
                    {
                        if (!_mainCharacterDominantor.taskStack.Top().isExecuting)
                            longLifeObjectManager.tipTextBoxBranch.LightUP(1);/**/
                        _mainCharacterDominantor.taskStack.Top().ExecuteVariableTask_Path001(1);
                        Debug.Log("Ex1");
                    }
                    else if(Input.GetKeyDown(KeyCode.Alpha3) && locker.numLimit >= 2)
                    {
                        if (!_mainCharacterDominantor.taskStack.Top().isExecuting)
                            longLifeObjectManager.tipTextBoxBranch.LightUP(2);/**/
                        _mainCharacterDominantor.taskStack.Top().ExecuteVariableTask_Path001(2);
                        Debug.Log("Ex2");
                    }
                    break;
                default: break;
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
