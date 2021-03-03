using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonRs;

namespace HZZH.UI.DerivedControl
{
    public partial class Frm_DripArray : Form
    {

        public float[] AxisPosCurr = new float[3];
        PointF3 pLeftUp = new PointF3();
        PointF3 pRightUp = new PointF3();
        PointF3 pRightDwon = new PointF3();

        //List<DripLineDef> DripLine;

        int xAxisID = 0;
        int yAxisID = 0;
        int zAxisID = 0;

        /// <summary>
        /// 绑定的板卡
        /// </summary>
        private Device.MotionCardDef MotionCard;

        public Frm_DripArray()
        {
            InitializeComponent();
        }

        public Frm_DripArray(Device.MotionCardDef MotionCard,int xid, int yid, int zid)
        {
            InitializeComponent();

            this.MotionCard = MotionCard;

            xAxisID = xid;
            yAxisID = yid;
            zAxisID = zid;
        }

        private void btn_readleftup_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = (decimal)MotionCard.MotionFun.MC_GetCurrPos(xAxisID);
            numericUpDown2.Value = (decimal)MotionCard.MotionFun.MC_GetCurrPos(yAxisID);
            numericUpDown3.Value = (decimal)MotionCard.MotionFun.MC_GetCurrPos(zAxisID);
        }

        private void btn_readrightup_Click(object sender, EventArgs e)
        {
            numericUpDown4.Value = (decimal)MotionCard.MotionFun.MC_GetCurrPos(xAxisID);
            numericUpDown5.Value = (decimal)MotionCard.MotionFun.MC_GetCurrPos(yAxisID);
            numericUpDown6.Value = (decimal)MotionCard.MotionFun.MC_GetCurrPos(zAxisID);
        }

        private void btn_readrightdwon_Click(object sender, EventArgs e)
        {
            numericUpDown7.Value = (decimal)MotionCard.MotionFun.MC_GetCurrPos(xAxisID);
            numericUpDown8.Value = (decimal)MotionCard.MotionFun.MC_GetCurrPos(yAxisID);
            numericUpDown9.Value = (decimal)MotionCard.MotionFun.MC_GetCurrPos(zAxisID);
        }

        private void buttonArr_Click(object sender, EventArgs e)
        {
            if ((!radioButton1.Checked) && (!radioButton2.Checked))
            {
                MessageBox.Show("请选择方向并确认");
                return;
            }

            int Num = Convert.ToInt32(numericUpDown10.Value);

            int dripDirection = radioButton1.Checked ? 0 : 1;//点胶方式 

            pLeftUp.X = Convert.ToSingle(numericUpDown1.Value);
            pLeftUp.Y = Convert.ToSingle(numericUpDown2.Value);
            pLeftUp.Z = Convert.ToSingle(numericUpDown3.Value);


            pRightUp.X = Convert.ToSingle(numericUpDown4.Value);
            pRightUp.Y = Convert.ToSingle(numericUpDown5.Value);
            pRightUp.Z = Convert.ToSingle(numericUpDown6.Value);

            pRightDwon.X = Convert.ToSingle(numericUpDown7.Value);
            pRightDwon.Y = Convert.ToSingle(numericUpDown8.Value);
            pRightDwon.Z = Convert.ToSingle(numericUpDown9.Value);

            List<PointF3> List;

            if (dripDirection == 0)
            {
                List = PointArray.MatrixArrayList(pLeftUp, pRightUp, pRightDwon, Num, 2, dripDirection);
            }
            else
            {
                List = PointArray.MatrixArrayList(pLeftUp, pRightUp, pRightDwon, 2, Num, dripDirection);
            }

            if (List.Count < 2)
                return;

            //DripLine = new List<DripLineDef>();

            for (int i = 1;i< List.Count;i++)
            {
                //DripLineDef def = new DripLineDef();
                PointF4 f4 = new PointF4();

                f4.X = List[i - 1].X;
                f4.Y = List[i - 1].Y;
                f4.Z = List[i - 1].Z;

                //def.Point.Add(f4);

                PointF4 point = new PointF4();

                point.X = List[i].X;
                point.Y = List[i].Y;
                point.Z = List[i].Z;

                //def.Point.Add(point);

                //DripLine.Add(def);
            }


        }

    }
}
