using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using OceanicAirlines.Models;

namespace OceanicAirlines
{
    public class DataService : IDataService
    {
        private string connectionString = "Data Source=dbs-oa-t2.database.windows.net;Initial Catalog=db-oa-t2;Persist Security Info=True;User ID=oaadmin;Password=netcompany-123";
        public List<City> GetCities()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM AllCities", connection);
            SqlDataReader reader = command.ExecuteReader();
            List<City> cities = new List<City>();
            while (reader.Read())
            {
                cities.Add(new City(reader.GetInt32(0), reader.GetValue(1).ToString(), reader.GetValue(2).ToString()));
            }
            reader.Close();
            connection.Close();
            return cities;
        }

        public List<SegmentDatabaseEntity> GetSegments()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT startCity, cityStart.name, cityStart.danishName, endCity, cityEnd.name, cityEnd.danishName FROM CompanySegments INNER JOIN AllCities cityEnd ON CompanySegments.endCity = cityEnd.ID INNER JOIN AllCities cityStart ON CompanySegments.startCity = cityStart.ID", connection);
            SqlDataReader reader = command.ExecuteReader();
            List<SegmentDatabaseEntity> segments = new List<SegmentDatabaseEntity>();
            while (reader.Read())
            {
                segments.Add(new SegmentDatabaseEntity(
                    new City(reader.GetInt32(0), reader.GetValue(1).ToString(), reader.GetValue(2).ToString()),
                    new City(reader.GetInt32(3), reader.GetValue(4).ToString(), reader.GetValue(5).ToString())
                    ));
            }
            reader.Close();
            connection.Close();
            return segments;
        }

        public string GetPasswordHash(string email)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT passwordHash FROM UserAccounts WHERE email ='" + email + "';", connection);
            SqlDataReader reader = command.ExecuteReader();

            string passwordHash = "";
            while (reader.Read())
            {
                passwordHash = reader.GetValue(0).ToString();
            }
            
            //while (reader.Read())
           // {
             //   users.Add(new User(reader.GetInt32(0), reader.GetValue(1).ToString(), reader.GetValue(2).ToString()));
            //}
            reader.Close();
            connection.Close();
            return passwordHash;

        }
            
    }
}
