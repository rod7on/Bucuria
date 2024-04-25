using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Dapper;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        String email, nume, prenume;

        int pret_minim, pret_maxim;
        String NumeleProdusului;

        string connStr = "Data Source=(localdb)\\Local;Initial Catalog=BucuriaBD;Integrated Security=True";


        private void Login_Click(object sender, EventArgs e)
        {
            email = textEmail.Text;
            nume = textNume.Text;
            prenume = textPrenume.Text;


            // Verificăm dacă unul sau mai multe câmpuri obligatorii sunt goale
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nume) || string.IsNullOrEmpty(prenume))
            {
                // Construim un mesaj care indică ce câmpuri trebuie completate
                string mesaj = "Completați următoarele câmpuri obligatorii:\n";

                if (string.IsNullOrEmpty(email))
                    mesaj += "- Email\n";
                if (string.IsNullOrEmpty(nume))
                    mesaj += "- Nume\n";
                if (string.IsNullOrEmpty(prenume))
                    mesaj += "- Prenume\n";

                // Afișăm MessageBox-ul cu mesajul corespunzător
                MessageBox.Show(mesaj, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            string query = "SELECT Nume FROM Clienti WHERE Email = @Email AND Nume = @Nume AND Prenume = @Prenume";

            // Conectare la baza de date și executare interogare
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Nume", nume);
                cmd.Parameters.AddWithValue("@Prenume", prenume);

                // Verifică dacă există un client cu datele introduse
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    //this.panel7.Show();
                    // Afișează un MessageBox cu numele clientului
                    MessageBox.Show("Bun venit, " + result.ToString() + "!");
                    this.panel9.Show();

                }
                else
                {
                    MessageBox.Show("Nu s-a găsit niciun client cu datele introduse.");
                }
            }
    }

        private void textEmail_MouseEnter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textEmail.Text))
            {
                textEmail.Text = "exemplu@gmail.com";
            }
        }

        private void textEmail_MouseLeave(object sender, EventArgs e)
        {
            if (textEmail.Text == "exemplu@gmail.com")
            {
                textEmail.Text = "";
            }
        }

        private void textNume_MouseEnter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textNume.Text))
            {
                textNume.Text = "ExempluNume";
            }
        }

        private void textNume_MouseLeave(object sender, EventArgs e)
        {
            if (textNume.Text == "ExempluNume")
            {
                textNume.Text = "";
            }
        }

        private void textPrenume_MouseEnter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textPrenume.Text))
            {
                textPrenume.Text = "ExempluPrenume";
            }
        }

        private void textPrenume_MouseLeave(object sender, EventArgs e)
        {
            if (textPrenume.Text == "ExempluPrenume")
            {
                textPrenume.Text = "";
            }
        }



        //Butonul de back pentru conectare
        private void backConect_Click(object sender, EventArgs e)
        {
            this.panel3.Hide();
            this.panel1.Show();
        }
        //buton de conectare
        private void buttonConect_Click(object sender, EventArgs e)
        {
            this.panel1.Hide();
            this.panel3.Show();
        }

        //Butonul de back pentru despre noi
        private void backDespreNoi_Click(object sender, EventArgs e)
        {
            this.panel2.Hide();
            this.panel1.Show();
        }

        //Butonul de back pentru harta
        private void backHarta_Click(object sender, EventArgs e)
        {
            this.panel4.Hide();
            this.panel1.Show();
           
        }

        //afisam harta
        private void buttonHarta_Click(object sender, EventArgs e)
        {
            this.panel1.Hide();
            this.panel4.Show();
        }

        //afisam produsele
        private void buttonProduse_Click(object sender, EventArgs e)
        {
            this.panel1.Hide();
            this.panel6.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.panel6.Hide();
            this.panel1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
                // Verificăm dacă textbox-ul nu este gol
                if (!string.IsNullOrEmpty(textBox2.Text))
            {
                // Construim query-ul SQL
                string query = "SELECT * FROM Comenzi WHERE CodComanda = @CodComanda";

                    // Deschidem conexiunea
                    conn.Open();

                // Cream un obiect SqlCommand
                SqlCommand cmd = new SqlCommand(query, conn);

                // Adaugam parametrul pentru codul comenzii
                cmd.Parameters.AddWithValue("@CodComanda", textBox2.Text);

                // Cream un obiect SqlDataReader pentru a citi datele din baza de date
                SqlDataReader reader = cmd.ExecuteReader();

                // Populam DataGridView-ul cu datele din baza de date
                dataGridView2.Rows.Clear(); // Curățăm orice date anterioare din DataGridView
                while (reader.Read())
                {
                        if (dataGridView2.Columns.Count == 0)
                        {
                            // Adăugați coloanele necesare
                            dataGridView2.Columns.Add("ClientId", "ID Client");
                            dataGridView2.Columns.Add("CodComanda", "Cod Comanda");
                            dataGridView2.Columns.Add("DataComanda", "Data Comanda");
                            dataGridView2.Columns.Add("NrProduse", "Numar Produse");
                        }
                        dataGridView2.Rows.Add(reader["IDClient"], reader["CodComanda"], reader["DataComanda"], reader["NrProduse"]);
                }

                    // Inchidem conexiunea si reader-ul
                    conn.Close();
                reader.Close();
            }
            else
            {
                MessageBox.Show("Introduceți un cod de comandă în TextBox.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Verificați dacă utilizatorul a introdus un cod de comandă valid
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                string codComanda = textBox2.Text;

                // Definiți conexiunea la baza de date
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    // Deschideți conexiunea
                    connection.Open();

                    // Definiți comanda SQL pentru ștergerea comenzii din baza de date
                    string query = "DELETE FROM Comenzi WHERE CodComanda = @CodComanda";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Adăugați parametrul pentru codul comenzii
                        command.Parameters.AddWithValue("@CodComanda", codComanda);

                        // Executați comanda de ștergere
                        int rowsAffected = command.ExecuteNonQuery();

                        // Verificați dacă comanda a fost ștearsă cu succes
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Comanda a fost ștearsă cu succes din baza de date.");
                        }
                        else
                        {
                            MessageBox.Show("Comanda cu acest cod nu există în baza de date.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Introduceți mai întâi un cod de comandă.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {// Verificați dacă utilizatorul a introdus un cod de comandă
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                string codComanda = textBox2.Text;

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    // Deschideți conexiunea
                    connection.Open();

                    // Actualizați numărul total de produse în comandă
                    string queryUpdate = "UPDATE Comenzi SET NrProduse = NrProduse + 1 WHERE CodComanda = @CodComanda";
                    SqlCommand updateCommand = new SqlCommand(queryUpdate, connection);
                    updateCommand.Parameters.AddWithValue("@CodComanda", codComanda);

                    try
                    {
                        // Executați comanda de actualizare
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Numărul de produse pentru comanda " + codComanda + " a fost actualizat cu succes.");
                        }
                        else
                        {
                            MessageBox.Show("Nu există o comandă cu codul " + codComanda + ".");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("A apărut o eroare: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Introduceți mai întâi un cod de comandă.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Verificati daca utilizatorul a introdus un cod de comanda
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                string codComanda = textBox2.Text;

             
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    // Deschideti conexiunea
                    connection.Open();

                    // Verificati daca comanda exista in baza de date
                    string queryCheck = "SELECT COUNT(*) FROM Comenzi WHERE CodComanda = @CodComanda";
                    SqlCommand checkCommand = new SqlCommand(queryCheck, connection);
                    checkCommand.Parameters.AddWithValue("@CodComanda", codComanda);
                    int comandaExists = (int)checkCommand.ExecuteScalar();

                    if (comandaExists > 0)
                    {
                        // Actualizati numarul total de produse in comanda
                        string queryUpdate = "UPDATE Comenzi SET NrProduse = NrProduse - 1 WHERE CodComanda = @CodComanda";
                        SqlCommand updateCommand = new SqlCommand(queryUpdate, connection);
                        updateCommand.Parameters.AddWithValue("@CodComanda", codComanda);

                        try
                        {
                            // Executati comanda de actualizare
                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Numarul de produse pentru comanda " + codComanda + " a fost actualizat cu succes.");
                            }
                            else
                            {
                                MessageBox.Show("Nu exista un produs in comanda cu codul " + codComanda + ".");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("A aparut o eroare: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nu exista o comanda cu codul " + codComanda + ".");
                    }
                }
            }
            else
            {
                MessageBox.Show("Introduceti mai intai un cod de comanda.");
            }
        }

        private void Cauta_Click_1(object sender, EventArgs e)
        {
            // Verificați dacă valorile introduse în numericUpDown sunt valide
            if (pretMinim.Value <= pretMaxim.Value)
            {
                pret_minim = Convert.ToInt32(pretMinim.Value);
                pret_maxim = Convert.ToInt32(pretMaxim.Value);
                NumeleProdusului = textBox1.Text;

                //va urma cod in st. 2
            }
            else
            {
                MessageBox.Show("Prețul minim trebuie să fie mai mic sau egal cu prețul maxim.");
            }

            using (IDbConnection db = new SqlConnection(connStr))
            {
                // Interogare pentru a selecta produsele din baza de date care se potrivesc criteriilor
                string query = "SELECT * FROM Produse WHERE Nume = @Nume AND Pret BETWEEN @PretMinim AND @PretMaxim";
                var produseGasite = db.Query<Produs>(query, new { Nume = NumeleProdusului, PretMinim = pret_minim, PretMaxim = pret_maxim });

                // Adaugarea produselor găsite în DataGridView
                dataGridView1.DataSource = produseGasite.ToList();
            }

        }

        public Form1()
        {
            InitializeComponent();

        }
        //Schimbam panelurile cand dam click pe Despre Noi
        private void buttonDespreNoi_Click(object sender, EventArgs e)
        {
            this.panel1.Hide();
            this.panel2.Show();
        }

    }
}
public class Produs
{
    public int Id { get; set; }
    public string Nume { get; set; }
    public decimal Pret { get; set; }
   
}