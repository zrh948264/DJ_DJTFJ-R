using System;
using System.Collections.Generic;
using HalconDotNet;
using System.Drawing;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Windows.Forms;


namespace Vision.Tool.Calibrate
{
    /// <summary>
    /// 9点标定——点对点的仿射变换
    /// </summary>
    [Serializable]
    public class CalibPointToPoint : IXmlSerializable
    {
        /// <summary>
        /// 记录的仿射变换的点
        /// </summary>
        public List<CalibDataPoint> CalibrateData { get; set; } 

        HHomMat2D PixelToWorld = new HHomMat2D();
        HHomMat2D WorldToPixel = new HHomMat2D();

        /// <summary>
        /// 是否建立标定关系
        /// </summary>
        public bool IsBuiltted
        {
            get;private set;
        }


        public double Sx
        {
            get
            {
                HTuple sx, sy, phi, threta, tx, ty;
                HOperatorSet.HomMat2dToAffinePar(PixelToWorld, out sx, out sy, out phi, out threta, out tx, out ty);
                return sx.D;
            }
        }
        public double Sy
        {
            get
            {
                HTuple sx, sy, phi, threta, tx, ty;
                HOperatorSet.HomMat2dToAffinePar(PixelToWorld, out sx, out sy, out phi, out threta, out tx, out ty);
                return sy.D;
            }
        }
        public double Phi
        {
            get
            {
                HTuple sx, sy, phi, threta, tx, ty;
                HOperatorSet.HomMat2dToAffinePar(PixelToWorld, out sx, out sy, out phi, out threta, out tx, out ty);
                return phi.D;
            }
        }
        public double Theta
        {
            get
            {
                HTuple sx, sy, phi, threta, tx, ty;
                HOperatorSet.HomMat2dToAffinePar(PixelToWorld, out sx, out sy, out phi, out threta, out tx, out ty);
                return threta.D;
            }
        }
        public double Tx
        {
            get
            {
                HTuple sx, sy, phi, threta, tx, ty;
                HOperatorSet.HomMat2dToAffinePar(PixelToWorld, out sx, out sy, out phi, out threta, out tx, out ty);
                return tx.D;
            }
        }
        public double Ty
        {
            get
            {
                HTuple sx, sy, phi, threta, tx, ty;
                HOperatorSet.HomMat2dToAffinePar(PixelToWorld, out sx, out sy, out phi, out threta, out tx, out ty);
                return ty.D;
            }
        }

        /// <summary>
        /// 标定数据被更改事件
        /// </summary>
        [field: NonSerialized]
        public event EventHandler ChangeCalibrateDataEventHandler;
        protected virtual void OnChangeCalibrateData()
        {
            EventHandler temp = System.Threading.Interlocked.CompareExchange(ref this.ChangeCalibrateDataEventHandler, null, null);
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }


        


        public CalibPointToPoint()
        {
            CalibrateData = new List<CalibDataPoint>();
            IsBuiltted = false;
        }

        public CalibPointToPoint(int count):this()
        {
            for (int i = 0; i < count; i++)
            {
                CalibrateData.Add(new CalibDataPoint());
            }
           
        }


        public void AddCalibratePoint(double pixelRow,double pixelCol,double x,double y)
        {
            CalibDataPoint point = new CalibDataPoint();
            point.PixelRow = pixelRow;
            point.PixelCol = pixelCol;
            point.X = x;
            point.Y = y;
            CalibrateData.Add(point);
            IsBuiltted = false;

            OnChangeCalibrateData();
        }

        public void ChangePixelCalibrationPoint(int index, double pixelRow, double pixelCol)
        {
            if (index < 0 || index >= CalibrateData.Count)
                return;
            CalibDataPoint point = CalibrateData[index];
            point.PixelRow = pixelRow;
            point.PixelCol = pixelCol;

            CalibrateData[index] = point;
            IsBuiltted = false;

            OnChangeCalibrateData();
        }

