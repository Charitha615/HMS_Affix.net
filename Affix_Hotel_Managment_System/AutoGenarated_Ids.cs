﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Affix_Hotel_Managment_System
{
    class AutoGenarated_Ids
    {
        public static MySqlConnection connection = DBUtil.get_DBConnection();
        static String sql;

        public static String Generate_Id(String prefix)
        {
            string lastDigits = "";
            int digit_count = 0;
            String Id = "";

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                sql = $"select Last_No from autogenerated_ids where prefix = '{prefix}'";

                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lastDigits = reader.GetString(0);

                }
                reader.Close();
                reader.Dispose();
                //connection.Close();
                //check the length of the retrived number = 0 if so(starts with 0000)
                //else if  reduce the 0s according to the length
                //else if length >  4 then directly add it to the prefix

                digit_count = lastDigits.Length;
                int lastDigit = int.Parse(lastDigits);
                lastDigit = lastDigit  + 1;
                switch (digit_count)
                {
                    case 1:
                        Id = prefix + "0000" + lastDigit;
                        break;
                    case 2:
                        Id = prefix + "000" +  lastDigit;
                        break;
                    case 3:
                        Id = prefix + "00" + lastDigit;
                        break;
                    case 4:
                        Id = prefix + "0" + lastDigit;
                        break;
                    default:
                        Id = prefix + "" + lastDigit;
                        break;

                }

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                sql = $"UPDATE autogenerated_ids set Last_No={lastDigit} where(Prefix ='{prefix}')";
                MySqlCommand command1 = new MySqlCommand(sql,connection);
                command1.ExecuteNonQuery();
                connection.Close();
                command1.Dispose();

                return Id;
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                sql = $"INSERT INTO autogenerated_ids(Prefix,Last_No) values('{prefix}',1)";

                MySqlCommand cmd1 = new MySqlCommand(sql,connection);
                cmd1.ExecuteNonQuery();

                MessageBox.Show("Auto Genarated Id Inserted");
                connection.Close();
                cmd1.Dispose();
                Id = prefix + "0000" + 1;
                return Id;
            }


           
        }


    }
}
