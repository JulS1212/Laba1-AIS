using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form, IView
    {
        // 1. РЕАЛИЗУЕМ СОБЫТИЯ IView
        public event Action FormLoaded;
        public event Action<string, string, int, string> AddPaintingRequested;
        public event Action<string, string> DeletePaintingRequested;
        public event Action<string, string, string, string, int, string> UpdatePaintingRequested;
        public event Action<int, int> SearchByYearRangeRequested;
        public event Action GroupByGenreRequested;
        public event Action SortByTitleAscendingRequested;
        public event Action SortByTitleDescendingRequested;
        public event Action ClearInputsRequested;
        public event Action<PaintingDto> PaintingSelected;


        public Form1()
        {
            InitializeComponent();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;

            // 2. ПРИВЯЗКА СОБЫТИЙ UI К НАШИМ IView СОБЫТИЯМ
            this.Load += (s, e) => FormLoaded?.Invoke();

            // Кнопка "Добавить картину"
            button1.Click += (s, e) =>
            {
                if (int.TryParse(textBox3.Text, out int year))
                    AddPaintingRequested?.Invoke(textBox1.Text, textBox2.Text, year, textBox4.Text);
            };

            // Кнопка "Удалить картину"
            button2.Click += (s, e) =>
                DeletePaintingRequested?.Invoke(textBox1.Text, textBox2.Text);

            // Кнопка "Изменить"
            button3.Click += (s, e) =>
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selected = (PaintingDto)dataGridView1.SelectedRows[0].DataBoundItem;
                    if (int.TryParse(textBox3.Text, out int newYear))
                        UpdatePaintingRequested?.Invoke(selected.Title, selected.Artist,
                                                       textBox1.Text, textBox2.Text,
                                                       newYear, textBox4.Text);
                }
            };

            // Кнопка "По жанрам"
            button4.Click += (s, e) => GroupByGenreRequested?.Invoke();

            // Кнопка "Найти" (по годам)
            button5.Click += (s, e) =>
            {
                if (int.TryParse(textBox5.Text, out int startYear) &&
                    int.TryParse(textBox6.Text, out int endYear))
                    SearchByYearRangeRequested?.Invoke(startYear, endYear);
            };

            // Кнопка "Очистить"
            button6.Click += (s, e) => ClearInputsRequested?.Invoke();

            // Кнопки сортировки
            sort1.Click += (s, e) => SortByTitleAscendingRequested?.Invoke();
            sort2.Click += (s, e) => SortByTitleDescendingRequested?.Invoke();

            // Выбор строки в таблице
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        // 3. РЕАЛИЗУЕМ МЕТОДЫ IView

        public void DisplayPaintings(List<PaintingDto> paintings)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = paintings;

            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["Id"].Visible = false;
                dataGridView1.Columns["Title"].HeaderText = "Название";
                dataGridView1.Columns["Artist"].HeaderText = "Автор";
                dataGridView1.Columns["Year"].HeaderText = "Год";
                dataGridView1.Columns["Genre"].HeaderText = "Жанр";
            }
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ClearInputs()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        public void SetSelectedPainting(PaintingDto painting)
        {
            textBox1.Text = painting.Title;
            textBox2.Text = painting.Artist;
            textBox3.Text = painting.Year.ToString();
            textBox4.Text = painting.Genre;
        }

        // 4. ОБРАБОТЧИКИ СОБЫТИЙ UI (оставляем только те, что не покрыты событиями IView)


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var paintingDto = (PaintingDto)dataGridView1.SelectedRows[0].DataBoundItem;
                PaintingSelected?.Invoke(paintingDto); // ← "Пользователь выбрал эту картину"
            }
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    dataGridView1.AutoGenerateColumns = true;
        //    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //    dataGridView1.MultiSelect = false;
        //    dataGridView1.ReadOnly = true;
        //}
    }
}