using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace CodeBase.Utils.SmartDebug
{
  public class DebugMessageBuilder
  {
    private readonly string _senderName;
    private readonly DSender _sender;

    private StringBuilder _stringBuilder = new StringBuilder();
    private string _text = string.Empty;
    private DebugFormat _format;

    private string _message;

    public DebugMessageBuilder(string senderName, DSender sender)
    {
      _senderName = senderName;
      _sender = sender;
    }

    public DebugMessageBuilder WithText(object message)
    {
      _stringBuilder.Append(message);
      _message = null;
      return this;
    }

    public DebugMessageBuilder WithFormat(DebugFormat format)
    {
      _format = format;
      return this;
    }

    public void Log([CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
      if (!PlatformAvailable(_sender.Platform) && _format != DebugFormat.Exception)
        return;

      string fileName = System.IO.Path.GetFileName(filePath);
      _text = _stringBuilder.ToString();
      _message ??= $"{_senderName}[{fileName}:{lineNumber.ToString()}] {_text}";
      
#if UNITY_EDITOR || DEBUG
      // ReSharper disable Unity.PerformanceCriticalCodeInvocation
      switch (_format)
      {
        case DebugFormat.Normal:
          Debug.Log(_message);
          break;

        case DebugFormat.Warning:
          Debug.LogWarning(_message);
          break;

        case DebugFormat.Assertion:
          Debug.LogAssertion(_message);
          break;

        case DebugFormat.Error:
          Debug.LogError(_message);
          break;

        case DebugFormat.Exception:
          Debug.LogException(new Exception(_message));
          break;
      }
      // ReSharper restore Unity.PerformanceCriticalCodeInvocation
#endif
    }

    private static bool PlatformAvailable(DebugPlatform platform)
    {
      return (Application.isEditor && platform.HasFlag(DebugPlatform.Editor)) ||
             (Debug.isDebugBuild && !Application.isEditor && platform.HasFlag(DebugPlatform.DebugBuild)) ||
             (!Debug.isDebugBuild && !Application.isEditor && platform.HasFlag(DebugPlatform.Build));
    }
  }
}