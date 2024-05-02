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
    public partial class Form1 : Form
    {

        internal enum Grids
        {
            Prog,
            Course,
            Stud,
            Enrollment
        }

        internal static Form1 current;

        private Grids grid;

        private bool OKToChange = true;

        public Form1()
        {
            current = this;
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            new Form2();
            Form2.current.Visible = false;

            Text = "Student courses";
            dataGridView1.Dock = DockStyle.Fill;
        }

        private void programsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange)
            {
                grid = Grids.Prog;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource1.DataSource = DAL.Programs.GetPrograms();
                bindingSource1.Sort = "ProgId";
                dataGridView1.DataSource = bindingSource1;

                dataGridView1.Columns["Prog_Name"].HeaderText = "Program Name";
                dataGridView1.Columns["ProgId"].DisplayIndex = 0;

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange)
            {
                grid = Grids.Stud;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource3.DataSource = DAL.Students.GetStudents();
                bindingSource3.Sort = "StId";
                dataGridView1.DataSource = bindingSource3;

                dataGridView1.Columns["StName"].HeaderText = "Student Name";
                dataGridView1.Columns["StId"].DisplayIndex = 0;
                dataGridView1.Columns["ProgId"].DisplayIndex = 1;
            }

        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange)
            {
                grid = Grids.Course;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource2.DataSource = DAL.Courses.GetCourses();
                bindingSource2.Sort = "CId";
                dataGridView1.DataSource = bindingSource2;

                dataGridView1.Columns["CName"].HeaderText = "Course Name";
                dataGridView1.Columns["CId"].DisplayIndex = 0;
                dataGridView1.Columns["CName"].DisplayIndex = 1;
                dataGridView1.Columns["ProgId"].DisplayIndex = 2;

            }
        }

        private void enrollmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange && (grid != Grids.Enrollment))
            {
                grid = Grids.Enrollment;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource4.DataSource = DAL.Enrollment.GetDisplayEnrollement();
                bindingSource4.Sort = "StId, CId";
                dataGridView1.DataSource = bindingSource4;

                dataGridView1.Columns["StName"].HeaderText = "Student Name";
                dataGridView1.Columns["CName"].HeaderText = "Course Name";
                dataGridView1.Columns["Prog_Name"].HeaderText = "Program Name";
                dataGridView1.Columns["final_grade"].HeaderText = "Final Grade";

                dataGridView1.Columns["StId"].DisplayIndex = 0;
                dataGridView1.Columns["StName"].DisplayIndex = 1;
                dataGridView1.Columns["CId"].DisplayIndex = 2;
                dataGridView1.Columns["CName"].DisplayIndex = 3;
                dataGridView1.Columns["Prog_Name"].DisplayIndex = 4;
                dataGridView1.Columns["ProgId"].DisplayIndex = 5;
                dataGridView1.Columns["final_grade"].DisplayIndex = 6;
                
            }

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            if(BLL.Programs.UpdatePrograms() == -1) 
            {
                bindingSource1.ResetBindings(false);
            }
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
            if (BLL.Courses.UpdateCourses() == -1)
            {
                bindingSource2.ResetBindings(false);
            }
        }

        private void bindingSource3_CurrentChanged(object sender, EventArgs e)
        {
            if (BLL.Students.UpdateStudents() == -1)
            {
                bindingSource3.ResetBindings(false);
            }
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2.current.Start(Form2.Modes.ADD, null);
        }

        private void finalGradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for final grade update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                Form2.current.Start(Form2.Modes.GRADING, c);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("At least one line must be selected for deletion");
            }
            else // (c.Count > 1)
            {
                List<string[]> lId = new List<string[]>();
                for (int i = 0; i < c.Count; i++)
                {
                    lId.Add(new string[] { "" + c[i].Cells["StId"].Value, "" + c[i].Cells["CId"].Value });
                }
                DAL.Enrollment.DeleteData(lId);
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                if ("" + c[0].Cells["final_grade"].Value == "")
                {
                    Form2.current.Start(Form2.Modes.MODIFY, c);
                }
                else
                {
                    MessageBox.Show("To update this line, final grade value must be removed first.");
                }
            }
        }

        private void menuStrip1_Click(object sender, EventArgs e)
        {
            OKToChange = true;
            Validate();
            if (grid == Grids.Stud)
            {
                
                if (BLL.Students.UpdateStudents() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (grid == Grids.Course)
            {
                
                if (BLL.Courses.UpdateCourses() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (grid == Grids.Prog)
            {
                if (BLL.Programs.UpdatePrograms() == -1)
                {
                    OKToChange = false;
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Insertion / Update rejected", "Data Error");
        }
    }
}
