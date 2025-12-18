using UnityEngine;

namespace SongLib
{
    public interface IGameInitializer
    {
        void Initialize(System.Action onCompleted);
    }
}
