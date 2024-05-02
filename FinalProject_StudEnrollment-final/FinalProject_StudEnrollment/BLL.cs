using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLL
{
    class Programs
    {
        internal static int UpdatePrograms()
        {
            DataSet ds = DAL.DataTables.getDataSet();

            DataTable dt = ds.Tables["Programs"].GetChanges(DataRowState.Added | DataRowState.Modified);

            if (dt != null)
            {
                if (dt.AsEnumerable().Any(r => !IsValidProgId(r.Field<string>("ProgId"))))
                {
                    MessageBox.Show("Invalid Id for Programs");
                    ds.RejectChanges();
                    return -1;
                }
                else
                {
                    return DAL.Programs.UpdatePrograms();
                }
            }
            else
            {
                return DAL.Programs.UpdatePrograms();
            }
        }

        private static bool IsValidProgId(string progId)
        {
            bool r = true;
            if (progId.Length != 5) { r = false; }
            else if (progId[0] != 'P') { r = false; }
            else
            {
                for (int i = 1; i < progId.Length; i++)
                {
                    r = r && Char.IsDigit(progId[i]);
                }
            }
            return r;
        }
    }

    class Courses
    {
        internal static int UpdateCourses()
        {
            DataSet ds = DAL.DataTables.getDataSet();

            DataTable dt = ds.Tables["Courses"].GetChanges(DataRowState.Added | DataRowState.Modified);

            if (dt != null)
            {
                if (dt.AsEnumerable().Any(r => !IsValidCId(r.Field<string>("CId"))))
                {
                    MessageBox.Show("Invalid Id for Courses");
                    ds.RejectChanges();
                    return -1;
                }
                else
                {
                    return DAL.Courses.UpdateCourses();
                }
            }
            else
            {
                return DAL.Courses.UpdateCourses();
            }
        }

        private static bool IsValidCId(string CId)
        {
            bool r = true;
            if (CId.Length != 7) { r = false; }
            else if (CId[0] != 'C') { r = false; }
            else
            {
                for (int i = 1; i < CId.Length; i++)
                {
                    r = r && Char.IsDigit(CId[i]);
                }
            }
            return r;
        }
    }

    class Students
    {
        internal static int UpdateStudents()
        {
            DataSet ds = DAL.DataTables.getDataSet();

            DataTable dt = ds.Tables["Students"].GetChanges(DataRowState.Added | DataRowState.Modified);

            if (dt != null)
            {
                if (dt.AsEnumerable().Any(r => !IsValidStId(r.Field<string>("StId"))))
                {
                    MessageBox.Show("Invalid Id for Students");
                    ds.RejectChanges();
                    return -1;
                }
                else
                {
                    return DAL.Students.UpdateStudents();
                }
            }
            else
            {
                return DAL.Students.UpdateStudents();
            }
        }

        private static bool IsValidStId(string StId)
        {
            bool r = true;
            if (StId.Length != 9) { r = false; }
            else if (StId[0] != 'S') { r = false; }
            else
            {
                for (int i = 1; i < StId.Length; i++)
                {
                    r = r && Char.IsDigit(StId[i]);
                }
            }
            return r;
        }
    }

    class Enrollment
    {
        internal static int UpdateFinalGrade(string[] a, string inputFinalGrade)
        {
            Nullable<int> fGrade;
            int temp;

            if (inputFinalGrade == "")
            {
                fGrade = null;
            }
            else if (int.TryParse(inputFinalGrade, out temp) && (0 <= temp && temp <= 100))
            {
                fGrade = temp;
            }
            else
            {
                MessageBox.Show("Final grade must be between 0-100");
                return -1;
            }

            return DAL.Enrollment.UpdateFinalGrade(a, fGrade);
        }
            
        internal static int InsertEnrollment(string[] a)
        {

            var progStudent = (
                from student in DAL.Students.GetStudents().AsEnumerable()
                where student.Field<string>("StId") == a[0]
                select student.Field<string>("ProgId")).SingleOrDefault();

            var progCourse = (
                from courses in DAL.Courses.GetCourses().AsEnumerable()
                where courses.Field<string>("CId") == a[1]
                select courses.Field<string>("ProgId")).SingleOrDefault();

            if (progStudent != progCourse)
            {
                MessageBox.Show("This course is not in the students program " + progStudent + "--" + progCourse);
                return -1;
            }
            else
            {
                return DAL.Enrollment.InsertData(a);
            }
        }

    }
}
