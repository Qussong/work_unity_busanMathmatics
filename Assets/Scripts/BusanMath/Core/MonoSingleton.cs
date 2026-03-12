using System;
using UnityEngine;

namespace BusanMath.Core
{
    /// <summary>
    /// MonoBehaviour 기반 싱글톤 베이스 클래스
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _isApplicationQuitting = false;

        /// <summary>
        /// 싱글톤 인스턴스 프로퍼티
        /// 멀티스레드 환경에서 lock을 사용하여 중복 생성 방지
        /// </summary>
        public static T Instance
        {
            get
            {
                if (true == _isApplicationQuitting)
                {
                    return null;
                }

                lock (_lock)
                {
                    if (null == _instance)
                    {
                        _instance = FindAnyObjectByType<T>();

                        if (null == _instance)
                        {
                            GameObject singletonObj = new GameObject();
                            _instance = singletonObj.AddComponent<T>();
                            singletonObj.name = $"[Singleton] {typeof(T)}";
                            DontDestroyOnLoad(singletonObj);
                        }
                    }

                    return _instance;
                }
            }
        }

        /// <summary>
        /// 인스턴스 존재 여부 확인 (인스턴스 생성 없이)
        /// </summary>
        public static bool HasInstance => null != _instance;

        protected virtual void Awake()
        {
            if(null == _instance)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
                OnSingletonAwake();
            }
            else if(this != _instance)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 싱글톤 초기화 후 호출되는 가상 메서드
        /// 서브클래스에서 오버라이드하여 초기화 로직 작성
        /// </summary>
        protected virtual void OnSingletonAwake() { }

        protected virtual void OnDestroy()
        {
            if (this == _instance)
            {
                OnSingletonDestroy();
                _instance = null;
            }
        }

        /// <summary>
        /// 싱글톤 파괴 시 호출되는 가상 메서드
        /// 서브클래스에서 오버라이드하여 정리 로직 작성
        /// </summary>
        protected virtual void OnSingletonDestroy() { }

        /// <summary>
        /// 앱 종료 시 플래그 설정하여 종료 중 인스턴스 재생성 방지
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            _isApplicationQuitting = true;
        }

    }
}
