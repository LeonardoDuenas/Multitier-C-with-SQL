using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace DAL
{
    internal class Connect
    {
        private static String FProConnectionString = GetConnectString();

        internal static String ConnectionString { get => FProConnectionString; }

        private static String GetConnectString()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "College1en";
            cs.UserID = "sa";
            cs.Password = "sysadm";
            return cs.ConnectionString;
        }
    }

    internal class DataTables
    {
        private static SqlDataAdapter adapterPrograms = InitAdapterPrograms();
        private static SqlDataAdapter adapterCourses = InitAdapterCourses();
        private static SqlDataAdapter adapterStudents = InitAdapterStudents();
        private static SqlDataAdapter adapterEnrollment = InitAdapterEnrollment();

        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitAdapterPrograms()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Programs ORDER BY ProgId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterCourses()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Courses ORDER BY CId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterStudents()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Students ORDER BY StId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterEnrollment()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Enrollment ORDER BY CId, StId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            loadPrograms(ds);
            loadCourses(ds);
            loadStudents(ds);
            loadEnrollment(ds);
            return ds;
        }

        private static void loadPrograms(DataSet ds)
        {
            adapterPrograms.Fill(ds, "Programs");

            ds.Tables["Programs"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["Programs"].Columns["Prog_Name"].AllowDBNull = false;

            ds.Tables["Programs"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Programs"].Columns["ProgId"]};
        }

        private static void loadCourses(DataSet ds)
        {
            adapterCourses.Fill(ds, "Courses");

            ds.Tables["Courses"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["CName"].AllowDBNull = false;

            ds.Tables["Courses"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Courses"].Columns["CId"]};

            ForeignKeyConstraint myFK01 = new ForeignKeyConstraint("MyFK01",
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Courses"].Columns["ProgId"]
                }
            );
            myFK01.DeleteRule = Rule.None;
            myFK01.UpdateRule = Rule.None;
            ds.Tables["Courses"].Constraints.Add(myFK01);
        }

        private static void loadStudents(DataSet ds)
        {
            adapterStudents.Fill(ds , "Students");

            ds.Tables["Students"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Students"].Columns["StName"].AllowDBNull = false;

            ds.Tables["Students"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Students"].Columns["StId"]};

            ForeignKeyConstraint myFK02 = new ForeignKeyConstraint("MyFK02",
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Students"].Columns["ProgId"]
                }
            );
            myFK02.DeleteRule = Rule.Cascade;
            myFK02.UpdateRule = Rule.Cascade;
            ds.Tables["Students"].Constraints.Add(myFK02);
        }

        private static void loadEnrollment(DataSet ds)
        {
            adapterEnrollment.Fill(ds, "Enrollment");

            ds.Tables["Enrollment"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Enrollment"].Columns["CId"].AllowDBNull = false;

            ds.Tables["Enrollment"].PrimaryKey = new DataColumn[2]
                    { ds.Tables["Enrollment"].Columns["StId"], ds.Tables["Enrollment"].Columns["CId"] };


            ForeignKeyConstraint myFK03 = new ForeignKeyConstraint("myFK03",
                new DataColumn[]{
                    ds.Tables["Courses"].Columns["CId"]
                },
                new DataColumn[] {
                    ds.Tables["Enrollment"].Columns["CId"]
                }
            );
            myFK03.DeleteRule = Rule.Cascade;
            myFK03.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollment"].Constraints.Add(myFK03);

            ForeignKeyConstraint myFK04 = new ForeignKeyConstraint("myFK04",
                new DataColumn[]{
                    ds.Tables["Students"].Columns["StId"]
                },
                new DataColumn[] {
                    ds.Tables["Enrollment"].Columns["StId"]
                }
            );
            myFK04.DeleteRule = Rule.Cascade;
            myFK04.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollment"].Constraints.Add(myFK04);
        }

        internal static SqlDataAdapter getAdapterPrograms()
        {
            return adapterPrograms;
        }
        internal static SqlDataAdapter getAdapterCourses()
        {
            return adapterCourses;
        }
        internal static SqlDataAdapter getAdapterStudents()
        {
            return adapterStudents;
        }
        internal static SqlDataAdapter getAdapterEnrollment()
        {
            return adapterEnrollment;
        }
        internal static DataSet getDataSet()
        {
            return ds;
        }

    }

    internal class Programs
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterPrograms();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetPrograms()
        {
            return ds.Tables["Programs"];
        }

        internal static int UpdatePrograms()
        {
            if (!ds.Tables["Programs"].HasErrors)
            {
                return adapter.Update(ds.Tables["Programs"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Courses
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterCourses();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetCourses()
        {
            return ds.Tables["Courses"];
        }

        internal static int UpdateCourses()
        {
            if (!ds.Tables["Courses"].HasErrors)
            {
                return adapter.Update(ds.Tables["Courses"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Students
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterStudents();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetStudents()
        {
            return ds.Tables["Students"];
        }

        internal static int UpdateStudents()
        {
            if (!ds.Tables["Students"].HasErrors)
            {
                return adapter.Update(ds.Tables["Students"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Enrollment
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterEnrollment();
        private static DataSet ds = DataTables.getDataSet();

        private static DataTable displayEnrollments = null;

        internal static DataTable GetDisplayEnrollement()
        {
            ds.Tables["Enrollment"].AcceptChanges();

            /* sql query for the display
             SELECT Students.StId, StName, Courses.CId, CName, final_grade, Programs.ProgId, Programs.Prog_Name 
             FROM Enrollment, Students, Courses, Programs
             WHERE Students.StId = Enrollment.StId AND Courses.CId = Enrollment.CId AND Students.ProgId = Programs.ProgId AND Courses.ProgId = Programs.ProgId;
             */

            var query = (
                from enrollment in ds.Tables["Enrollment"].AsEnumerable()
                from student in ds.Tables["Students"].AsEnumerable()
                from course in ds.Tables["Courses"].AsEnumerable()
                from program in ds.Tables["Programs"].AsEnumerable()
                where enrollment.Field<string>("Stid") == student.Field<string>("StId")
                where enrollment.Field<string>("CId") == course.Field<string>("CId")
                where student.Field<string>("ProgId") == program.Field<string>("ProgId")
                select new
                {
                    StId = student.Field<string>("StId"),
                    StName = student.Field<string>("StName"),
                    CId = course.Field<string>("CId"),
                    CName = course.Field<string>("CName"),
                    ProgName = program.Field<string>("Prog_Name"),
                    ProgId = program.Field<string>("ProgId"),
                    finalGrade = enrollment.Field<Nullable<int>>("final_grade")
                }) ;

            DataTable result = new DataTable();
            result.Columns.Add("StId");
            result.Columns.Add("StName");
            result.Columns.Add("CId");
            result.Columns.Add("CName");
            result.Columns.Add("Prog_Name");
            result.Columns.Add("ProgId");
            result.Columns.Add("final_grade");
            
            

            foreach(var x in query)
            {
                object[] allFields = { x.StId, x.StName, x.CId, x.CName, x.ProgName ,x.ProgId, x.finalGrade };
                result.Rows.Add(allFields);
            }

            displayEnrollments = result;

            return displayEnrollments;
        }

        internal static int InsertData(string[] a)
        {
            var test = (
                from enrollment in ds.Tables["Enrollment"].AsEnumerable()
                where enrollment.Field<string>("StId") == a[0]
                where enrollment.Field<string>("CId") == a[1]
                select enrollment);

            if(test.Count() > 0)
            {
                MessageBox.Show("This enrollment already exists");
                return -1;
            }
            try
            {                
                DataRow line = ds.Tables["Enrollment"].NewRow();
                line.SetField("StId", a[0]);
                line.SetField("CId", a[1]);
                ds.Tables["Enrollment"].Rows.Add(line);

                adapter.Update(ds.Tables["Enrollment"]);

                if (displayEnrollments != null)
                {
                    var query = (
                        from student in ds.Tables["Students"].AsEnumerable()
                        from courses in ds.Tables["Courses"].AsEnumerable()
                        from programs in ds.Tables["Programs"].AsEnumerable()
                        where student.Field<string>("StId") == a[0]
                        where courses.Field<string>("CId") == a[1]
                        where programs.Field<string>("ProgId") == student.Field<string>("ProgId")
                        select new
                        {
                            StId = student.Field<string>("StId"),
                            StName = student.Field<string>("StName"),
                            CId = courses.Field<string>("CId"),
                            CName = courses.Field<string>("CName"),
                            ProgId = programs.Field<string>("ProgId"),
                            Prog_Name = programs.Field<string>("Prog_Name"),
                            finalGrade = line.Field<Nullable<int>>("final_grade")
                        });

                    var r = query.Single();
                    displayEnrollments.Rows.Add(new object[] { r.StId, r.StName, r.CId, r.CName, r.Prog_Name, r.ProgId, r.finalGrade });
                }

                return 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Insertion / Update rejected");
                return -1;
            }
        }

        internal static int UpdateData(string[] a)
        {
            return 0;
        }

        internal static int DeleteData(List<string[]> lId)
        {
            //Adapt using query syntax to select the row (so the enollment stid and cid) FROM ENROLLMENT
            //Would not work, because the delete needs a List of arrays -> you can delete multiple rows at the same time.

            try
            {
                var lines = ds.Tables["Enrollment"].AsEnumerable()
                                .Where(s =>
                                   lId.Any(x => (x[0] == s.Field<string>("StId") && x[1] == s.Field<string>("CId"))));


                foreach (var line in lines)
                {
                    if (line.Field<string>("final_grade") != null)
                    {
                        MessageBox.Show("Final grade already exist");
                        return -1;
                    }
                    
                    line.Delete();
                }

                adapter.Update(ds.Tables["Enrollment"]);


                if (displayEnrollments != null)
                {
                    foreach (var p in lId)
                    {
                        var r = displayEnrollments.AsEnumerable()
                                .Where(s => (s.Field<string>("StId") == p[0] && s.Field<string>("CId") == p[1]))
                                .Single();
                        displayEnrollments.Rows.Remove(r);
                    }
                }

                return 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Update / Deletion rejected");
                return -1;
            }
        }

        internal static int UpdateFinalGrade(string[] a, Nullable<int> final_grade)
        {
            try
            {
                //might not work
                /*
                var line = ds.Tables["Enrollment"].AsEnumerable()
                                    .Where(s =>
                                      (s.Field<string>("StId") == a[0] && s.Field<string>("CId") == a[1]))
                                    .Single();
                */
                
                var line = (
                    from enrollment in ds.Tables["Enrollment"].AsEnumerable()
                    where enrollment.Field<string>("StId") == a[0]
                    where enrollment.Field<string>("CId") == a[1]
                    select enrollment).Single();

                line.SetField("final_grade", final_grade);

                adapter.Update(ds.Tables["Enrollment"]);

                if(displayEnrollments != null)
                {
                    var r = displayEnrollments.AsEnumerable()
                                    .Where(s =>
                                      (s.Field<string>("StId") == a[0] && s.Field<string>("CId") == a[1]))
                                    .Single();
                    r.SetField("final_grade", final_grade);
                }
                return 0;
            }
            catch (Exception e)
            {
                MessageBox.Show("Update / Deletion rejection");
                return -1;
            }
        }
    }
}
