namespace CodeBase.Gameplay.Features.Avatar.Provider
{
  public interface IAvatarProvider
  {
    Avatar Avatar { get; }
    void SetAvatar(Avatar player);
  }
}
