using Dapper;
using OneToManyAK.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneToManyAK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Group> Groups { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            //LoadStudents();
            LoadGroups();
        }

        public async void LoadStudents()
        {
            var conn = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                var sql = @"
                        SELECT S.StudentId,S.Firstname,S.Age,
                        G.GroupId,G.Title
                        FROM Students AS S
                        INNER JOIN Groups AS G
                        ON S.GroupId=G.GroupId";

                var result = await connection.QueryAsync<Student, Group, Student>(sql,
                    (student, group) =>
                    {
                        student.Group = group;
                        student.GroupId = group.GroupId;
                        return student;
                    },splitOn:"GroupId");
                var students = result.ToList();
                myDataGrid.ItemsSource = students;
            }
        }

        //public async void LoadGroups()
        //{
        //    var conn = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
        //    using (var connection = new SqlConnection(conn))
        //    {
        //        var sql = @"
        //                SELECT G.GroupId,G.Title,S.StudentId,S.Firstname,
        //                S.Age
        //                FROM Groups AS G
        //                INNER JOIN Students AS S
        //                ON S.GroupId=G.GroupId";

        //        var result = await connection.QueryAsync<Group, Student, Group>(sql,
        //            (group, student) =>
        //            {
        //                group.Students.Add(student);
        //                student.GroupId = group.GroupId;
        //                student.Group = group;
        //                return group;
        //            },splitOn:"StudentId");
        //        Groups = result.ToList();
        //        myDataGrid.ItemsSource = Groups;
        //    }
        //}

        public async void LoadGroups()
        {
            var conn = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                var sql = @"
                        SELECT G.GroupId,G.Title
                        FROM Groups AS G
                       ";

                var result = await connection.QueryAsync<Group>(sql);
                myDataGrid.ItemsSource = result;
            }
        }

        private async void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = myDataGrid.SelectedItem as Group;
            var id = item.GroupId;
            var conn = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                var sql = @"
                        SELECT S.StudentId,S.Firstname,S.Age,S.GroupId
                        FROM Students AS S
                        WHERE S.GroupId=@gId
                       ";

                var result = await connection.QueryAsync<Student>(sql, new { gId = id });
                myDataGrid2.ItemsSource = result;
            }
        }

        private void myDataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
