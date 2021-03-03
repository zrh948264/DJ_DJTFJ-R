using CommonRs;
using HzControl.Logic;
using HZZH.Logic.Commmon;
using HZZH.Logic.LogicMain;
using MyControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HZZH.Logic.SubLogicPrg
{
	public class PickDetectDef : LogicTask
	{
		public float Pos_DetectMax { get; set; }//测高极限

		public PickDetectDef() : base("拾取测高")
		{ 
		
		}
		protected override void LogicImpl()
		{
			switch (LG.Step)
			{
				case 1:
					FPointXYZR p = TaskMain.PickTake.Pos_Take.Clone();//取晶位置，clone出来
					p.Z = TaskMain.PickJump.Ready.Z;//Z位置改成预备高度
					TaskMain.PickJump.Start(p);//跳跃到取晶位上方
					LG.StepNext(2);
					break;

				case 2:
					if (TaskMain.PickJump.GetSta() == 0)//等待定位结束
					{
						DeviceRsDef.Ax_PickZ.MC_MoveAbs(0, Pos_DetectMax);//以最慢速度向测高极限移动
						LG.StepNext(3);
					}
					break;

				case 3:
					if (DeviceRsDef.Ax_PickZ.busy)//在轴运行过程中
					{
						if(DeviceRsDef.I_PickDetect.value)//检测到测高信号
						{
							TaskMain.PickTake.Pos_Take.Z = DeviceRsDef.Ax_PickZ.currPos;//更新取晶高度
							DeviceRsDef.Ax_PickZ.MC_Stop();//停止轴
							LG.StepNext(4);//
						}
					}
					else//测高轴运行结束
					{
						MessageBox.Show("测高失败，到达测高极限", "提示");
						LG.End();
					}
					break;

				case 4:
					if (!DeviceRsDef.Ax_PickZ.busy)
					{
						MessageBox.Show("测高完成，取晶高度已更新", "提示");
						LG.End();
					}
					break;
			}
		}
	}
}
