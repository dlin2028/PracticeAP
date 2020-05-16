using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PracticeAP.Data
{
    public class CourseService
    {
        private static List<(int, string)> courses = new List<(int, string)>();
        public Task<string[]> GetCoursesAsync()
        {
            return Task.Run(() =>
            {
                string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(projectPath)
                    .AddJsonFile("appsettings.json")
                    .Build();

                using (SqlConnection conn = new SqlConnection(configuration.GetConnectionString("localdb")))
                {
                    conn.Open();

                    using (SqlCommand command = new SqlCommand("EXEC GetAllCourses", conn))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        courses = new List<(int, string)>();
                        while (reader.Read())
                        {
                            var course = (reader.GetInt32(0), reader.GetString(1));
                            courses.Add(course);
                        }
                    }
                }
                return courses.Select(x => x.Item2).ToArray();
            });
        }
}
}
