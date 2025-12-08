namespace SongLib.Patterns.State
{
    /// <summary>
    /// 상태 패턴의 기본 인터페이스
    /// </summary>
    public interface IState
    {
        void OnEnter();
        void OnUpdate(float deltaTime);
        void OnExit();
    }
}