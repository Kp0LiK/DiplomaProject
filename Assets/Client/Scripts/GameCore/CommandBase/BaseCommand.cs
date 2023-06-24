using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class BaseCommand
    {
        public abstract void Execute();
        public abstract void Undo();
    }
}