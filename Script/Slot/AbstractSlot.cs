using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractSlot : ICopyAble<AbstractSlot>
    {
        public int index;
        public string stringIndex, label, lore, comment;
        public int createIndex;
        public static int createIndexCounter = 0;
        public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
        private string iconname = "";
        public Sprite icon { set { artwork = value; } get { if (artwork == null) artwork = ImageDataBase.TryGetImage(iconName); return artwork; } }
        private Sprite artwork;

        public SlotMono slotMono;
        // 1.3节增加
        // 卡槽最大可容纳的元素数量 *一个没有用上的拓展性接口
        public int maxSlotItemCount = 1;
        // 已经容纳的桌面元素列表
        public List<ITableElement> stackItemList = new List<ITableElement>();
        // 卡槽拥有的卡牌
        public AbstractCard card;
        // 卡槽所属的行动框
        public AbstractVerb verb;
        // 这个卡槽被卡牌装载时，只在哪些行动框中显示
        public List<string> slotPossibleShowVerbList = new List<string>();

        // 2.1节增加
        // 是否为卡槽 如果为false，则是桌面的网格
        public bool isSlot = true;
        // 是否为行动框卡槽，如果是，会加载其中的卡牌的slot
        public bool isVerbSlot = false;
        // 是否允许元素叠加
        public bool isAllowStack = false;

        // 2.3节增加
        // 是否为事件中的卡槽，影响其分发
        public bool isRecipeSlot = false;

        // 3.1.1.4 卡槽性相要求 增加
        // 卡槽中卡牌需要满足至少一种的性相 or
        public Dictionary<string, int> requipredAspectsDictionary = new Dictionary<string, int>();
        // 卡槽中卡牌不能多于的所有性相 and
        public Dictionary<string, int> forbiddenAspectsDictionary = new Dictionary<string, int>();
        // 卡槽中卡牌必须满足的所有性相 and
        public Dictionary<string, int> essentialAspectsDictionary = new Dictionary<string, int>();

        // 3.2.1.1 磁吸与耗尽卡槽 增加
        // 卡槽是否尝试自动吸取卡牌
        public bool isGreedy = false;
        // 卡槽是否会消耗其中的卡牌
        public bool isConsumes = false;
        public AbstractSlot GetNewCopy()
        {
            return GetNewCopy(this);
        }
        public AbstractSlot GetNewCopy(AbstractSlot slot)
        {
            AbstractSlot retSlot = new AbstractSlot();
            retSlot.createIndex = createIndexCounter++;
            retSlot.index = slot.index;
            retSlot.stringIndex = slot.stringIndex;
            retSlot.label = slot.label;
            retSlot.lore = slot.lore;
            retSlot.icon = ImageDataBase.TryGetImage(slot.iconName);/* ImageDataBase.imageDataBase[card.stringIndex];*/

            retSlot.isSlot = slot.isSlot;
            retSlot.isVerbSlot = slot.isVerbSlot;
            retSlot.isAllowStack = slot.isAllowStack;
            retSlot.slotPossibleShowVerbList = new List<string>(slot.slotPossibleShowVerbList);
            retSlot.isRecipeSlot = slot.isRecipeSlot;
            retSlot.requipredAspectsDictionary = slot.requipredAspectsDictionary;
            retSlot.forbiddenAspectsDictionary = slot.forbiddenAspectsDictionary;
            retSlot.essentialAspectsDictionary = slot.essentialAspectsDictionary;
            retSlot.isGreedy = slot.isGreedy;
            retSlot.isConsumes = slot.isConsumes;
            return retSlot;
        }
    }
}