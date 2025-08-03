namespace Utilities.StateMachine
{
    public interface IState<T>
    {
        public T Owner { get; set; }
        public void OnEnter();
        public void OnExit();
        public void Tick();
    }
}