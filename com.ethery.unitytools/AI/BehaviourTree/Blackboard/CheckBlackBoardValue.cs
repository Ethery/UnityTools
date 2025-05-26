using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityTools.AI.BehaviourTree.Tasks
{
    public class CheckBlackBoardValue : IfElseCondition
    {
        public CheckBlackBoardValue(string key, object valueToCheck, Task trueTask,Task falseTask):base(trueTask,falseTask)
        {
            Key = key;
            Value = valueToCheck;
        }

        protected override bool CheckCondition(Blackboard blackboard)
        {
            object value = blackboard.GetValue(Key);

            if(value!= null)
            {
                bool result = value.Equals(Value);
                return result;
            }
            else
            {
                return false;
            }
        }

        public string Key;
        public object Value;
    }
}
