using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Interface.Task;
using Interface.Task.Chain;
using Interface.TextParser.ReturnUnit;
using Interface.TextPhraser;

namespace Scripts
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

            OnlyInteract_InChain,
        }
        public int numLimit;

        public ControllerLocker()
        {
            _state = ControllerLockerState.Unlocked;
        }

        public void LockFrom(object from, ControllerLockerState state, int numLimit = -1)
        {
            if (numLimit == -1 && state == ControllerLockerState.OnlyNum) throw new System.Exception("no numLimit | LockFrom : " + from.ToString());
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

        public PersistentObjectManager persistentObjectManager;

        private MainCharacterDominator _mainCharacterDominator;
        private ITaskStructCarrier taskStructCarrier;

        private IParserUnitCarrier parserUnitCarrier;
        private ITaskChainNodeCarrier chainNodeCarrier_NowRunning;


        //private ITaskExecuter_Mk001 taskExecuter;
        private ITaskExecuter_Mk002 taskExecuter;
        private IVariableTaskExecuter001 variableTaskExecuter001;
        
        void Start()
        {
            locker = new ControllerLocker();
            character_rigidbody2D = character.GetComponent<Rigidbody2D>();

            _mainCharacterDominator = gameObject.GetComponent<MainCharacterDominator>();

            parserUnitCarrier = _mainCharacterDominator;
            chainNodeCarrier_NowRunning = _mainCharacterDominator;

            taskStructCarrier = _mainCharacterDominator;
            //taskExecuter = _mainCharacterDominator;
            variableTaskExecuter001 = _mainCharacterDominator;
        }

        private void Normal_PlayerInteract()
        {
            if (!_mainCharacterDominator.isExecuting) return;
            if (parserUnitCarrier.GetTaskEntranceStruct().Count == 0) return;

            Unit_Mk004 unit = chainNodeCarrier_NowRunning.GetTaskChainNode();

            unit = parserUnitCarrier.GetTaskEntranceStruct().Dequeue().data;

            taskExecuter.ExecuteTaskAsync(persistentObjectManager.parserTranslator.Translate(unit));

            unit = persistentObjectManager.parserUnitModifier.Modify(unit);

            //qwq我也没想到会写这么复杂啊啊啊啊啊啊啊
            //20210811 TODO:求ycMia至少对接一下剧本解析

            //20210817 checkout to branch dev-ycMia
        }

        private void Entrance_PlayerInteract()
        {
            
        }

        void Update()
        {
            switch (locker.Value)
            {
                case ControllerLocker.ControllerLockerState.Unlocked:
                    //if (taskStructCarrier.GetTaskStruct().Count > 0 && taskStructCarrier.GetTaskStruct().Top().Count == 0)
                    //{
                    //    taskStructCarrier.GetTaskStruct().PopTop();
                    //}

                    if (_onGround && Input.GetKeyDown(KeyCode.W))
                        character_rigidbody2D.AddForce(new Vector2(0f, force));
                    if (Input.GetKey(KeyCode.A))
                        character.gameObject.transform.Translate(new Vector2(-speed * Time.deltaTime, 0f));

                    if (Input.GetKey(KeyCode.S))
                    { }//

                    if (Input.GetKey(KeyCode.D))
                        character.gameObject.transform.Translate(new Vector2(speed * Time.deltaTime, 0f));
                    if (Input.GetKeyDown(KeyCode.J))
                    {
                        Entrance_PlayerInteract();
                    }
                    break;

                case ControllerLocker.ControllerLockerState.OnlyInteract:
                    if (Input.GetKeyDown(KeyCode.J))
                    {
                        //if (_mainCharacterDominator.isExecuting == false)
                        //{
                        //    if (taskStructCarrier.GetTaskStruct().Count > 0 && taskStructCarrier.GetTaskStruct().Top().Count > 0)
                        //    {
                        //        //Debug.Log("2");
                        //        taskExecuter.ExecuteTask();
                        //    }
                        //    else
                        //    {
                        //        throw new System.Exception("This ChildQueue is Empty but latest task didn't give back the locker.Value | Input.GetKeyDown(KeyCode.J)");
                        //    }
                        //}
                        //else
                        //{   }
                    }
                    break;
                    
                case ControllerLocker.ControllerLockerState.OnlyInteract_InChain:
                    if (Input.GetKey(KeyCode.J))
                    {
                        //if (_mainCharacterDominator.isExecuting == false)
                        //{
                        //    ref Unit_Mk004 node = ref _mainCharacterDominator.GetTaskChainNode();
                        //    if (node.Contents.Length == 0)
                        //    {
                        //        throw new System.NotImplementedException();
                        //    }
                        //    else
                        //    {
                        //        if (node.Style == TextStyle.plain)
                        //        {
                        //            _mainCharacterDominator.ExecuteTask_Mk002(persistentObjectManager.paserTranslator.Translate(node));
                        //            node = persistentObjectManager.parserUnitModifier.Modify(node);
                        //        }
                        //        else
                        //        {
                        //            throw new System.NotImplementedException();
                        //        }
                        //    }
                        //}
                        //else
                        //{   }
                    }
                    break;


                case ControllerLocker.ControllerLockerState.OnlyNum:
                    if (Input.GetKeyDown(KeyCode.Alpha1) && locker.numLimit >= 0)
                    {
                        if (!_mainCharacterDominator.isExecuting)
                            persistentObjectManager.tipTextBoxBranch.LightUP(0);/**/
                                                                                //_mainCharacterDominantor.GetTaskStruct().Top().
                        variableTaskExecuter001.ExecuteVariableTask(0);
                        //Debug.Log("Ex0");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2) && locker.numLimit >= 1)
                    {
                        if (!_mainCharacterDominator.isExecuting)
                            persistentObjectManager.tipTextBoxBranch.LightUP(1);/**/
                        variableTaskExecuter001.ExecuteVariableTask(1);
                        //Debug.Log("Ex1");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3) && locker.numLimit >= 2)
                    {
                        if (!_mainCharacterDominator.isExecuting)
                            persistentObjectManager.tipTextBoxBranch.LightUP(2);/**/
                        variableTaskExecuter001.ExecuteVariableTask(2);
                        //Debug.Log("Ex2");
                    }
                    break;
                default: break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
                _onGround = true;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
                _onGround = false;
        }
    }
}
