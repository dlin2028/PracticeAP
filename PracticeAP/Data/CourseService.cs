using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PracticeAP.Data
{
    public class CourseService
    {
        private static List<(int, string)> courses = new List<(int, string)>();
        CancellationTokenSource tokenSource;

        public CourseService()
        {
            tokenSource = new CancellationTokenSource();
        }

        public async Task<string[]> GetCoursesAsync()
        {
            CancellationToken ct = tokenSource.Token;

            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();

            using (SqlConnection conn = new SqlConnection(configuration.GetConnectionString("localdb")))
            {
                await conn.OpenAsync(ct);
                await Task.Delay(1000);
                ct.ThrowIfCancellationRequested();

                using (SqlCommand command = new SqlCommand("GetAllCourses", conn) { CommandType = CommandType.StoredProcedure })
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    courses = new List<(int, string)>();

                    while (await reader.ReadAsync(ct))
                    {
                        ct.ThrowIfCancellationRequested();
                        var course = ((int)reader["CourseId"], reader["Name"].ToString());
                        courses.Add(course);
                    }
                }
            }
            return courses.Select(x => x.Item2).ToArray();
        }

        public void CancelThing()
        {
            tokenSource.Cancel();
        }
    }
}
