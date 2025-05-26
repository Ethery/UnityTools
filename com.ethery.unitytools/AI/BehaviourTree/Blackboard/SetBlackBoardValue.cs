using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityTools.AI.BehaviourTree.Tasks
{
    public class SetBlackBoardValue : Task
    {
        public SetBlackBoardValue(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public override ETaskStatus Tick(Blackboard blackboard)
        {
            if(blackboard.GetValue(Key) != null)
            {
                blackboard.Values[Key] = Value;
                return ETaskStatus.Success;
            }
            else
            {
                blackboard.Values.Add(Key, Value);
                return ETaskStatus.Success;
            }
        }

		public override string ToString()
		{
			return $"{base.ToString()} {Value} to {Key}";
		}

		public string Key;
        public object Value;
    }
}
