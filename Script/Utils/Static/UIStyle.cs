using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIStyle {
    // 密教模拟器移植 取色样本
    public enum GlowPurpose { Default, OnHover, Hint}
    public enum GlowTheme {Classic,Mansus}
    
    public static Color aspectHover = new Color32(0xDD, 0xDD, 0xDD, 0xFF);

    public static Color hoverWhite = new Color32(0xFF, 0xFF, 0xFF, 0xFF);

    public static Color brightPink = new Color32(0xFF, 0xA8, 0xEA, 0xFF);
    public static Color lightRed = new Color32(0xFF, 0x59, 0x63, 0xFF);
    public static Color lightBlue = new Color32(0x94, 0xE2, 0xEF, 0xFF);
    public static Color warmWhite = new Color32(0xFF, 0xEA, 0x77, 0xFF); // ffea77
    
    public static Color coldWhite = Color.white;
    public static Color vile = new Color32(156, 181, 76,255);


    public static Color slotPink = new Color32(0xFF, 0xA8, 0xEA, 0xFF); // new Color32(0x8E, 0x5D, 0x82, 0xFF) // DARKER HIGHLIGHT VARIANT
    public static Color slotDefault = new Color32(0x1C, 0x43, 0x62, 0xFF);
    
    public static Color textColorLight = new Color32(0x24, 0x80, 0x89, 0xFF); // 248089FF

    public static Color GetGlowColor(GlowPurpose purpose,GlowTheme theme) {
        switch (purpose) {
            case GlowPurpose.OnHover:
                return hoverWhite;
            case GlowPurpose.Default:
                if (theme == GlowTheme.Classic)
                    return lightBlue;
                else
                    return coldWhite;
            case GlowPurpose.Hint:
                if (theme == GlowTheme.Classic)
                    return coldWhite;
                else
                    return warmWhite;
            default:
                return brightPink;
        }
    }

}
