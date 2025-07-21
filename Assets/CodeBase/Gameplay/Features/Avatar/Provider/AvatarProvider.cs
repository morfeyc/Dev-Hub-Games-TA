using System;
using CodeBase.Utils.SmartDebug;

namespace CodeBase.Gameplay.Features.Avatar.Provider
{
  public class AvatarProvider : IAvatarProvider, IDisposable
  {
    private Avatar _avatar;

    public Avatar Avatar
    {
      get
      {
        if (!_avatar)
        {
          DLogger.Message(DSenders.Empty)
            .WithText("Attempted to access Player before it was set in PlayerProvider!")
            .WithFormat(DebugFormat.Warning)
            .Log();
        }
        
        return _avatar;
      }
      private set => 
        _avatar = value;
    }

    public void SetAvatar(Avatar player)
    {
      Avatar = player;
    }

    public void Dispose()
    {
      _avatar = null;
    }
  }
}