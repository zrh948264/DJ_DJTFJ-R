using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Vision.Tool.Calibrate
{
    using HalconDotNet;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;


    [Serializable]
    public class CalibSimilarity: IXmlSerializable
    {
        public PointF[] ImgPoints = null;
        public PointF[] MachinePoints = null;
        [XmlIgnore]
        public HHomMat2D Mat2D { get; set; }

        public CalibSimilarity()
        {
            ImgPoints = new PointF[2];
            MachinePoints = new PointF[2];
            Mat2D = new HHomMat2D();
        }

        public void BuildTransferMatrix()
        {
            HTuple hv_machineY = new HTuple(), hv_machineX = new HTuple();
            HTuple hv_imageY = new HTuple(), hv_imageX = new HTuple();

            int count = Math.Min(ImgPoints.Length, MachinePoints.Length);
            if (count < 2)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                hv_machineX[i] = MachinePoints[i].X;
                hv_machineY[i] = MachinePoints[i].Y;

                hv_imageX[i] = ImgPoints[i].X;
                hv_imageY[i] = ImgPoints[i].Y;
            }

            try
            {
                Mat2D.VectorToSimilarity(hv_imageX, hv_imageY, hv_machineX, hv_machineY);
                var v = Scaling;
                IsBuiltted = true;
            }
            catch (Exception)
            {
                Mat2D = new HHomMat2D();
                IsBuiltted = false;
            }
        }

        public double Scaling
        {
            get
            {
                double sx, sy, phi, theta, tx, ty;
                sx = Mat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);

                return sx;
            }
        }

        public double RotationAngle
        {
            get
            {
                double sx, sy, phi, theta, tx, ty;
                sx = Mat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);

                return phi * 180 / Math.PI;
            }
        }


        private bool IsBuiltted = false;


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
                XmlSerializer typeSer = new XmlSerializer(typeof(PointF[]));
                reader.ReadStartElement("ImgPoints");
                ImgPoints = typeSer.Deserialize(reader) as PointF[];
                reader.ReadEndElement();

                typeSer = new XmlSerializer(typeof(PointF[]));
                reader.ReadStartElement("MachinePoints");
                MachinePoints = typeSer.Deserialize(reader) as PointF[];
                reader.ReadEndElement();

                var vvv = new double[6];
                typeSer = new XmlSerializer(vvv.GetType());
                reader.ReadStartElement("Mat2D");
                vvv = typeSer.Deserialize(reader) as double[];
                Mat2D = new HHomMat2D(vvv);
                reader.ReadEndElement();


                typeSer = new XmlSerializer(typeof(bool));
                reader.ReadStartElement("IsBuiltted");
                IsBuiltted = (bool)typeSer.Deserialize(reader);
                reader.ReadEndElement();
  
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            XmlSerializer xml = new XmlSerializer(ImgPoints.GetType());
            writer.WriteStartElement("ImgPoints");
            xml.Serialize(writer, ImgPoints);
            writer.WriteEndElement();

            xml = new XmlSerializer(MachinePoints.GetType());
            writer.WriteStartElement("MachinePoints");
            xml.Serialize(writer, MachinePoints);
            writer.WriteEndElement();

            var vvv = Mat2D.RawData.ToDArr();

            xml = new XmlSerializer(vvv.GetType());
            writer.WriteStartElement("Mat2D");
            xml.Serialize(writer, vvv);
            writer.WriteEndElement();

            xml = new XmlSerializer(typeof(bool));
            writer.WriteStartElement("IsBuiltted");
            xml.Serialize(writer, IsBuiltted);
            writer.WriteEndElement();
        }






        public void TransToMachinePoint(float x, float y, out float tX, out float tY)
        {
            double transferX, transferY;
            transferX = Mat2D.AffineTransPoint2d(x, y, out transferY);
            tX = (float)transferX;
            tY = (float)transferY;
        }



        
        /// <summary>
        /// 将图像中的点转为已参考点对照的机械坐标
        /// </summary>
        /// <param name="imgPoint">在图片中的像素点，以左上角为原点</param>
        /// <param name="MachinePoint">图像中的点转为机械的实际坐标</param>
        /// <param name="imgReferpoint">图像中的参考点，一般为图像中心</param>
        /// <param name="machineReferPoint">图像参考点对应的机械坐标</param>
        public void PixelPointToWorldPoint(PointF pixelPoint, out PointF WorldPoint, PointF pixelReferpoint, PointF WorldReferPoint)
        {
            if (IsBuiltted == false)
            {
                WorldPoint = WorldReferPoint;
                return;
            }

            float tx, ty;
            TransToMachinePoint(pixelPoint.X, pixelPoint.Y, out tx, out ty);
            float cx, cy;
            TransToMachinePoint(pixelReferpoint.X, pixelReferpoint.Y, out cx, out cy);

            WorldPoint = new PointF();
            WorldPoint.X = WorldReferPoint.X - tx + cx;
            WorldPoint.Y = WorldReferPoint.Y - ty + cy;
        }


   

    }
}
