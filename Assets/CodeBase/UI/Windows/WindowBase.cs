using System;
using UnityEngine;

namespace CodeBase.UI.Windows
{
  [RequireComponent(typeof(Canvas))]
  public class WindowBase : MonoBehaviour
  {
    public event Action OnOpen = () => {  }; 
    public event Action OnClose = () => {  }; 
    
    [SerializeField] private Canvas _canvas;

    private void Awake() =>
      OnAwake();

    protected virtual void OnAwake()
    {
    }

    public virtual void Open()
    {
      _canvas.enabled = true;
      OnOpen?.Invoke();
    }

    public virtual void Close()
    {
      _canvas.enabled = false;
      OnClose?.Invoke();
    }

    public virtual void Destroy() =>
      Destroy(gameObject);

    private void Reset() =>
      _canvas = GetComponent<Canvas>()
               ?? null;
  }
}