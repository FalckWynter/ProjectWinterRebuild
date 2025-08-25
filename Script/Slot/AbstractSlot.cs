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
        // 1.3������
        // �����������ɵ�Ԫ������ *һ��û�����ϵ���չ�Խӿ�
        public int maxSlotItemCount = 1;
        // �Ѿ����ɵ�����Ԫ���б�
        public List<ITableElement> stackItemList = new List<ITableElement>();
        // ����ӵ�еĿ���
        public AbstractCard card;
        // �����������ж���
        public AbstractVerb verb;
        // ������۱�����װ��ʱ��ֻ����Щ�ж�������ʾ
        public List<string> slotPossibleShowVerbList = new List<string>();

        // 2.1������
        // �Ƿ�Ϊ���� ���Ϊfalse���������������
        public bool isSlot = true;
        // �Ƿ�Ϊ�ж��򿨲ۣ�����ǣ���������еĿ��Ƶ�slot
        public bool isVerbSlot = false;
        // �Ƿ�����Ԫ�ص���
        public bool isAllowStack = false;

        // 2.3������
        // �Ƿ�Ϊ�¼��еĿ��ۣ�Ӱ����ַ�
        public bool isRecipeSlot = false;

        // 3.1.1.4 ��������Ҫ�� ����
        // �����п�����Ҫ��������һ�ֵ����� or
        public Dictionary<string, int> requipredAspectsDictionary = new Dictionary<string, int>();
        // �����п��Ʋ��ܶ��ڵ��������� and
        public Dictionary<string, int> forbiddenAspectsDictionary = new Dictionary<string, int>();
        // �����п��Ʊ���������������� and
        public Dictionary<string, int> essentialAspectsDictionary = new Dictionary<string, int>();

        // 3.2.1.1 ������ľ����� ����
        // �����Ƿ����Զ���ȡ����
        public bool isGreedy = false;
        // �����Ƿ���������еĿ���
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