namespace Client
{
    public interface IEnemySwitchState
    {
        void SwitchState<T>() where T : BaseEnemyState;
    }

}
