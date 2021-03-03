using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CommonRs;

namespace HZZH.Database
{
    /// <summary>
    /// 工程参数
    /// </summary>
    public class ProjectData : ProductBase
    {
        /// <summary>
        /// 运行速度
        /// </summary>
        public float Spd { set; get; }

        public int UPH { set; get; }

        public int Yield { set; get; }

        /// <summary>
        /// 飞拍速度
        /// </summary>
        public float Fly_Spd { set; get; }


        #region 间距
        /// <summary>
        /// 吸嘴间距
        /// </summary>
        public float Nozzle_space { get; set; }        
        /// <summary>
        /// 胶阀间距
        /// </summary>
        public float Glue_space { get; set; }

        #endregion


        #region 高度&位置    根据吸嘴间距 + 下相机标定位置
        /// <summary>
        /// 标定贴料相机中心
        /// </summary>
        public PointF4 Pos_Designation { set; get; }
        /// <summary>
        /// 胶阀标定贴料相机中心
        /// </summary>
        public PointF4 Glue_Designation { set; get; }
        /// <summary>
        /// 标定取料相机中心
        /// </summary>
        public PointF4 Pos_Designation_L { set; get; }


        /// <summary>
        /// 标定下相机中心
        /// </summary>
        public PointF4 Pos_Designation_R { set; get; }
        /// <summary>
        /// 飞拍开始位置 
        /// </summary>
        public PointF4 Pos_CCDStar { set; get; }
        /// <summary>
        /// 飞拍结束位置
        /// </summary>
        public PointF4 Pos_CCDEnd { set; get; }
        /// <summary>
        /// 飞拍高度
        /// </summary>
        public float Fly_Hight { set; get; }


        /// <summary>
        /// 避让位_Y方向（贴料区）
        /// </summary>
        public float Avoid_pos { get; set; }
        /// <summary>
        /// 避让位_Y方向（取料区）
        /// </summary>
        public float Avoid_pos1 { get; set; }
        /// <summary>
        /// 皮带前进长度
        /// </summary>
        public float Belt_Length { get; set; }

        /// <summary>
        /// Z轴安全高度
        /// </summary>
        public float Safe_Hight { set; get; }
        /// <summary>
        /// 吸嘴轴安全高度
        /// </summary>
        public float nSafe_Hight { set; get; }
        /// <summary>
        /// Z轴工作高度
        /// </summary>
        public float Work_Hight { set; get; }
        /// <summary>
        /// 吸嘴轴工作高度(贴)
        /// </summary>
        public float nWork_Hight { set; get; }

        /// <summary>
        /// 吸嘴轴工作高度
        /// </summary>
        public float nTake_Hight { set; get; }
        
        /// <summary>
        /// Z轴点胶高度1
        /// </summary>
        public float Glue_Hight { set; get; }


        /// <summary>
        /// 四个吸嘴的旋转中心
        /// </summary>
        public PointF2[] RatationCenter { get; set; }

        #endregion


        #region 延时
        /// <summary>
        /// 开真空延时
        /// </summary>
        public int Vacuo_Delay { get; set; }
        /// <summary>
        /// 破真空延时
        /// </summary>
        public int BVacuo_Delay { get; set; }
        /// <summary>
        /// 开胶延时
        /// </summary>
        public int Glue_Delay { get; set; }
        /// <summary>
        /// 开胶延时
        /// </summary>
        public int Glue1_Delay { get; set; }
        /// <summary>
        /// 开胶延时
        /// </summary>
        public int Glue_Delay1 { get; set; }
        /// <summary>
        /// 开胶延时
        /// </summary>
        public int Glue1_Delay1 { get; set; }
        /// <summary>
        /// 破真空延时
        /// </summary>
        public int Down_Delay { get; set; }
        /// <summary>
        /// 皮带移动后延时
        /// </summary>
        public int Belt_Delay { get; set; }
        #endregion

        /// <summary>
        /// 点胶水模式 
        /// 0 ：俩种胶水同时点
        /// 1：点胶阀1
        /// 2：两次点胶点不同胶水
        /// 3：每次抬起气缸点胶
        /// </summary>
        public uint Gluemode { get; set; }

        /// <summary>
        /// 左取料爆光时间
        /// </summary>
        public int CameraExposureTime0 { get; set; }
        /// <summary>
        /// 右取料爆光时间
        /// </summary>
        public int CameraExposureTime1 { get; set; }
        /// <summary>
        /// 贴料爆光时间
        /// </summary>
        public int CameraExposureTime2 { get; set; }
        


        public ProjectData()
        {
            Pos_Designation = new PointF4();
            Pos_Designation_L = new PointF4();
            Pos_Designation_R = new PointF4();

            Glue_Designation = new PointF4();

            RatationCenter = new PointF2[4];
            for(int i = 0;i<4;i++)
            {
                RatationCenter[i] = new PointF2();
            }

        }

        /// <summary>
        /// 根据旋转中心进行变换
        /// </summary>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="angle"></param>
        /// <param name="qx"></param>
        /// <param name="qy"></param>
        public void RotaCenterToOffset(float centerX, float centerY, //旋转中心
                                        float x, float y, float angle, //飞拍结果
                                        out float qx, out float qy) //输出
        {
            float xTerm = x - centerX;
            float yTerm = y - centerY;
            float a = angle * (float)Math.PI / 180f;
            qx = xTerm * (float)Math.Cos(a) - yTerm * (float)Math.Sin(a) + centerX;
            qy = xTerm * (float)Math.Sin(a) + yTerm * (float)Math.Cos(a) + centerY;
        }



        [OnDeserialized()]
        private void OnDeserializedMethod(StreamingContext context)
        {
            foreach (var item in this.GetType().GetProperties())
            {
                if (item.GetValue(this) == null && item.CanWrite)
                {
                    object obj = item.PropertyType.Assembly.CreateInstance(item.PropertyType.FullName);
                    item.SetValue(this, obj);
                }
            }
        }

    }

    /// <summary>
    /// 吸嘴参数，不保存
    /// </summary>
    public class NozzleData
    {
        /// <summary>
        /// 禁用
        /// </summary>
        public bool En { get; set; }
        /// <summary>
        /// 吸嘴上是否有料
        /// </summary>
        public bool IsHave { get; set; }
        /// <summary>
        /// 物料料类型
        /// </summary>
        public int Type { get; set; }


    }


}
