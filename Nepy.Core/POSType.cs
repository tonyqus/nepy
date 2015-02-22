using System;
using System.Collections.Generic;

using System.Text;
using System.ComponentModel;

namespace Nepy.Core
{
        //[TypeConverter(typeof(EnumToStringUsingDescription))]
    /// <summary>
    /// Part of Speech enums
    /// </summary>
    [Flags]
    public enum POSType : int
    {
        /// <summary>
        /// newline
        /// </summary>
        NEWLINE = -1,
        /// <summary>
        /// 形容词 形语素
        /// </summary>
        [Description("形容词")]
        D_A = 0x40000000,	

        /// <summary>
        /// 区别词 区别语素
        /// </summary>
        [Description("区别词")]
        D_B = 0x20000000,	

        /// <summary>
        /// 连词 连语素
        /// </summary>
        [Description("连词")]
        D_C = 0x10000000,	

        /// <summary>
        /// 副词 副语素
        /// </summary>
        [Description("副词")]
        D_D = 0x08000000,	

        /// <summary>
        /// 叹词 叹语素
        /// </summary>
        [Description("叹词")]
        D_E = 0x04000000,	

        /// <summary>
        /// 方位词 方位语素
        /// </summary>
        [Description("方位词")]
        D_F = 0x02000000,	

        /// <summary>
        /// 成语
        /// </summary>
        [Description("成语")]
        D_I = 0x01000000,	

        /// <summary>
        /// 习语
        /// </summary>
        [Description("习语")]
        D_L = 0x00800000,	

        /// <summary>
        /// 数词 数语素
        /// </summary>
        [Description("数词")]
        A_M = 0x00400000,	

        /// <summary>
        /// 数量词
        /// </summary>
        [Description("数量词")]
        D_MQ = 0x00200000,	

        /// <summary>
        /// 名词 名语素
        /// </summary>
        [Description("名词")]
        D_N = 0x00100000,	

        /// <summary>
        /// 拟声词
        /// </summary>
        [Description("拟声词")]
        D_O = 0x00080000,	

        /// <summary>
        /// 介词
        /// </summary>
        [Description("介词")]
        D_P = 0x00040000,	

        /// <summary>
        /// 量词 量语素
        /// </summary>
        [Description("量词")]
        A_Q = 0x00020000,	

        /// <summary>
        /// 代词 代语素
        /// </summary>
        [Description("代词")]
        D_R = 0x00010000,	

        /// <summary>
        /// 处所词
        /// </summary>
        [Description("处所词")]
        D_S = 0x00008000,	

        /// <summary>
        /// 时间词
        /// </summary>
        [Description("时间词")]
        D_T = 0x00004000,	

        /// <summary>
        /// 助词 助语素
        /// </summary>
        [Description("助词")]
        D_U = 0x00002000,	

        /// <summary>
        /// 动词 动语素
        /// </summary>
        [Description("动词")]
        D_V = 0x00001000,	

        /// <summary>
        /// 标点符号
        /// </summary>
        [Description("标点符号")]
        D_W = 0x00000800,	

        /// <summary>
        /// 非语素字
        /// </summary>
         [Description("非语素字")]
        D_X = 0x00000400,	

        /// <summary>
        /// 语气词 语气语素
        /// </summary>
         [Description("语气词")]
        D_Y = 0x00000200,	

        /// <summary>
        /// 状态词
        /// </summary>
         [Description("状态词")]
        D_Z = 0x00000100,	

        /// <summary>
        /// 人名
        /// </summary>
        [Description("人名")]
        A_NR = 0x00000080,	

        /// <summary>
        /// 地名
        /// </summary>
        [Description("地名")]
        A_NS = 0x00000040,	

        /// <summary>
        /// 机构团体
        /// </summary>
        [Description("机构团体")]
        A_NT = 0x00000020,	

        /// <summary>
        /// 外文字符
        /// </summary>
        [Description("外文字符")]
        A_NX = 0x00000010,	

        /// <summary>
        /// 其他专名
        /// </summary>
        [Description("其他专名")]
        A_NZ = 0x00000008,	

        /// <summary>
        /// 前接成分
        /// </summary>
        [Description("前接成分")]
        D_H = 0x00000004,	

        /// <summary>
        /// 后接成分
        /// </summary>
        [Description("后接成分")]
        D_K = 0x00000002,	

        /// <summary>
        /// 未知词性
        /// </summary>
        [Description("未知")]
        UNKNOWN = 0x00000000,   
    }
    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute),
                                                       false);
            return attributes.Length == 0
                ? value.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }
    } 

}