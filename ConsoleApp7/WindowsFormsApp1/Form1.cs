using Controllers;
using Shared;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form, IView
    {
        private PaintingController _controller;

        public Form1()
        {
          
            InitializeComponent();
            InitializeEvents();

          
        }
        public void SetController(PaintingController controller)
        {
            _controller = controller;// Сохраняем ссылку
            _controller.LoadPaintings();//// Загружаем данные в DataGridView
        }
        private void InitializeEvents()
        {
            // Загрузка данных при старте
            this.Load += (s, e) => _controller.LoadPaintings();

            // Кнопка добавления
            button1.Click += (s, e) =>
            {
                try
                {
                    _controller.AddPainting(textBox1.Text, textBox2.Text,
                                          int.Parse(textBox3.Text), textBox4.Text);
                }
                catch (FormatException)
                {
                    ShowError("Некорректный год!");
                }
            };
            //Кнопка удаления:

            button2.Click += (s, e) => _controller.DeletePainting(textBox1.Text, textBox2.Text);
            //Кнопка обновления:
            button3.Click += (s, e) =>
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    try
                    {
                        var selected = (PaintingDto)dataGridView1.SelectedRows[0].DataBoundItem;
                        _controller.UpdatePainting(selected.Title, selected.Artist,
                                                  textBox1.Text, textBox2.Text,
                                                  int.Parse(textBox3.Text), textBox4.Text);
                    }
                    catch (FormatException)
                    {
                        ShowError("Некорректный год!");
                    }
                }
            };

            button4.Click += (s, e) => _controller.GroupByGenre();

            button5.Click += (s, e) =>
            {
                if (int.TryParse(textBox5.Text, out int startYear) &&
                    int.TryParse(textBox6.Text, out int endYear))
                {
                    _controller.SearchByYearRange(startYear, endYear);
                }
                else
                {
                    ShowError("Некорректные годы!");
                }
            };

            button6.Click += (s, e) => _controller.ClearInputs();
            sort1.Click += (s, e) => _controller.SortByTitleAscending();
            sort2.Click += (s, e) => _controller.SortByTitleDescending();

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        // Реализация IView интерфейса
        public void DisplayPaintings(List<PaintingDto> paintings)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<List<PaintingDto>>(DisplayPaintings), paintings);
                return;
            }

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
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowError), message);
                return;
            }
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowMessage), message);
                return;
            }
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var paintingDto = (PaintingDto)dataGridView1.SelectedRows[0].DataBoundItem;
                textBox1.Text = paintingDto.Title;
                textBox2.Text = paintingDto.Artist;
                textBox3.Text = paintingDto.Year.ToString();
                textBox4.Text = paintingDto.Genre;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
           
            base.OnFormClosing(e);
        }
    }
}