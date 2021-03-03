using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HzControl.Communal.Controls
{
    [DefaultEvent("DataSourceUpdate")]
    [ProvideProperty("BindingName", typeof(Control))]
    public class UserBingData : Component, IExtenderProvider
    {
        private static readonly Timer readValueTimer = new Timer();

        static UserBingData()
        {
            readValueTimer.Interval = 200;
            readValueTimer.Start();
        }

        public UserBingData()
        {
            readValueTimer.Tick += ReadValueTimer_Tick;
            this.Disposed += UserBingData_Disposed;
        }

        public UserBingData(IContainer container) : this()
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            container.Add(this);
        }

        public bool CanExtend(object extendee)
        {
            return /*extendee is NumericUpDown ||*/ extendee is MyTextBox;
        }

        private Hashtable hashtable = new Hashtable();

        public string GetBindingName(Control numeric)
        {
            if (hashtable.Contains(numeric))
                return (string)hashtable[numeric];
            else
                return string.Empty;
        }

        public void SetBindingName(Control numeric, string name)
        {
            if (numeric == null) return;

            if (string.IsNullOrEmpty(name))
            {
                hashtable.Remove(numeric);
            }
            else
            {
                if (!hashtable.ContainsKey(numeric))
                {
                    numeric.Disposed += Numeric_Disposed;
                }
                hashtable[numeric] = name;
            }
        }

        private void Numeric_Disposed(object sender, EventArgs e)
        {
            hashtable.Remove(sender);
        }

        public static object GetBindingDataSource(object source, string propertyName)
        {
            Debug.Assert(source != null);
            Debug.Assert(propertyName != null);

            string[] name = SplitBindingName(propertyName);
            object propertyValue = source.GetType().GetProperty(name[0]).GetValue(source, null);
            if (name.Length > 1)
            {
                string propertyName2 = CombineBindingName(name.Skip(1).ToArray());
                return GetBindingDataSource(propertyValue, propertyName2);
            }
            else
            {
                return propertyValue;
            }
        }

        public static string[] SplitBindingName(string propertyName)
        {
            char[] split = new char[] { '.' };
            string[] str = propertyName.Split(split);
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = str[i].Trim();
            }
            return str;
        }

        public static string CombineBindingName(string[] propertyName)
        {
            bool flag = false;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in propertyName)
            {
                if (flag == true)
                {
                    stringBuilder.Append('.');
                }
                stringBuilder.Append(item);
                flag = true;
            }
            return stringBuilder.ToString();
        }

        public static T[] GetControls<T>(Control control) where T : Control
        {
            List<T> list = new List<T>();
            Queue<Control> controls = new Queue<Control>();
            controls.Enqueue(control);
            while (controls.Count > 0)
            {
                Control ctrl = controls.Dequeue();
                for (int i = 0; i < ctrl.Controls.Count; i++)
                {
                    controls.Enqueue(ctrl.Controls[i]);
                }

                if (ctrl is T)
                {
                    list.Add((T)ctrl);
                }
            }
            return list.ToArray();
        }

        private void SetBindingObj(Control ctrl, string propertyName, object obj, string name)
        {
            Binding binding = SetBinding(ctrl, propertyName, obj, name);
            binding.BindingComplete += UserBingData_BindingComplete;
            if (ctrl is MyTextBox)
            {
                MyTextBox myText = ctrl as MyTextBox;
                try
                {
                    if (obj.GetType().GetProperty(name).PropertyType.IsClass && myText.InputType != MyTextBox.eInputType.String)
                    {
                        throw new FormatException("绑定格式错误");
                    }
                    else if (myText.InputType == MyTextBox.eInputType.Int)
                    {
                        binding.FormattingEnabled = true;
                        binding.FormatString = "F0";
                    }
                    else if (myText.InputType == MyTextBox.eInputType.Float)
                    {
                        binding.FormattingEnabled = true;
                        binding.FormatString = myText.Format;
                    }
                }
                catch
                {
                    binding.Control.DataBindings.Remove(binding);
                    throw;
                }
            }


        }

        public static Binding SetBinding(Control ctrl, string propertyName, object obj, string name)
        {
            if (ctrl.DataBindings[propertyName] != null)
            {
                ctrl.DataBindings.Remove(ctrl.DataBindings[propertyName]);
            }
            ctrl.DataBindings.Add(propertyName, obj, name, true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged);
            return ctrl.DataBindings[propertyName];
        }


        private void UserBingData_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Success && e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
            {
                EventHandler temp = DataSourceUpdate;
                if (temp != null)
                {
                    temp.Invoke(e.Binding.Control, EventArgs.Empty);
                }
            }

            if (e.BindingCompleteState == BindingCompleteState.Success && e.BindingCompleteContext == BindingCompleteContext.ControlUpdate)
            {
                EventHandler temp = ControlUpdate;
                if (temp != null)
                {
                    temp.Invoke(e.Binding.Control, EventArgs.Empty);
                }
            }
        }

        [Description("指示正在从控件属性更新数据源值")]
        public event EventHandler DataSourceUpdate;
        [Description("指示正在从数据源更新控件属性值")]
        public event EventHandler ControlUpdate;


        /// <summary>
        /// 按照设置的属性路径绑定对象中的各属性
        /// </summary>
        /// <param name="dataSource"></param>
        public void SetBindingDataSource(object dataSource)
        {
            foreach (Control item in hashtable.Keys)
            {
                string bindingString = GetBindingName(item);
                if (string.IsNullOrWhiteSpace(bindingString))
                {
                    continue;
                }

                try
                {
                    string[] strs = SplitBindingName(bindingString);
                    if (strs.Length == 1)
                    {
                        SetBindingObj(item, "Text", dataSource, strs[0]);
                    }
                    else
                    {
                        string sourceName = CombineBindingName(strs.Take(strs.Length - 1).ToArray());
                        object source = GetBindingDataSource(dataSource, sourceName);
                        SetBindingObj(item, "Text", source, strs.Last());
                    }
                }
                catch
                {
                    item.Text = "****";
                }
            }
        }

        /// <summary>
        /// 从数据源中读取值
        /// </summary>
        public void ReadValue()
        {
            foreach (Control item in hashtable.Keys)
            {
                if (item.DataBindings.Count > 0)
                {
                    item.DataBindings[0].ReadValue();
                }
            }
        }


        private void UserBingData_Disposed(object sender, EventArgs e)
        {
            readValueTimer.Tick -= ReadValueTimer_Tick;
        }

        private void ReadValueTimer_Tick(object sender, EventArgs e)
        {
            foreach (Control item in hashtable.Keys)
            {
                if (item.FindForm().Visible == true && item.DataBindings.Count > 0)
                {
                    if (item.Focused == false)
                    {
                        Binding binding = item.DataBindings[0];
                        string obj1 = binding.Control.GetType().GetProperty(binding.PropertyName).GetValue(binding.Control, null).ToString();
                        string obj2 = null;
                        if (binding.FormattingEnabled == true)
                        {
                            obj2 = string.Format("{0:" + binding.FormatString + "}", binding.DataSource.GetType().GetProperty(binding.BindingMemberInfo.BindingField).GetValue(binding.DataSource, null));
                        }
                        else
                        {
                            obj2 = binding.DataSource.GetType().GetProperty(binding.BindingMemberInfo.BindingField).GetValue(binding.DataSource, null).ToString();
                        }

                        if (string.Equals(obj1, obj2) == false)
                        {
                            item.DataBindings[0].ReadValue();
                        }
                    }
                }
            }
        }

    }
}
