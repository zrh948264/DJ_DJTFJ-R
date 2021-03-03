using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CommonRs;
using System.Security.Cryptography;

namespace MyControl
{
    public partial class UserInfo : Form
    {
       
        List<UserMode> UserNameList = new List<UserMode>();
        public UserInfo()
        {
            InitializeComponent();

                UserNameList.Add(new UserMode() { Index = 0, Name = "操作员" });
                UserNameList.Add(new UserMode() { Index = 1, Name = "工程师" });
                UserNameList.Add(new UserMode() { Index = 2, Name = "厂家" });
                //UserNameList.Add(new UserMode() { Index = 3, Name = "开发者" });
            cmb_UserType.DataSource = null;
            cmb_UserType.DataSource = UserNameList;
            cmb_UserType.ValueMember = "Index";
            cmb_UserType.DisplayMember = "Name";
        }

        public void GetUserList(User user)
        {
            this.user = user;
            //UserMgrLogos();
            //LoadUsers(ConfigHandle.Instance.UserDefine.UserList);
            if (cmb_UserType.Items.Count!=0)
            {
                cmb_UserType.SelectedIndex = 0;
            }
        }


        public User user;             //用户  

        private void btn_AddUserinfo_Click(object sender, EventArgs e)
        {
            if (txt_UserName.Text.Trim() == "")
            {
                MessageBox.Show("请输入用户名");
                txt_UserName.Focus();
                return;
            }
            if (txt_UserPwd.Text.Trim() == "")
            {
                MessageBox.Show("请输入密码");
                txt_UserPwd.Focus();
                return;
            }

            if (cmb_UserType.SelectedIndex == -1)
            {
                MessageBox.Show("请选择用户类型");
                cmb_UserType.Focus();
                return;
            }         
            //if (ConfigHandle.Instance.UserDefine.UserList.Exists(u => u.Name.Equals(txt_UserName.Text)))
            //{
            //    MessageBox.Show("用户名 '" + txt_UserName.Text + "' 已经存在，输入新的用户名 ");
            //    txt_UserName.Focus();
            //    return;
            //}
            //ConfigHandle.Instance.UserDefine.UserList.Add(new User(txt_UserName.Text, GetSHA1HashData(txt_UserPwd.Text), cmb_UserType.SelectedIndex.ToString()));
            //LoadUsers(ConfigHandle.Instance.UserDefine.UserList);
        }

        private void btn_ClearUserinfo_Click(object sender, EventArgs e)
        {
            txt_UserName.Text = txt_UserPwd.Text = "";
            cmb_UserType.SelectedIndex = 0;
        }
        //表格显示
        private void LoadUsers(List<User> m_listUsers)
        {
            this.DG_Users.Rows.Clear();
            for (int i = 0; i < m_listUsers.Count; i++)
            {
                DG_Users.Rows.Add();
                DG_Users[0, i].Value = m_listUsers[i].Name;
                DG_Users[2, i].Value = m_listUsers[i].CreatedOn;
                DG_Users[3, i].Value = "删除";
                DG_Users[1, i].Value = m_listUsers[i].Type;
                switch (m_listUsers[i].Type)
                {
                    case "0":
                        DG_Users[1, i].Value = "操作员";
                        break;
                    case "1":
                        DG_Users[1, i].Value = "工程师";
                        break;
                    case "2":
                        DG_Users[1, i].Value = "厂家";
                        break;
                    case "3":
                        DG_Users.Rows[i].Visible = false;
                        break;
                    default:
                        break;
                }
            }
        }
        //删除用户
        private void DG_Users_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 3) && (e.RowIndex != -1))
            {
                DialogResult result = MessageBox.Show("是否删除此用户: " + DG_Users[0, e.RowIndex].Value.ToString() + "?", "温馨提示", MessageBoxButtons.OKCancel);
                if (result==DialogResult.OK)
                {
                    //ConfigHandle.Instance.UserDefine.UserList.RemoveAt(e.RowIndex);
                    //LoadUsers(ConfigHandle.Instance.UserDefine.UserList);
                }

            }
        }
        public string GetSHA1HashData(string data)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }
       
        private void UserMgrLogos()
        {
            try
            {
                if (user != null)
                {
                    switch (user.Type)
                    {
                        case "0":
                            UserNameList.Add(new UserMode() { Index = 0, Name = "操作员" });
                            break;
                        case "1":
                            UserNameList.Add(new UserMode() { Index = 0, Name = "操作员" });
                            UserNameList.Add(new UserMode() { Index = 1, Name = "工程师" });
                            break;
                        case "2":
                            UserNameList.Add(new UserMode() { Index = 0, Name = "操作员" });
                            UserNameList.Add(new UserMode() { Index = 1, Name = "工程师" });
                            UserNameList.Add(new UserMode() { Index = 2, Name = "厂家" });
                            break;
                        case "3":
                            UserNameList.Add(new UserMode() { Index = 0, Name = "操作员" });
                            UserNameList.Add(new UserMode() { Index = 1, Name = "工程师" });
                            UserNameList.Add(new UserMode() { Index = 2, Name = "厂家" });
                            UserNameList.Add(new UserMode() { Index = 3, Name = "开发者" });
                            break;
                        default:
                            UserNameList.Add(new UserMode() { Index = 0, Name = "操作员" });
                            UserNameList.Add(new UserMode() { Index = 1, Name = "工程师" });
                            UserNameList.Add(new UserMode() { Index = 2, Name = "厂家" });
                            UserNameList.Add(new UserMode() { Index = 3, Name = "开发者" });
                            break;
                    }
                }
                cmb_UserType.DataSource = null;
                cmb_UserType.DataSource = UserNameList;
                cmb_UserType.ValueMember = "Index";
                cmb_UserType.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
    }
}
