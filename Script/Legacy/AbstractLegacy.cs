using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractLegacy : AbstractElement
    {
        // 选择界面的描述
        public string description = "This is a Legacy";
        // 游戏开始时的描述
        public string startDescription = "No Start Description";
        // 要监视的性相或者要素名称
        public List<string> inspectElementsList = new List<string>();
        // 以哪些行动框开始游戏
        public List<string> startingVerbsIDList = new List<string>();
        // 初始具有的卡牌
        public List<string> effects = new List<string>();
        // 无论以哪个职业开始，都可以被选择
        public bool availableWithoutEndingMatch = true;
        // 可以直接从指定的面板开始行动
        public bool newStart = false;
        // 可以从哪个结局被查找到
        public List<string> fromEndingList = new List<string>();
        // 哪些职业不会成为这场游戏后的备选职业
        public List<string> excludesOnEnding = new List<string>();

        public AbstractLegacy GetNewCopy()
        {
            AbstractLegacy legacy = new AbstractLegacy();

            // 继承自 AbstractElement 的字段
            legacy.stringIndex = this.stringIndex;
            legacy.index = this.index;
            legacy.label = this.label;
            legacy.lore = this.lore;
            legacy.comment = this.comment;

            // AbstractLegacy 自身的字段
            legacy.startDescription = this.startDescription;
            legacy.inspectElementsList = new List<string>(this.inspectElementsList);
            legacy.startingVerbsIDList = new List<string>(this.startingVerbsIDList);
            legacy.effects = new List<string>(this.effects);
            legacy.availableWithoutEndingMatch = this.availableWithoutEndingMatch;
            legacy.newStart = this.newStart;
            legacy.fromEndingList = new List<string>(this.fromEndingList);
            legacy.excludesOnEnding = new List<string>(this.excludesOnEnding);

            return legacy;
        }
    }
}