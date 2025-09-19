using PlentyFishFramework;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSpeedManager : IController
{
    // 存储每个来源的最新设置（同一来源只保留一个）
    private Dictionary<GameSpeedExchangSource, GameSpeedExchangeArgs> speedRequests
        = new Dictionary<GameSpeedExchangSource, GameSpeedExchangeArgs>();

    // 按优先级排序时，如果同级，比较时间戳
    public class RequestWrapper
    {
        public GameSpeedExchangeArgs Args;
        public long Timestamp; // 递增的时间戳（保证同优先级时后来的覆盖前面的）
    }

    public Dictionary<GameSpeedExchangSource, RequestWrapper> requestWrappers
        = new Dictionary<GameSpeedExchangSource, RequestWrapper>();

    private long counter = 0; // 全局时间戳计数器

    // 控制哪些来源可以被允许修改速度
    private HashSet<GameSpeedExchangSource> blockedSources = new HashSet<GameSpeedExchangSource>();

    // 当前生效的速度
    public GameSpeed CurrentSpeed { get; private set; } = GameSpeed.Normal;

    public bool IsCurrentControlledBy(GameSpeedExchangSource source)
    {
        if (requestWrappers.Count == 0) return false;

        var best = requestWrappers.Values
            .OrderByDescending(r => r.Args.ControlPriorityLevel)
            .ThenByDescending(r => r.Timestamp)
            .First();

        return best.Args.GameExchangeSource == source;
    }

    /// <summary>
    /// 提交速度请求。如果来源被阻止，则忽略。
    /// </summary>
    public void RequestSpeed(GameSpeedExchangeArgs args)
    {
        if (blockedSources.Contains(args.GameExchangeSource))
            return;
        if (CurrentSpeed == GameSpeed.Paused && args.GameSpeed == GameSpeed.Paused)
        {
            args.GameSpeed = GameSpeed.Normal;
        }
        var wrapper = new RequestWrapper
        {
            Args = args,
            Timestamp = ++counter
        };

        requestWrappers[args.GameExchangeSource] = wrapper;
        RecalculateSpeed();
        this.GetModel<LevelModel>().GameSpeedExchangeEvent.Invoke(args);
        if (args.WithSFX)
        {
            if (args.GameSpeed == GameSpeed.Paused)
                AudioKit.PlaySound(AudioDataBase.TryGetAudio("UI_PauseStart"));
            else
                AudioKit.PlaySound(AudioDataBase.TryGetAudio("UI_PauseEnd"));
        }


    }

    /// <summary>
    /// 阻止某个来源修改速度。
    /// 如果该来源已有设置，将被移除。
    /// </summary>
    public void BlockSource(GameSpeedExchangSource source)
    {
        blockedSources.Add(source);
        if (requestWrappers.ContainsKey(source))
        {
            requestWrappers.Remove(source);
            RecalculateSpeed();
        }
    }

    /// <summary>
    /// 解除对来源的阻止。
    /// </summary>
    public void UnblockSource(GameSpeedExchangSource source)
    {
        blockedSources.Remove(source);
    }

    /// <summary>
    /// 清除某来源的速度请求。
    /// </summary>
    public void ClearSource(GameSpeedExchangSource source)
    {
        if (requestWrappers.ContainsKey(source))
        {
            requestWrappers.Remove(source);
            RecalculateSpeed();
        }
    }

    /// <summary>
    /// 重新计算生效的速度。
    /// </summary>
    private void RecalculateSpeed()
    {
        if (requestWrappers.Count == 0)
        {
            CurrentSpeed = GameSpeed.Normal; // 默认恢复正常
            return;
        }

        // 取优先级最高的，如果同级取最新的
        var best = requestWrappers.Values
            .OrderByDescending(r => r.Args.ControlPriorityLevel)
            .ThenByDescending(r => r.Timestamp)
            .First();
        CurrentSpeed = best.Args.GameSpeed;
        this.GetModel<LevelModel>().GameSpeedExchangeEvent.Invoke(new GameSpeedExchangeArgs() { ControlPriorityLevel = -1,GameSpeed = CurrentSpeed,GameExchangeSource = GameSpeedExchangSource.ClearSpeedRequest,WithSFX = false});
    }

    public IArchitecture GetArchitecture()
    {
        return ProjectWinterArchitecture.Interface;
    }
}
