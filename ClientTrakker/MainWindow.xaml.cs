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
using System.Globalization;

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
            ShowButtons();
            ShowAllReminders();
        }

        private void ShowAllClients()
        {
            try
            {
                string query = "select * from Client order by ClientFirstName asc";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                using (sqlDataAdapter)
                {
                    DataTable AllClientsTable = new DataTable();
                    sqlDataAdapter.Fill(AllClientsTable);
                    lstClients.SelectedValuePath = "Id";
                    lstClients.ItemsSource = AllClientsTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConnection.Close();
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
                ClearClientSelection.IsEnabled = true;
            }
            catch (Exception)
            {
                //throw ex;
            }
            finally
            {
                sqlConnection.Close();
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
            ClearClientSelection.IsEnabled = false;
        }

        private void ClearClientSelection_Click(object sender, RoutedEventArgs e)
        {
            lstClients.UnselectAll();
            ClearClientData();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            if( // first name
                Validator.IsNotEmpty(txtClientFirstName, "First name") &&
                Validator.IsLetters(txtClientFirstName, "first name") &&
                // last name
                Validator.IsNotEmpty(txtClientLastName, "Last name") &&
                Validator.IsLetters(txtClientLastName, "last name") &&
                // client phone
                Validator.IsNotEmpty(txtClientPhone, "Phone number") &&
                Validator.IsPhoneNumber(txtClientPhone, "phone number") &&
                // client email
                Validator.IsNotEmpty(txtClientEmail, "Email") &&
                Validator.IsEmail(txtClientEmail, "Email") &&
                // client address
                Validator.IsAddress(txtClientAddress, "address") &&
                // client city
                Validator.IsLettersAndSpaces(txtClientCity, "city") &&
                // client province
                Validator.IsLettersAndSpaces(txtClientProvince, "province") &&
                // client country
                Validator.IsLettersAndSpaces(txtClientCountry, "country"))
            {
                try
                {
                    string query = @"insert into Client values (@ClientFirstName, @ClientLastName, @ClientPhone, @ClientEmail, @ClientAddress, @ClientCity, @ClientProvince, @ClientCountry)";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@ClientFirstName", Validator.ToTitleCase(txtClientFirstName.Text));
                    sqlCommand.Parameters.AddWithValue("@ClientLastName", Validator.ToTitleCase(txtClientLastName.Text));
                    sqlCommand.Parameters.AddWithValue("@ClientPhone", txtClientPhone.Text);
                    sqlCommand.Parameters.AddWithValue("@ClientEmail", txtClientEmail.Text);
                    sqlCommand.Parameters.AddWithValue("@ClientAddress", Validator.ToTitleCase(txtClientAddress.Text));
                    sqlCommand.Parameters.AddWithValue("@ClientCity", Validator.ToTitleCase(txtClientCity.Text));
                    sqlCommand.Parameters.AddWithValue("@ClientProvince", Validator.ToTitleCase(txtClientProvince.Text));
                    sqlCommand.Parameters.AddWithValue("@ClientCountry", Validator.ToTitleCase(txtClientCountry.Text));
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

        private void ShowButtons()
        {            
            if(lstClients.SelectedIndex >= 0)
            {
                ClearClientSelection.IsEnabled = true;
            }
            else
            {
                ClearClientSelection.IsEnabled = false;
            }
        }

        private void AddReminder_Click(object sender, RoutedEventArgs e)
        {
            try
            {         
                string query = @"insert into Reminder (ReminderTitle, ReminderDetails, ReminderDate, DateAdded) values (@ReminderTitle, @ReminderDetails, @ReminderDate, @DateAdded)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ReminderTitle", Validator.ToTitleCase(txtReminderTitle.Text));
                sqlCommand.Parameters.AddWithValue("@ReminderDetails", txtReminderDetails.Text);
                sqlCommand.Parameters.AddWithValue("@ReminderDate", ReminderDate.SelectedDate);
                sqlCommand.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                txtReminderTitle.Text = "";
                txtReminderDetails.Text = "";
                ReminderDate.Text = "";
            }
        }

        private void ShowAllReminders()
        {
            try
            {
                string query = "select * from Reminder order by ReminderDate asc";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                using (sqlDataAdapter)
                {
                    DataTable AllRemindersTable = new DataTable();
                    sqlDataAdapter.Fill(AllRemindersTable);
                    lstReminders.SelectedValuePath = "Id";
                    lstReminders.ItemsSource = AllRemindersTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
