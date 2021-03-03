using System;
using System.Text;
using System.Windows.Forms;
using CommonRs;
using System.Security.Cryptography;

namespace MyControl
{
    public partial class ChangeUserPwd : Form
    {
        User currentUser;
        public ChangeUserPwd()
        {
            InitializeComponent();
        }

        public void SetUser(User currentUser)
        {
            this.currentUser = currentUser;
        }

        private void btn_ChangePwd_Click(object sender, EventArgs e)
        {
            if (txt_OldPassword.Text.Trim() == "")
            {
                MessageBox.Show("请输入旧密码", ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txt_OldPassword.Focus();
                return;
            }
            if (txt_PasswordNew.Text.Trim() == "")
            {
                MessageBox.Show("请输入新密码", ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txt_PasswordNew.Focus();
                return;
            }
            if (!currentUser.PassWord.Equals(GetSHA1HashData(txt_OldPassword.Text)))
            {
                MessageBox.Show("旧密码错误", ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //for (int i = 0; i < ConfigHandle.Instance.UserDefine.UserList.Count; i++)
            //{
            //    if (ConfigHandle.Instance.UserDefine.UserList[i].Name.Equals(currentUser.Name) && ConfigHandle.Instance.UserDefine.UserList[i].PassWord.Equals(GetSHA1HashData(txt_OldPassword.Text)))
            //    {
            //        ConfigHandle.Instance.UserDefine.UserList[i].PassWord = GetSHA1HashData(txt_PasswordNew.Text);
            //        currentUser = ConfigHandle.Instance.UserDefine.UserList[i];
            //        break;
            //    }
            //}
            MessageBox.Show("成功修改密码 ", ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private string GetSHA1HashData(string data)
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

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public User GetCurrentUser()
        {
            return currentUser;
        }

        private void ck_ViewPassword_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox che = (CheckBox)sender;
            if (che.Checked)
            {
                txt_OldPassword.PasswordChar = new char();
                txt_PasswordNew.PasswordChar = new char();
            }
            else
            {
                txt_OldPassword.PasswordChar = '*';
                txt_PasswordNew.PasswordChar = '*';
            }
        }
    }
}
