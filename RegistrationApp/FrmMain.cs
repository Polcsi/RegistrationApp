using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RegistrationApp
{
    public partial class FrmMain : Form
    {
        public List<User> Users { get; set; }
        private int UsernameMax { get; set; }
        private int FullNameMax { get; set; }
        private int ResidenceMax { get; set; }
        private int StartSelection { get; set; }
        private int LengthSelection { get; set; }
        public FrmMain()
        {
            InitializeComponent();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            UsernameMax = txtUsername.MaxLength;
            FullNameMax = txtFullName.MaxLength;
            ResidenceMax = txtResidence.MaxLength;

            lblUsernameCounter.Text = $"{UsernameMax}";
            lblFullnameCounter.Text = $"{FullNameMax}";
            lblLakhelyCounter.Text = $"{ResidenceMax}";

            cmbGender.Items.Insert(0, "Férfi");
            cmbGender.Items.Insert(1, "Nő");
            cmbGender.SelectedIndex = 0;

            Users = new List<User>();
        }
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            calculateRemainingCharacters(UsernameMax, (TextBox)sender, lblUsernameCounter);
        }
        private void txtFullName_TextChanged(object sender, EventArgs e)
        {
            calculateRemainingCharacters(FullNameMax, (TextBox)sender, lblFullnameCounter);
        }
        private void txtResidence_TextChanged(object sender, EventArgs e)
        {
            calculateRemainingCharacters(ResidenceMax, (TextBox)sender, lblLakhelyCounter);
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                checkInputFields(grpRegistration);
                Users.Add(new User(txtUsername.Text, txtPassword.Text, txtFullName.Text, Convert.ToDateTime(mskdTxtBirthDate.Text), txtResidence.Text, cmbGender.Text));
                clearInputFields(grpRegistration);
                listRegisteredUsers();
            }
            catch (EmptyValueException ex)
            {
                MessageBox.Show($"{ex.Message} Töltse ki a '{ex.Field}' mezőt.", "Üres Mező Érték Hiba", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            sfd.FileName = "people.txt";
            sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.DefaultExt = ".txt";
            sfd.InitialDirectory = Directory.GetCurrentDirectory();

            if(sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        foreach (User user in Users)
                        {
                            sw.WriteLine($"{user.UserName};{user.Password};{user.FullName};{user.BirthDate.ToString("yyyy.MM.dd")};{user.Residence};{user.Gender}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Occured During Writing File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            ofd.FileName = "default.txt";
            ofd.DefaultExt = ".txt";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.InitialDirectory = Directory.GetCurrentDirectory();

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] lines = File.ReadAllLines(ofd.FileName);
                    Users.Clear();
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');
                        Users.Add(new User(parts[0], parts[1], parts[2], Convert.ToDateTime(parts[3]), parts[4], parts[5]));
                    }
                    listRegisteredUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Occured During Reading File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Biztos benne, hogy törölni szeretné az elemeket?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                Users.Clear();
                rchTxtBxResult.Clear();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchedText = txtSearch.Text;
            if (searchedText == "" || searchedText == null)
            {
                listRegisteredUsers();
            } else
            {
                List<User> searchedUsers = new List<User>();
                foreach (User user in Users)
                {
                    if(user.UserName.Contains(searchedText))
                    {
                        searchedUsers.Add(user);
                    } else if (user.Password.Contains(searchedText))
                    {
                        searchedUsers.Add(user);
                    }
                }
                listRegisteredUsers(searchedUsers);
                StartSelection = rchTxtBxResult.Text.IndexOf(searchedText);
                LengthSelection = searchedText.Length;
            }
        }
        private void rchTxtBxResult_Click(object sender, EventArgs e)
        {
            if(txtSearch.Text != "")
            {
                RichTextBox txtBox = (RichTextBox)sender;
                txtBox.SelectionStart = StartSelection;
                if (txtBox.Tag == null)
                {
                    txtBox.Tag = true;
                    txtBox.SelectionLength = LengthSelection;
                }
                else
                {
                    txtBox.Tag = null;
                    txtBox.SelectionLength = 0;
                }
            }
        }
        private static void calculateRemainingCharacters(int maxLength, TextBox txtBox, Label lbl)
        {
            lbl.Text = $"{maxLength - txtBox.TextLength}";
        }
        private static void checkInputFields(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox || control is MaskedTextBox)
                {
                    if (control.Text == "" || control.Text is null || control.Text == "  .  .")
                    {
                        throw new EmptyValueException("Üres mező!", field: $"{control.AccessibleName}");
                    }
                }
                else
                {
                    checkInputFields(control);
                }
            }
        }
        private static void clearInputFields(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox || control is MaskedTextBox)
                {
                    control.Text = "";
                }
                else
                {
                    clearInputFields(control);
                }
            }
        }
        private void listRegisteredUsers(List<User> usersCollection = null)
        {
            if(usersCollection == null)
            {
                usersCollection = Users;
            }
            rchTxtBxResult.Clear();
            foreach (User user in usersCollection)
            {
                rchTxtBxResult.Text += user;
                rchTxtBxResult.Text += Environment.NewLine;
            }
        }
    }
}
