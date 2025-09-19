using PlentyFishFramework;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSpeedManager : IController
{
    // �洢ÿ����Դ���������ã�ͬһ��Դֻ����һ����
    private Dictionary<GameSpeedExchangSource, GameSpeedExchangeArgs> speedRequests
        = new Dictionary<GameSpeedExchangSource, GameSpeedExchangeArgs>();

    // �����ȼ�����ʱ�����ͬ�����Ƚ�ʱ���
    public class RequestWrapper
    {
        public GameSpeedExchangeArgs Args;
        public long Timestamp; // ������ʱ�������֤ͬ���ȼ�ʱ�����ĸ���ǰ��ģ�
    }

    public Dictionary<GameSpeedExchangSource, RequestWrapper> requestWrappers
        = new Dictionary<GameSpeedExchangSource, RequestWrapper>();

    private long counter = 0; // ȫ��ʱ���������

    // ������Щ��Դ���Ա������޸��ٶ�
    private HashSet<GameSpeedExchangSource> blockedSources = new HashSet<GameSpeedExchangSource>();

    // ��ǰ��Ч���ٶ�
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
    /// �ύ�ٶ����������Դ����ֹ������ԡ�
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
    /// ��ֹĳ����Դ�޸��ٶȡ�
    /// �������Դ�������ã������Ƴ���
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
    /// �������Դ����ֹ��
    /// </summary>
    public void UnblockSource(GameSpeedExchangSource source)
    {
        blockedSources.Remove(source);
    }

    /// <summary>
    /// ���ĳ��Դ���ٶ�����
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
    /// ���¼�����Ч���ٶȡ�
    /// </summary>
    private void RecalculateSpeed()
    {
        if (requestWrappers.Count == 0)
        {
            CurrentSpeed = GameSpeed.Normal; // Ĭ�ϻָ�����
            return;
        }

        // ȡ���ȼ���ߵģ����ͬ��ȡ���µ�
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