        public void ChangeMachineCalibrationPoint(int index, double x, double y)
        {
            if (index < 0 || index >= CalibrateData.Count)
                return;
            CalibDataPoint point = CalibrateData[index];
            point.X = x;
            point.Y = y;
            CalibrateData[index] = point;
            IsBuiltted = false;

            OnChangeCalibrateData();
        }

        public void RemoveAtCalibrationPoint(int index)
        {
            if (index < 0 || index >= CalibrateData.Count)
                return;
            CalibrateData.RemoveAt(index);
            IsBuiltted = false;

            OnChangeCalibrateData();

        }

        public void ClearCalibrationData()
        {
            CalibrateData.Clear();
            PixelToWorld = new HHomMat2D();
            WorldToPixel = new HHomMat2D();
            IsBuiltted = false;

            OnChangeCalibrateData();
        }

        /// <summary>
        /// 更具对应点，构建标定关系
        /// </summary>
        public void BuildTransferMatrix()
        {
            HTuple hv_worldX = new HTuple(), hv_worldY = new HTuple();
            HTuple hv_imageR = new HTuple(), hv_imageC = new HTuple();

            int count = CalibrateData.Count;
            if (count < 3)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                hv_worldX[i] = CalibrateData[i].X;
                hv_worldY[i] = CalibrateData[i].Y;

                hv_imageR[i] = CalibrateData[i].PixelRow;
                hv_imageC[i] = CalibrateData[i].PixelCol;
            }

            try
            {
                PixelToWorld.VectorToHomMat2d(hv_imageR, hv_imageC, hv_worldX, hv_worldY);
                WorldToPixel.VectorToHomMat2d(hv_worldX, hv_worldY, hv_imageR, hv_imageC);
                IsBuiltted = true;

                OnChangeCalibrateData();

            }
            catch (Exception)
            {
                PixelToWorld = new HHomMat2D();
                WorldToPixel = new HHomMat2D();
                IsBuiltted = false;
                return;
            }
        }

        public void MatrixTransToWorld(double row, double col, out double x, out double y)
        {
            x = PixelToWorld.AffineTransPoint2d(row, col, out y);
        }

        public void MatrixTransToPixel(double x, double y, out double row, out double col)
        {
            row = WorldToPixel.AffineTransPoint2d(x, y, out col);
        }

        /// <summary>
        /// 获取标定数据的均值误差
        /// </summary>
        /// <returns></returns>
        public double CalibrateError()
        {
            if (CalibrateData.Count < 2) return 0;

            HTuple pixelRow, pixelCol, worldTransRow, worldTransCol;
            GetCalibrateDataPixelPoint(out pixelRow, out pixelCol);
            GetCalibrateDataTransWorldPoint(out worldTransRow, out worldTransCol);

            HTuple error = ((pixelRow - worldTransRow) * (pixelRow - worldTransRow) +
                (pixelCol - worldTransCol) * (pixelCol - worldTransCol)).TupleMean();

            return error.D;

        }


        /// <summary>
        /// 将图像中的点转为已参考点对照的机械坐标
        /// </summary>
        /// <param name="pixelPoint">在图片中的（模板匹配到的）像素点，以左上角为原点</param>
        /// <param name="WorldPoint">图像中的（模板匹配到的）点转为机械的实际坐标</param>
        /// <param name="pixelReferpoint">图像中的参考点，一般为图像中心</param>
        /// <param name="WorldReferPoint">图像参考点对应的机械坐标,坐标为0表示直接给偏差</param>
        public void PixelPointToWorldPoint(PointF pixelPoint, out PointF WorldPoint, PointF pixelReferpoint, PointF WorldReferPoint)
        {
            if (!IsBuiltted)
            {
                WorldPoint = WorldReferPoint;
                return;
            }

            double tx, ty;
            MatrixTransToWorld(pixelPoint.Y, pixelPoint.X, out tx, out ty);
            double cx, cy;
            MatrixTransToWorld(pixelReferpoint.Y, pixelReferpoint.X, out cx, out cy);

            WorldPoint = new PointF();
            WorldPoint.X = (float)(WorldReferPoint.X - tx + cx);
            WorldPoint.Y = (float)(WorldReferPoint.Y - ty + cy);
        }


