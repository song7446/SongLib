using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SongLib
{
    public static class AddressableManager
    {
        // 이미 로드된 에셋들의 핸들을 추적하여 중복 로딩 방지 및 메모리 해제에 사용합니다.
                private static Dictionary<string, AsyncOperationHandle> loadedAssets = new Dictionary<string, AsyncOperationHandle>();
        
                /// <summary>
                /// 어드레서블 주소를 통해 에셋을 비동기로 로드합니다.
                /// </summary>
                public static async Task<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object
                {
                    // 1. 이미 로드된 에셋이라면 캐싱된 결과를 바로 반환
                    if (loadedAssets.ContainsKey(address))
                    {
                        return loadedAssets[address].Result as T;
                    }
        
                    // 2. 비동기 로딩 시작
                    AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
                    await handle.Task; // 로딩이 끝날 때까지 여기서 대기합니다.
        
                    // 3. 로딩 성공 여부 체크
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        loadedAssets.Add(address, handle);
                        return handle.Result;
                    }
                    else
                    {
                        Debug.LogError($"[SongLib.Addressable] 에셋 로드 실패: {address}");
                        Addressables.Release(handle); // 실패한 핸들은 반드시 해제
                        return null;
                    }
                }
        
                /// <summary>
                /// 사용이 끝난 에셋을 메모리에서 해제합니다. (어드레서블은 필수!)
                /// </summary>
                public static void ReleaseAsset(string address)
                {
                    if (loadedAssets.ContainsKey(address))
                    {
                        Addressables.Release(loadedAssets[address]);
                        loadedAssets.Remove(address);
                        Debug.Log($"[SongLib.Addressable] 메모리 해제 완료: {address}");
                    }
                }
    }
}
