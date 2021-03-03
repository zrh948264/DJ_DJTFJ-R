using CommonRs;
using Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HZZH.Logic.Commmon
{
    public enum InIndex : int   //输入口定义
    {
        X1, X2, X3, X4, X5, X6, X7, X8, X9, X10, X11, X12, X13, X14, X15, X16, X17, X18, X19, X20, X21, X22, X23, X24, X25, X26, X27, X28, X29, X30,
        X31, X32, X33, X34, X35, X36, X37, X38, X39, X40, X41, X42, X43, X44, X45, X46, X47, X48, X49, X50, X51, X52, X53, X54, X55, X56, X57, X58, X59, X60,
    }
    public enum OutIndex : int	//输入口定义
    {
        Y1, Y2, Y3, Y4, Y5, Y6, Y7, Y8, Y9, Y10, Y11, Y12, Y13, Y14, Y15, Y16, Y17, Y18, Y19, Y20, Y21, Y22, Y23, Y24, Y25, Y26, Y27, Y28, Y29, Y30, Y31, Y32,
    }
    public static class DeviceRsDef
    {
        #region 自动生成板卡资源列表
        /// <summary>
        /// 轴列表
        /// </summary>
        public readonly static List<AxisClass> AxisList = new List<AxisClass>();
        /// <summary>
        /// 输入列表
        /// </summary>
        public readonly static List<InputClass> InputList = new List<InputClass>();
        /// <summary>
        /// 输出列表
        /// </summary>
        public readonly static List<OutputClass> OutputList = new List<OutputClass>();

        static DeviceRsDef()
        {
            foreach (var item in typeof(DeviceRsDef).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
            {
                if (item.FieldType == typeof(AxisClass))
                {
                    AxisList.Add((AxisClass)item.GetValue(null));
                }

                if (item.FieldType == typeof(InputClass))
                {
                    InputList.Add((InputClass)item.GetValue(null));
                }

                if (item.FieldType == typeof(OutputClass))
                {
                    OutputList.Add((OutputClass)item.GetValue(null));
                }
            }

        }
        #endregion

        public static MotionCardDef MotionCard = new MotionCardDef("192.168.1.30", 8089);

        #region 轴定义
        public static AxisClass Axis_x = new AxisClass(MotionCard,0, "X轴");
        public static AxisClass Axis_y = new AxisClass(MotionCard, 1, "Y轴");
        public static AxisClass Axis_z = new AxisClass(MotionCard, 2, "Z轴");

        public static AxisClass Axis_n1 = new AxisClass(MotionCard, 3, "吸嘴1Z轴");
        public static AxisClass Axis_n2 = new AxisClass(MotionCard, 4, "吸嘴2Z轴");
        public static AxisClass Axis_n3 = new AxisClass(MotionCard, 5, "吸嘴3Z轴");
        public static AxisClass Axis_n4 = new AxisClass(MotionCard, 6, "吸嘴4Z轴");

        public static AxisClass Axis_r1 = new AxisClass(MotionCard, 7, "吸嘴1R轴");
        public static AxisClass Axis_r2 = new AxisClass(MotionCard, 8, "吸嘴2R轴");
        public static AxisClass Axis_r3 = new AxisClass(MotionCard, 9, "吸嘴3R轴");
        public static AxisClass Axis_r4 = new AxisClass(MotionCard, 10, "吸嘴4R轴");

        public static AxisClass Axis_Belt = new AxisClass(MotionCard, 11, "皮带");
        #endregion


        #region 输出定义
        public static OutputClass Q_Green = new OutputClass(MotionCard, (int)OutIndex.Y1, "绿灯");
        public static OutputClass Q_Yellow = new OutputClass(MotionCard, (int)OutIndex.Y2, "黄灯");
        public static OutputClass Q_Red = new OutputClass(MotionCard, (int)OutIndex.Y3, "红灯");
        public static OutputClass Q_Buzzer = new OutputClass(MotionCard, (int)OutIndex.Y4, "蜂鸣");

        public static OutputClass Q_vacuum1 = new OutputClass(MotionCard, (int)OutIndex.Y5, "吸嘴1真空启动");
        public static OutputClass Q_vacuum2 = new OutputClass(MotionCard, (int)OutIndex.Y6, "吸嘴2真空启动");
        public static OutputClass Q_vacuum3 = new OutputClass(MotionCard, (int)OutIndex.Y7, "吸嘴3真空启动");
        public static OutputClass Q_vacuum4 = new OutputClass(MotionCard, (int)OutIndex.Y8, "吸嘴4真空启动");

        public static OutputClass Q_Vbreak1 = new OutputClass(MotionCard, (int)OutIndex.Y9, "吸嘴1破真空启动");
        public static OutputClass Q_Vbreak2 = new OutputClass(MotionCard, (int)OutIndex.Y10, "吸嘴2破真空启动");
        public static OutputClass Q_Vbreak3 = new OutputClass(MotionCard, (int)OutIndex.Y11, "吸嘴3破真空启动");
        public static OutputClass Q_Vbreak4 = new OutputClass(MotionCard, (int)OutIndex.Y12, "吸嘴4破真空启动");

        public static OutputClass Q_Glue1 = new OutputClass(MotionCard, (int)OutIndex.Y13, "胶阀1启用");
        public static OutputClass Q_Glue2 = new OutputClass(MotionCard, (int)OutIndex.Y14, "胶阀2启用");

        public static OutputClass Q_Down1 = new OutputClass(MotionCard, (int)OutIndex.Y15, "胶阀1气缸下降");
        public static OutputClass Q_Down2 = new OutputClass(MotionCard, (int)OutIndex.Y16, "胶阀2气缸下降");

        #endregion

        #region 输入定义
        public static InputClass I_X = new InputClass(MotionCard, (int)InIndex.X1, "X轴原点");
        public static InputClass I_Y = new InputClass(MotionCard, (int)InIndex.X2, "Y轴原点");
        public static InputClass I_Z = new InputClass(MotionCard, (int)InIndex.X3, "主Z轴原点");
        
        public static InputClass I_Z1 = new InputClass(MotionCard, (int)InIndex.X4, "吸嘴1z轴原点");
        public static InputClass I_Z2 = new InputClass(MotionCard, (int)InIndex.X5, "吸嘴2z轴原点");
        public static InputClass I_Z3 = new InputClass(MotionCard, (int)InIndex.X6, "吸嘴3z轴原点");
        public static InputClass I_Z4 = new InputClass(MotionCard, (int)InIndex.X7, "吸嘴4z轴原点");

        //public static InputClass I_R1 = new InputClass(MotionCard, (int)InIndex.X8, "吸嘴1r轴原点");
        //public static InputClass I_R2 = new InputClass(MotionCard, (int)InIndex.X9, "吸嘴2r轴原点");
        //public static InputClass I_R3 = new InputClass(MotionCard, (int)InIndex.X10, "吸嘴3r轴原点");
        //public static InputClass I_R4 = new InputClass(MotionCard, (int)InIndex.X11, "吸嘴4r轴原点");

        public static InputClass I_vacuum1 = new InputClass(MotionCard, (int)InIndex.X17, "吸嘴1真空感应");
        public static InputClass I_vacuum2 = new InputClass(MotionCard, (int)InIndex.X18, "吸嘴2真空感应");
        public static InputClass I_vacuum3 = new InputClass(MotionCard, (int)InIndex.X19, "吸嘴3真空感应");
        public static InputClass I_vacuum4 = new InputClass(MotionCard, (int)InIndex.X20, "吸嘴4真空感应");

        public static InputClass I_Start = new InputClass(MotionCard, (int)InIndex.X21, "启动");
        public static InputClass I_Stop = new InputClass(MotionCard, (int)InIndex.X22, "停止");
        public static InputClass I_Reset = new InputClass(MotionCard, (int)InIndex.X23, "复位");
        public static InputClass I_Scram = new InputClass(MotionCard, (int)InIndex.X24, "急停");
        public static InputClass I_PAUSE = new InputClass(MotionCard, (int)InIndex.X25, "暂停");
        #endregion




    }
}
