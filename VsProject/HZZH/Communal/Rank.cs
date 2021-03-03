using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HzControl.Communal
{
    /// <summary>
    /// 带有上下限范围的输入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Rank<T> where T : IComparable<T>
    {
        private T _value = default(T);
        /// <summary>
        /// 范围最大值
        /// </summary>
        public T Max { get; set; }
        /// <summary>
        /// 范围最小值
        /// </summary>
        public T Min { get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.CompareTo(Min) < 0)
                {
                    _value = Min;
                }
                else if (value.CompareTo(Max) > 0)
                {
                    _value = Max;
                }
                else
                {
                    _value = value;
                }
            }
        }

        /// <summary>
        /// 默认构造
        /// </summary>
        public Rank()
        {
            Min = default(T);
            Max = default(T);
        }

        /// <summary>
        /// 上下限制构造
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Rank(T value, T min, T max)
        {
            this.Value = value;
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// 判断值是否在范围中
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool InRank(T value)
        {
            return value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
        }

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="rank"></param>
        public static implicit operator T(Rank<T> rank)
        {
            return rank.Value;
        }
    }

    /// <summary>
    /// 带范围的整形数据
    /// </summary>
    [Serializable]
    public class RankInt32 : Rank<int>
    {
        public RankInt32() : base(0, -32768, 32767)
        {
        }

        public RankInt32(int value, int min, int max) : base(value, min, max)
        {
        }
    }

    /// <summary>
    /// 带范围的浮点型数据
    /// </summary>
    [Serializable]
    public class RankFloat : Rank<float>
    {
        public RankFloat() : base(0, -999999f, 99999f)
        {
        }

        public RankFloat(float value, float min, float max) : base(value, min, max)
        {
        }
    }
}
