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
            
            reader.Close();
            connection.Close();
            return passwordHash;
        }
        
        public int GetUserID(string email)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT ID FROM UserAccounts WHERE email ='" + email + "';", connection);
            SqlDataReader reader = command.ExecuteReader();

            int userID = 0;
            while (reader.Read())
            {
                userID = reader.GetInt32(0);
            }

            reader.Close();
            connection.Close();
            return userID;
        }

        public bool AddOrder(
            int lastLocation,
            string route,
            int userID, 
            double weight,
            double width,
            double height,
            double depth,
            double price,
            double time,
            string category)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = new SqlCommand("INSERT INTO TrackingRecords (dateReceived, status, lastLocation, route, userID, weight, dimensions, price, time, category) VALUES(@dateReceived, @status, @lastLocation, @route, @userID, @weight, @dimensions, @price, @time, @category);", connection);

            SqlParameter dateReceivedParam = new SqlParameter("@dateReceived", SqlDbType.VarChar);
            dateReceivedParam.Value = DateTime.Now.ToString();
            command.Parameters.Add(dateReceivedParam);

            SqlParameter statusParam = new SqlParameter("@status", SqlDbType.VarChar);
            statusParam.Value = "Order placed";
            command.Parameters.Add(statusParam);

            SqlParameter lastLocationParam = new SqlParameter("@lastLocation", SqlDbType.Int);
            lastLocationParam.Value = lastLocation;
            command.Parameters.Add(lastLocationParam);

            SqlParameter routeParam = new SqlParameter("@route", SqlDbType.VarChar);
            routeParam.Value = route;
            command.Parameters.Add(routeParam);

            SqlParameter userIDParam = new SqlParameter("@userID", SqlDbType.Int);
            userIDParam.Value = userID;
            command.Parameters.Add(userIDParam);

            SqlParameter weightParam = new SqlParameter("@weight", SqlDbType.Real);
            weightParam.Value = weight;
            command.Parameters.Add(weightParam);

            SqlParameter dimensionsParam = new SqlParameter("@dimensions", SqlDbType.VarChar);
            string tempDimensions = "Width = " + width + "; Height = " + height + "; Depth = " + depth;
            dimensionsParam.Value = tempDimensions;
            command.Parameters.Add(dimensionsParam);

            SqlParameter priceParam = new SqlParameter("@price", SqlDbType.Real);
            priceParam.Value = price;
            command.Parameters.Add(priceParam);

            SqlParameter timeParam = new SqlParameter("@time", SqlDbType.Real);
            timeParam.Value = time;
            command.Parameters.Add(timeParam);

            SqlParameter categoryParam = new SqlParameter("@category", SqlDbType.VarChar);
            categoryParam.Value = category;
            command.Parameters.Add(categoryParam);

            var suc = command.ExecuteNonQuery() > 0;
            connection.Close();
            return suc;
        }
    }
}
