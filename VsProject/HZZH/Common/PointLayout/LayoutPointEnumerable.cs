using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.PointLayout
{
    class LayoutPointEnumerable : IEnumerable<LayoutPoint>
    {
        ILayoutPoint layoutPoints = null;

        public LayoutPointEnumerable(ILayoutPoint lay)
        {
            if (lay == null) throw new ArgumentNullException();

            layoutPoints = lay;
        }

        public IEnumerator<LayoutPoint> GetEnumerator()
        {
            return layoutPoints.GetLayoutPoints().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return layoutPoints.GetLayoutPoints().GetEnumerator();
        }

    }



    public interface ILayoutPoint
    {
        List<LayoutPoint> GetLayoutPoints();

    }

}
