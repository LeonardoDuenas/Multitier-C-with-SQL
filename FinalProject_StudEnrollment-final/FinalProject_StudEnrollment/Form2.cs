using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject_StudEnrollment
{
    public partial class Form2 : Form
    {
        internal enum Modes
        {
            ADD,
            MODIFY,
            GRADING
        }

        internal static Form2 current;

        private Modes mode = Modes.ADD;

        private string[] enrollmentInitial;

        public Form2()
        {
            current = this;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;
            Text = "" + mode;

            comboBox1.DisplayMember = "StId";
            comboBox1.ValueMember = "StId";
            comboBox1.DataSource = DAL.Students.GetStudents();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;

            comboBox2.DisplayMember = "CId";
            comboBox2.ValueMember = "CId";
            comboBox2.DataSource = DAL.Courses.GetCourses();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.Enabled = false;

            if (((mode == Modes.MODIFY) || (mode == Modes.GRADING)) && (c != null))
            {
                comboBox1.SelectedValue = c[0].Cells["StId"].Value; //c is null
                comboBox2.SelectedValue = c[0].Cells["CId"].Value;
                textBox3.Text = "" + c[0].Cells["final_grade"].Value;
                enrollmentInitial = new string[] { (string)c[0].Cells["StId"].Value, (string)c[0].Cells["CId"].Value };
            }
            if (mode == Modes.ADD)
            {
                textBox1.Enabled = false;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
            }
            if (mode == Modes.MODIFY)
            {
                textBox1.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = true;
                enrollmentInitial = new string[] { (string)c[0].Cells["StId"].Value, (string)c[0].Cells["CId"].Value };
            }
            if (mode == Modes.GRADING)
            {
                textBox3.Enabled = true;
                textBox3.ReadOnly = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
            }

            ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var a = from r in DAL.Students.GetStudents().AsEnumerable()
                        where r.Field<string>("StId") == (string)comboBox1.SelectedValue
                        select new { Name = r.Field<string>("StName") };
                textBox1.Text = a.Single().Name;
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                var a = from r in DAL.Courses.GetCourses().AsEnumerable()
                        where r.Field<string>("CId") == (string)comboBox2.SelectedValue
                        select new { Name = r.Field<string>("CName") };
                textBox2.Text = a.Single().Name;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = -1;
            if (mode == Modes.ADD)
            {
                r = BLL.Enrollment.InsertEnrollment(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });
            }
            if (mode == Modes.MODIFY)
            {
                List<string[]> lId = new List<string[]>
                {
                    enrollmentInitial
                };

                r = BLL.Enrollment.InsertEnrollment(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });

                if (r == 0)
                {
                    r = DAL.Enrollment.DeleteData(lId);
                }
            }
            if (mode == Modes.GRADING)
            {
                r = BLL.Enrollment.UpdateFinalGrade(enrollmentInitial, textBox3.Text);
            }

            if (r == 0) { Close(); }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
