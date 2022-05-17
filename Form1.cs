using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WF_Exercicio1
{
    public partial class Form1 : Form
    {
        bool temCamposVazios;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panelMain.Visible = true;
            panelEditar.Visible = false;
            panelAdicionar.Visible = false;
            dataGridView1.Enabled = true;

            string con = @"Data Source=DESKTOP-HU2PF0R;Initial Catalog=dbPessoas;Integrated Security=True";
            SqlConnection objCon = new SqlConnection(con);

            //abre a conexão
            objCon.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Pessoa", objCon);
            cmd.ExecuteNonQuery();

            //Adaptar
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = cmd;
            da.Fill(ds);

            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = ds.Tables[0].TableName;

            //fecha conexão
            objCon.Close();

            AtualizaCampos();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            AtualizaCampos();
            if (dataGridView1.CurrentRow.Cells[0].Value.ToString() == "")
            {
                btnAtualizar.Enabled = false;
            }
            else
            {
                btnAtualizar.Enabled = true;
            }
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            panelAdicionar.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
        }

        public void AtualizaCampos()
        {
            txtNome.Enabled = true;
            txtSobrenome.Enabled = true;
            txtDataNascimento.Enabled = true;
            txtCelular.Enabled = true;
            try
            {
                txtNome.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtSobrenome.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtDataNascimento.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtCelular.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            }
            catch
            {
                txtNome.Text            = "";
                txtSobrenome.Text       = "";
                txtDataNascimento.Text  = "";
                txtCelular.Text         = "";
            }


            txtNome.Enabled = false;
            txtSobrenome.Enabled = false;
            txtDataNascimento.Enabled = false;
            txtCelular.Enabled = false;
        }

        public void EnableButtons()
        {
            txtNome.Enabled = true;
            txtSobrenome.Enabled = true;
            txtDataNascimento.Enabled = true;
            txtCelular.Enabled = true;
        }

        public void ClearButtons()
        {
            txtNome.Text = "";
            txtSobrenome.Text = "";
            txtDataNascimento.Text = "";
            txtCelular.Text = "";
        }

        public void DisableButtons()
        {
            txtNome.Enabled = true;
            txtSobrenome.Enabled = true;
            txtDataNascimento.Enabled = true;
            txtCelular.Enabled = true;
        }

        private void btnAtualizar_Click_1(object sender, EventArgs e)
        {
            EnableButtons();
            dataGridView1.Enabled = false;
            panelMain.Visible = false;
            panelEditar.Visible = true;
        }

        private void btnInserir_Click_1(object sender, EventArgs e)
        {
            EnableButtons();
            ClearButtons();
            dataGridView1.Enabled = false;
            panelMain.Visible = false;
            panelAdicionar.Visible = true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            EnableButtons();
            panelAdicionar.Visible = false;
            panelMain.Visible = true;
            Form1_Load(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panelEditar.Visible = false;
            panelMain.Visible = true;
            Form1_Load(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string con = @"Data Source=DESKTOP-HU2PF0R;Initial Catalog=dbPessoas;Integrated Security=True";
            SqlConnection objCon = new SqlConnection(con);

            string sql = @"DELETE FROM Pessoa WHERE ID = '" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";

            SqlCommand cmd = new SqlCommand(sql, objCon);
            cmd.CommandType = CommandType.Text;
            objCon.Open();
            try
            {
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    MessageBox.Show("Dado removido com sucesso!");
                }
                Form1_Load(sender, e);
                panelEditar.Visible = false;
                panelMain.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deu ruim" + ex.Message);
                throw;
            }
            finally { objCon.Close(); }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                VerificaCamposVazios();

                if (temCamposVazios)
                {
                    throw new Exception();
                } else
                {
                    string con = @"Data Source=DESKTOP-HU2PF0R;Initial Catalog=dbPessoas;Integrated Security=True";
                    SqlConnection objCon = new SqlConnection(con);

                    string sql = @"INSERT INTO Pessoa  (PrimeiroNome, UltimoNome, DataNascimento, Celular)
                                         VALUES ('" + txtNome.Text + "','" + txtSobrenome.Text + "','" + txtDataNascimento.Text + "','" + txtCelular.Text + "')";

                    SqlCommand cmd = new SqlCommand(sql, objCon);

                    objCon.Open();
                    try
                    {
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Dado cadastrado com sucesso!");
                        }
                        cmd.CommandType = CommandType.Text;
                        Form1_Load(sender, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Deu ruim" + ex.Message);
                        throw;
                    }
                    finally { objCon.Close(); }
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Preencha todos os campos");
            }

            
        }

        public void VerificaCamposVazios()
        {
            temCamposVazios = false;
            if (txtNome.Text == "" || txtSobrenome.Text == "" || txtCelular.Text == "" || txtDataNascimento.Text == "")
            {
                temCamposVazios = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                VerificaCamposVazios();

                if (temCamposVazios)
                {
                    throw new Exception();
                } else
                {
                    string con = @"Data Source=DESKTOP-HU2PF0R;Initial Catalog=dbPessoas;Integrated Security=True";
                    SqlConnection objCon = new SqlConnection(con);

                    string sql = @"UPDATE Pessoa  SET PrimeiroNome = '" + txtNome.Text + "', UltimoNome = '" + txtSobrenome.Text + "', DataNascimento = '" + txtDataNascimento.Text + "', Celular = '" + txtCelular.Text + "' WHERE ID = '" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
                    SqlCommand cmd = new SqlCommand(sql, objCon);

                    objCon.Open();
                    try
                    {
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Dado atualizado com sucesso!");
                        }

                        cmd.CommandType = CommandType.Text;
                        Form1_Load(sender, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Deu ruim" + ex.Message);
                        throw;
                    }
                    finally { objCon.Close(); }
                }
                
            } catch (Exception ex)
            {
                MessageBox.Show("Preencha todos os campos");
            } 
        }
    }
}
