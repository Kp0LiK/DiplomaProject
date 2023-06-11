using System;
using System.Threading.Tasks;

public interface ILoadOperation
{
    string Description { get; }
    Task Load(Action onProgressEnd);
}