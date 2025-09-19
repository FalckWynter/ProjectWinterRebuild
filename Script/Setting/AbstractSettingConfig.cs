using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlentyFishFramework
{
    public abstract class AbstractSettingConfigBase
    {
        public string label;
        public string comment;
        public string tabID;

        public abstract object GetValue();
        public abstract float GetValueAsFloat();
        public abstract int GetValueAsInt();
        public abstract string GetValueAsString();

        public abstract void SetValue(object value);

        public UnityEvent OnValueChanged = new UnityEvent();
    }

    public class AbstractSettingConfig<T> : AbstractSettingConfigBase
    {
        public T value;
        public T minValue;
        public T maxValue;
        public AbstractSettingConfig()
        {

        }
        public AbstractSettingConfig(T defaultValue)
        {
            value = defaultValue;
        }
        public AbstractSettingConfig(T defaultValue, T min = default, T max = default)
        {
            value = defaultValue;
            minValue = min;
            maxValue = max;
        }
        public override float GetValueAsFloat()
        {
            try
            {
                // string 不能转数值 -> 抛异常
                if (typeof(T) == typeof(string))
                    throw new InvalidCastException("String type cannot be converted to float.");

                return Convert.ToSingle(value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    $"Failed to convert {typeof(T).Name} value to float. Details: {ex.Message}");
            }
        }

        public override int GetValueAsInt()
        {
            try
            {
                if (typeof(T) == typeof(string))
                    throw new InvalidCastException("String type cannot be converted to int.");

                return Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    $"Failed to convert {typeof(T).Name} value to int. Details: {ex.Message}");
            }
        }

        public override string GetValueAsString()
        {
            try
            {
                // string 类型直接返回
                return value?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    $"Failed to convert {typeof(T).Name} value to string. Details: {ex.Message}");
            }
        }
        public override object GetValue()
        {
            return value;
        }

        public override void SetValue(object newValue)
        {
            value = (T)newValue;
            OnValueChanged.Invoke();
        }
    }
    //public class AbstractSettingConfig
    //{
    //    public string label, comment;
    //    // 根据这个ID设置值
    //    public string tabID;
    //    public string stringValue = "Null";
    //    public float floatValue = 0.5f;
    //    public int intValue = 1;
    //    public float maxValue = 1, minValue = 0;
    //    public ValueType valueType = ValueType.Int;
    //    public UnityEvent OnValueChanged = new UnityEvent();
    //    public object currentValue
    //    {
    //        get
    //        {
    //            switch (valueType)
    //            {
    //                case ValueType.String:
    //                    return stringValue;
    //                case ValueType.Float:
    //                    return floatValue;
    //                case ValueType.Int:
    //                    return intValue;
    //            }
    //            return stringValue;
    //        }
    //        set
    //        {
    //            switch (valueType)
    //            {
    //                case ValueType.String:
    //                    stringValue = value.ToString();
    //                    break;
    //                case ValueType.Float:
    //                    floatValue = (float)value;
    //                    break;
    //                case ValueType.Int:
    //                    intValue = (int)value;
    //                    break;
    //            }
    //            OnValueChanged.Invoke();
    //        }
    //    }
    //    public enum ValueType
    //    {
    //        String,Float,Int
    //    }
    //}
}