using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractLegacy : AbstractElement
    {
        // ѡ����������
        public string description = "This is a Legacy";
        // ��Ϸ��ʼʱ������
        public string startDescription = "No Start Description";
        // Ҫ���ӵ��������Ҫ������
        public List<string> inspectElementsList = new List<string>();
        // ����Щ�ж���ʼ��Ϸ
        public List<string> startingVerbsIDList = new List<string>();
        // ��ʼ���еĿ���
        public List<string> effects = new List<string>();
        // �������ĸ�ְҵ��ʼ�������Ա�ѡ��
        public bool availableWithoutEndingMatch = true;
        // ����ֱ�Ӵ�ָ������忪ʼ�ж�
        public bool newStart = false;
        // ���Դ��ĸ���ֱ����ҵ�
        public List<string> fromEndingList = new List<string>();
        // ��Щְҵ�����Ϊ�ⳡ��Ϸ��ı�ѡְҵ
        public List<string> excludesOnEnding = new List<string>();

        public AbstractLegacy GetNewCopy()
        {
            AbstractLegacy legacy = new AbstractLegacy();

            // �̳��� AbstractElement ���ֶ�
            legacy.stringIndex = this.stringIndex;
            legacy.index = this.index;
            legacy.label = this.label;
            legacy.lore = this.lore;
            legacy.comment = this.comment;

            // AbstractLegacy ������ֶ�
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