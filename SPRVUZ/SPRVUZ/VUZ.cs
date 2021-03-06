﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPRVUZ
{
    public partial class VUZ : Form
    {
        private OleDbConnection connection = new OleDbConnection();

        public VUZ()
        {
            InitializeComponent();
            splitContainer1.Panel2.Visible = false;
            connection.ConnectionString = //@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=VUZBD.accdb";
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\gusei\source\repos\SPRVUZ\SPRVUZ\VUZBD.accdb";

        }

        private void VUZ_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "vUZBDDataSet.Города". При необходимости она может быть перемещена или удалена.
            this.городаTableAdapter.Fill(this.vUZBDDataSet.Города);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "vUZBDDataSet.ВУЗы". При необходимости она может быть перемещена или удалена.
            this.вУЗыTableAdapter.Fill(this.vUZBDDataSet.ВУЗы);           
            //автоподгон ширины
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                string query = "select ID_города,Название_города from Города"; 
                command.CommandText = query;
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    POKAZ.Items.Add(reader[0].ToString() + "       "+ reader[1].ToString());
                }
                POKAZ.Sorted = true;
                connection.Close();

            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка!" + ex); }
        }

        private void наГлавнуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Закрыть приложение?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

       
        //кнопка удаления данных
        private void DELBUT_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                if (CODEVUZ.Text != "")
                {
                    if (MessageBox.Show("Удалить выбранные данные?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string query = "DELETE FROM ВУЗы WHERE Код_ВУЗа=" + IDCITY.Text + "";
                        command.CommandText = query;
                    }
                }
                else
                    MessageBox.Show("Поле 'Код ВУЗа' не должно быть пустыми");
                command.ExecuteNonQuery();
                MessageBox.Show("Успешное удаление данных");
                this.вУЗыTableAdapter.Fill(this.vUZBDDataSet.ВУЗы);
                connection.Close();


            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка!" + ex); }
        }

        //показ панели добавления/изменения/удаления
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Visible = true;
        }

        private void добавить2ВариантToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SAVE save = new SAVE();
            save.Show();
        }

        //добавить
        private void ADDBUT_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                if (CODEVUZ.Text != "" && NameVUZ.Text != "" &&  IDCITY.Text!= "")
                {
                    command.CommandText = "INSERT INTO ВУЗы (Код_ВУЗа,Наименование_ВУЗа,ID_города,Юридический_адрес,Номер_телефона,Веб_сайт,Признак) VALUES('" + CODEVUZ.Text + 
                        "','" + NameVUZ.Text + "','" + IDCITY.Text + "','" + OfficialAdress.Text + "','" + Number.Text + "','"+SiteVUZ.Text + "','" + priznak.Text + "')";
                    
                    command.ExecuteNonQuery();
                    MessageBox.Show("Успешное сохранение");
                    this.вУЗыTableAdapter.Fill(this.vUZBDDataSet.ВУЗы);
                    connection.Close();

                }
                else
                    MessageBox.Show("Поля 'Код ВУЗа', 'Наименование ВУЗа', 'ID города' не должны быть пустыми");
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка! \n" + ex); }
        }

        private void ClearBUT_Click(object sender, EventArgs e)
        {
            CODEVUZ.Clear();
            NameVUZ.Clear();
            IDCITY.Clear();
            OfficialAdress.Clear();
            Number.Clear();
            SiteVUZ.Clear();
            priznak.Clear();
        }

      

        private void UPDATEBUT_Click(object sender, EventArgs e)
        {
            connection.Open();
            this.вУЗыTableAdapter.Fill(this.vUZBDDataSet.ВУЗы);
            connection.Close();
        }

        private void ChangeBUT_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                if (CODEVUZ.Text != "" && NameVUZ.Text != "" && IDCITY.Text != "")
                {
                    if (MessageBox.Show("Изменить данную строку?", "Замена данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string query = "UPDATE ВУЗы  SET Наименование_ВУЗа='" + NameVUZ.Text + "' ,ID_города='" + IDCITY.Text +"' ,Юридический_адрес ='"+ OfficialAdress .Text+
                            "' ,Номер_телефона ='"+ Number.Text + "' ,Веб_сайт='" + SiteVUZ.Text + "' ,Признак='" + priznak.Text + "' WHERE Код_ВУЗа="+ CODEVUZ.Text+"";

                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        MessageBox.Show("Успешное изменение данных");
                        this.вУЗыTableAdapter.Fill(this.vUZBDDataSet.ВУЗы);
                        connection.Close();
                    }
                }
                else
                    MessageBox.Show("Поля 'Код_ВУЗа', 'Наименование_ВУЗа', 'ID города' не должны быть пустыми");
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка!" + ex); }
        }

        private void SEARCHBUT_Click(object sender, EventArgs e)
        {
            switch(comboBox1.Text)
            {
                case "Код ВУЗа":
                    вУЗыBindingSource.Filter = "Код_ВУЗа= \'" + grayTXT1.Text + "\'";
                    break;
                case "Наименование ВУЗа":
                    вУЗыBindingSource.Filter = "Наименование_ВУЗа= \'" + grayTXT1.Text + "\'";
                    break;
                case "ID города":
                    вУЗыBindingSource.Filter = "ID_города= \'" + grayTXT1.Text + "\'";
                    break;
                case "Юридический адрес":
                    вУЗыBindingSource.Filter = "Юридический_адрес= \'" + grayTXT1.Text + "\'";
                    break;
                case "Номер телефона":
                    вУЗыBindingSource.Filter = "Номер_телефона= \'" + grayTXT1.Text + "\'";
                    break;
                case "Веб-сайт":
                    вУЗыBindingSource.Filter = "Веб_сайт= \'" + grayTXT1.Text + "\'";
                    break;
                case "Признак":
                    вУЗыBindingSource.Filter = "Признак= \'" + grayTXT1.Text + "\'";
                    break;

            }
        }
    }
}
