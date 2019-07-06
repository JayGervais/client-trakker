using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Data.SqlClient;
using System.Data;

namespace ClientTrakker
{
    /// <summary>
    /// An application used to track clients
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["ClientTrakker.Properties.Settings.clienttrakkerConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);
            ShowAllClients();
        }

        private void ShowAllClients()
        {
            try
            {
                string query = "select * from Client";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                using (sqlDataAdapter)
                {
                    DataTable AllClientsTable = new DataTable();
                    sqlDataAdapter.Fill(AllClientsTable);
                    lstClients.SelectedValuePath = "Id";
                    lstClients.ItemsSource = AllClientsTable.DefaultView;
                }
            }
            catch (Exception)
            {
                // add exception
            }
        }

        private void ShowSelectedClientsInTextBox()
        {
            try
            {
                string query = @"select ClientFirstName, ClientLastName, ClientPhone, ClientEmail, ClientAddress, ClientCity, ClientProvince, ClientCountry from Client where Id = @Client_Id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@Client_Id", lstClients.SelectedValue);
                    DataTable ClientDataTable = new DataTable();
                    sqlDataAdapter.Fill(ClientDataTable);
                    txtClientFirstName.Text = ClientDataTable.Rows[0]["ClientFirstName"].ToString();
                    txtClientLastName.Text = ClientDataTable.Rows[0]["ClientLastName"].ToString();
                    txtClientPhone.Text = ClientDataTable.Rows[0]["ClientPhone"].ToString();
                    txtClientEmail.Text = ClientDataTable.Rows[0]["ClientEmail"].ToString();
                    txtClientAddress.Text = ClientDataTable.Rows[0]["ClientAddress"].ToString();
                    txtClientCity.Text = ClientDataTable.Rows[0]["ClientCity"].ToString();
                    txtClientProvince.Text = ClientDataTable.Rows[0]["ClientProvince"].ToString();
                    txtClientCountry.Text = ClientDataTable.Rows[0]["ClientCountry"].ToString();
                }
            }
            catch (Exception)
            {
              //  MessageBox.Show(ToString());
            }
        }

        private void ListAllClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedClientsInTextBox();
        }

        private void UpdateClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = @"update Client set ClientFirstName = @ClientFirstName, ClientLastName = @ClientLastName, ClientPhone = @ClientPhone, ClientEmail = @ClientEmail, ClientAddress = @ClientAddress, ClientCity = @ClientCity, ClientProvince = @ClientProvince, ClientCountry = @ClientCountry where Id = @Client_Id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Client_Id", lstClients.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@ClientFirstName", txtClientFirstName.Text);
                sqlCommand.Parameters.AddWithValue("@ClientLastName", txtClientLastName.Text);
                sqlCommand.Parameters.AddWithValue("@ClientPhone", txtClientPhone.Text);
                sqlCommand.Parameters.AddWithValue("@ClientEmail", txtClientEmail.Text);
                sqlCommand.Parameters.AddWithValue("@ClientAddress", txtClientAddress.Text);
                sqlCommand.Parameters.AddWithValue("@ClientCity", txtClientCity.Text);
                sqlCommand.Parameters.AddWithValue("@ClientProvince", txtClientProvince.Text);
                sqlCommand.Parameters.AddWithValue("@ClientCountry", txtClientCountry.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception)
            {
               MessageBox.Show(ToString());
            }
            finally
            {
                sqlConnection.Close();
                MessageBox.Show("Client Updated");
                ShowAllClients();
            }
        }

        private void DeleteClient()
        {
            try
            {
                string query = "delete from Client where id = @Client_Id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Client_Id", lstClients.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllClients();
                ClearClientData();
            }
        }

        private void deleteClientAccount(object sender, RoutedEventArgs e)
        {
            MessageBoxResult deleteClient = MessageBox.Show("Are you sure you want to delete this client?", "Confirmation", MessageBoxButton.YesNo);
            if (deleteClient == MessageBoxResult.Yes)
            {
                DeleteClient();
            }
        }

        private void ClearClientData()
        {
            txtClientFirstName.Text = String.Empty;
            txtClientLastName.Text = String.Empty;
            txtClientPhone.Text = String.Empty;
            txtClientEmail.Text = String.Empty;
            txtClientAddress.Text = String.Empty;
            txtClientCity.Text = String.Empty;
            txtClientProvince.Text = String.Empty;
            txtClientCountry.Text = String.Empty;
        }

        private void ClearClientSelection_Click(object sender, RoutedEventArgs e)
        {
            lstClients.UnselectAll();
            ClearClientData();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = @"insert into Client values (@ClientFirstName, @ClientLastName, @ClientPhone, @ClientEmail, @ClientAddress, @ClientCity, @ClientProvince, @ClientCountry)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ClientFirstName", txtClientFirstName.Text);
                sqlCommand.Parameters.AddWithValue("@ClientLastName", txtClientLastName.Text);
                sqlCommand.Parameters.AddWithValue("@ClientPhone", txtClientPhone.Text);
                sqlCommand.Parameters.AddWithValue("@ClientEmail", txtClientEmail.Text);
                sqlCommand.Parameters.AddWithValue("@ClientAddress", txtClientAddress.Text);
                sqlCommand.Parameters.AddWithValue("@ClientCity", txtClientCity.Text);
                sqlCommand.Parameters.AddWithValue("@ClientProvince", txtClientProvince.Text);
                sqlCommand.Parameters.AddWithValue("@ClientCountry", txtClientCountry.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllClients();
                ClearClientData();
            }
        }

    }
}