        /// <summary>
        /// 将机械点转为已参考图像点对应的图像坐标
        /// </summary>
        /// <param name="machinePoint">在机械上的某一点坐标</param>
        /// <param name="imgPoint">转换得到的图像的像素点，已左上角为原点</param>
        /// <param name="imgReferpoint">图像中的参考点，一般为图像中心</param>
        /// <param name="machineReferPoint">图像参考点对应的机械坐标</param>
        public void WorldPointToPixelPoint(PointF WorldPoint, out PointF pixelPoint, PointF pixelReferpoint, PointF WorldReferPoint)
        {
            if (!IsBuiltted)
            {
                throw new Exception("未标定");
            }

            double tx, ty;
            MatrixTransToPixel(WorldPoint.X, WorldPoint.Y, out tx, out ty);
            double cx, cy;
            MatrixTransToPixel(WorldReferPoint.X, WorldReferPoint.Y, out cx, out cy);

            pixelPoint = new PointF();
            pixelPoint.X = (float)(pixelReferpoint.X - tx + cx);
            pixelPoint.Y = (float)(pixelReferpoint.Y - ty + cy);
        }


        public int GetCalibrateDataPixelPoint(out HTuple row, out HTuple col)
        {
            List<double> mr = new List<double>();
            List<double> mc = new List<double>();
            for (int i = 0; i < CalibrateData.Count; i++)
            {
                mr.Add(CalibrateData[i].PixelRow);
                mc.Add(CalibrateData[i].PixelCol);
            }

            row = mr.ToArray();
            col = mc.ToArray();

            return CalibrateData.Count;
        }

        public int GetCalibrateDataTransWorldPoint(out HTuple row, out HTuple col)
        {
            List<double> mr = new List<double>();
            List<double> mc = new List<double>();
            for (int i = 0; i < CalibrateData.Count; i++)
            {
                double tr, tc;
                MatrixTransToPixel(CalibrateData[i].X, CalibrateData[i].Y, out tr, out tc);
                mr.Add(tr);
                mc.Add(tc);
            }

            row = mr.ToArray();
            col = mc.ToArray();

            return CalibrateData.Count;
        }


        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                XmlSerializer typeSer = new XmlSerializer(CalibrateData.GetType());
                reader.ReadStartElement("CalibrateData");
                CalibrateData = typeSer.Deserialize(reader) as List<CalibDataPoint>;
                reader.ReadEndElement();

                typeSer = new XmlSerializer(typeof(double[]));
                reader.ReadStartElement("PixelToWorld");
                double[] temp = typeSer.Deserialize(reader) as double[];
                reader.ReadEndElement();
                PixelToWorld = new HHomMat2D(new HTuple(temp));

                typeSer = new XmlSerializer(typeof(double[]));
                reader.ReadStartElement("WorldToPixel");
                temp = typeSer.Deserialize(reader) as double[];
                reader.ReadEndElement();
                WorldToPixel = new HHomMat2D(new HTuple(temp));

                reader.MoveToContent();
            }
            reader.ReadEndElement();


            IsBuiltted = CalibrateData.Count > 2;
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            XmlSerializerNamespaces _namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(/*string.Empty*/) });

            XmlSerializer xml = new XmlSerializer(CalibrateData.GetType());
            writer.WriteStartElement("CalibrateData");
            xml.Serialize(writer, CalibrateData, _namespaces);
            writer.WriteEndElement();

            var temp = PixelToWorld.RawData.ToDArr();
            xml = new XmlSerializer(temp.GetType());
            writer.WriteStartElement("PixelToWorld");
            xml.Serialize(writer, temp, _namespaces);
            writer.WriteEndElement();

            temp = WorldToPixel.RawData.ToDArr();
            xml = new XmlSerializer(temp.GetType());
            writer.WriteStartElement("WorldToPixel");
            xml.Serialize(writer, temp, _namespaces);
            writer.WriteEndElement();

        }



      



    }



}
