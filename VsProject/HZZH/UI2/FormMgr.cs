using HZZH.UI.DerivedControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace HZZH.UI2
{
    class FrmMgr
    {
        private static readonly Dictionary<string, List<BaseSubForm>> forms = new Dictionary<string, List<BaseSubForm>>();

        private static readonly Dictionary<string, Type> typeMap = new Dictionary<string, Type>();

        static FrmMgr()
        {
            foreach (var item in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (item.IsSubclassOf(typeof(BaseSubForm)))
                {
                    typeMap.Add(item.Name, item);
                    forms.Add(item.Name, new List<BaseSubForm>());
                }
            }
        }

        private static object CreateInstance(string typeName)
        {
            Debug.Assert(typeMap.ContainsKey(typeName), "未查找到该名字类型");
            return Activator.CreateInstance(typeMap[typeName], true);
        }

        public static BaseSubForm GetFormInst(string typeName, int index = 0)
        {
            Debug.Assert(forms.ContainsKey(typeName), "未查找到该名字类型");

            while (index >= forms[typeName].Count)
            {
                forms[typeName].Add((BaseSubForm)CreateInstance(typeName));
            }

            return forms[typeName][index];
        }

        public static T GetFormInst<T>(int index) where T : BaseSubForm
        {
            Type type = typeof(T);
            return GetFormInst(type.Name, index) as T;
        }

        public static void Init()
        {
            foreach (var item in typeMap.Keys)
            {
                GetFormInst(item);
            }
        }



        private static Control container = null;
        private static BaseSubForm showFrom;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        const int WM_SETREDRAW = 0x000B; //设置窗口是否能重画 

        public static void Show(string formName)
        {
            if (string.IsNullOrWhiteSpace(formName))
            {
                return;
            }

            char[] split = new char[] { ',' };
            string[] array = formName.Split(split);
            int index = 0;
            if (array.Length == 2)
            {
                index = Convert.ToInt32(array[1]);
            }
            BaseSubForm baseSubForm = GetFormInst(array[0], index);

            if (container != null && object.ReferenceEquals(showFrom, baseSubForm) == false)
            {
                SendMessage(container.Handle, WM_SETREDRAW, 0, 0);

                if (showFrom != null)
                {
                    showFrom.Hide();

                    while (formStack.Contains(formName))
                    {
                        formStack.Pop();
                    }
                }
                container.Controls.Clear();
                showFrom = baseSubForm;
                container.Controls.Add(showFrom);
                showFrom.Show();
                formStack.Push(formName);
                SendMessage(container.Handle, WM_SETREDRAW, -1, 0);
                container.Refresh();
            }
        }

        public static void RegisterContainer(Control ctrl)
        {
            if (container != null)
            {
                container.Controls.Clear();
            }

            container = ctrl;
        }


        private static readonly Stack<string> formStack = new Stack<string>();
        public static void Show()
        {
            if (formStack.Count > 1)
            {
                formStack.Pop();
                Show(formStack.Peek());
            }
        }
    }
}
