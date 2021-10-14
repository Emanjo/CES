using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;



namespace OceanicAirlines
{
    public class DataService
    {
        private string connectionString = "Data Source=dbs-oa-t2.database.windows.net;Initial Catalog=db-oa-t2;Persist Security Info=True;User ID=oaadmin;Password=netcompany-123";
        public List<City> getCities()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Cities", connection);
            SqlDataReader reader = command.ExecuteReader();
            List<City> cities = new List<City>();
            while (reader.Read())
            {
                cities.Add(new City(reader.GetInt32(0), reader.GetValue(1).ToString()));
            }
            reader.Close();
            connection.Close();
            return cities;
        }

        public List<Segment> getSegments()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT startCity, cityStart.name, endCity, cityEnd.name FROM CompanySegments INNER JOIN Cities cityEnd ON CompanySegments.endCity = cityEnd.ID INNER JOIN Cities cityStart ON CompanySegments.startCity = cityStart.ID", connection);
            SqlDataReader reader = command.ExecuteReader();
            List<Segment> segments = new List<Segment>();
            while (reader.Read())
            {
                segments.Add(new Segment(
                    new City(reader.GetInt32(0), reader.GetValue(1).ToString()),
                    new City(reader.GetInt32(2), reader.GetValue(3).ToString())
                    ));
            }
            reader.Close();
            connection.Close();
            return segments;
        }
    }
}
