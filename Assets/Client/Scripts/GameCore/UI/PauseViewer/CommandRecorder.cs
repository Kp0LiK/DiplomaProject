using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class CommandRecorder : MonoBehaviour
    {
        private readonly Stack<BaseCommand> _baseCommands = new Stack<BaseCommand>();

        public void Record(BaseCommand baseCommand)
        {
            _baseCommands.Push(baseCommand);
            baseCommand.Execute();
        }

        public void Rewind()
        {
            if (_baseCommands.Count == 0)
                return;

            var action = _baseCommands.Pop();
            action.Undo();
        }

        public void ActionReset() => _baseCommands.Clear();
    }   
}
