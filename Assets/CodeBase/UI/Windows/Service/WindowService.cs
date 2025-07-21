using System.Collections.Generic;
using CodeBase.Services.StaticDataService;
using CodeBase.UI.Factory;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.UI.Windows.Service
{
  public class WindowService : IWindowService, IInitializable
  {
    private WindowsConfig _windowsConfig;
    
    private readonly Dictionary<WindowId, WindowBase> _cachedWindows = new();
    private readonly IUIFactory _uiFactory;
    private readonly IStaticDataService _staticDataService;

    public WindowService(IStaticDataService staticDataService, IUIFactory uiFactory)
    {
      _staticDataService = staticDataService;
      _uiFactory = uiFactory;
    }

    public void Initialize()
    {
      _windowsConfig = _staticDataService.GetSingleConfig<WindowsConfig>();
    }

    public async UniTask Open(WindowId id)
    {
      await Open<WindowBase>(id);
    }

    public async UniTask<TWindow> Open<TWindow>(WindowId id) where TWindow : WindowBase
    {
      DebugMessageBuilder debugMsg = DLogger.Message(DSenders.UI);
      
      if (TryGetFromCache(id, out WindowBase cachedWindow))
      {
        cachedWindow.Open();
        return cachedWindow as TWindow;
      }

      WindowBase window = await CreateWindow(id);
      window.Open();
      _cachedWindows[id] = window;
      return window as TWindow;
    }

    public void Close(WindowId id)
    {
      if (TryGetFromCache(id, out WindowBase cachedWindow))
        cachedWindow.Close();
    }

    public void CloseAll()
    {
      foreach (WindowBase window in _cachedWindows.Values)
        if (window != null)
          window.Close();
    }

    public void CleanUp()
    {
      foreach (WindowBase window in _cachedWindows.Values)
        if (window != null)
          window.Destroy();

      _cachedWindows.Clear();
    }

    private async UniTask<WindowBase> CreateWindow(WindowId id)
    {
      if(!_windowsConfig)
        _windowsConfig = _staticDataService.GetSingleConfig<WindowsConfig>();
      
      return await _uiFactory.CreateWindow(_windowsConfig.WindowConfigs[id]);
    }

    private bool TryGetFromCache(WindowId id, out WindowBase window)
    {
      if (!_cachedWindows.TryGetValue(id, out WindowBase cachedWindow))
      {
        window = null;
        return false;
      }

      if (cachedWindow == null)
      {
        window = null;
        return false;
      }

      window = cachedWindow;
      return true;
    }
  }
}