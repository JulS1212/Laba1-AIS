using BusinessLogical;
using BusinessLogical.Interfaces;
using Model;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogical.Services;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public IPaintingService PaintingService { get; set; }
        public IPaintingValidator PaintingValidator { get; set; }
        private Painting selectedPainting;
        public Form1()
        {
            InitializeComponent();
            IKernel ninjectKernel = new StandardKernel(new NinjectConfig());
            PaintingService = ninjectKernel.Get<IPaintingService>();
            PaintingValidator = ninjectKernel.Get<IPaintingValidator>();

            // Новые подписки для кнопок сортировки
            sort1.Click += sort1_Click;
            sort2.Click += button8_Click;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            RefreshList();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(textBox1.Text) || //проверка что не пустые
        //            string.IsNullOrWhiteSpace(textBox2.Text) ||
        //            string.IsNullOrWhiteSpace(textBox3.Text) ||
        //            string.IsNullOrWhiteSpace(textBox4.Text))
        //        {
        //            MessageBox.Show("Заполните все поля!", "Ошибка",
        //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        if (int.TryParse(textBox3.Text, out int year))
        //        {
        //            string title = textBox1.Text.Trim(); //убираем пробелы
        //            string artist = textBox2.Text.Trim();
        //            string genre = textBox4.Text.Trim();

        //            // Проверяем, существует ли такая картина
        //            if (PaintingService.PaintingExists(title, artist))
        //            {
        //                MessageBox.Show("Такая картина уже существует!\nВведите другую картину.",
        //                    "Дубликат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return; // Прерываем выполнение
        //            }

        //            PaintingService.AddPainting(title, artist, year, genre);
        //            RefreshList();
        //            ClearFields();
        //            MessageBox.Show("Картина добавлена!", "Успех",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Введите корректный год!", "Ошибка",
        //                MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Ошибка",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                // БЫЛА сложная валидация в коде:
                // if (int.TryParse(textBox3.Text, out int year)) {...}

                // СТАЛА простая валидация через валидатор:
                string title = textBox1.Text.Trim();
                string artist = textBox2.Text.Trim();
                string genre = textBox4.Text.Trim();

                if (!int.TryParse(textBox3.Text, out int year))
                {
                    MessageBox.Show("Введите корректный год!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ИСПОЛЬЗУЕМ ВАЛИДАТОР:
                string validationMessage = PaintingValidator.ValidateWithMessage(title, artist, year, genre);
                if (validationMessage != null)
                {
                    MessageBox.Show(validationMessage, "Ошибка валидации",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверяем уникальность через сервис:
                if (PaintingService.PaintingExists(title, artist))
                {
                    MessageBox.Show("Такая картина уже существует!", "Дубликат",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Добавляем через сервис:
                PaintingService.AddPainting(title, artist, year, genre);
                RefreshList();
                ClearFields();
                MessageBox.Show("Картина добавлена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var painting = (Painting)dataGridView1.SelectedRows[0].DataBoundItem;
                if (PaintingService.DeletePainting(painting.Title, painting.Artist))
                {
                    RefreshList();
                    ClearFields();
                }
            }
        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    // Проверяем, что картина выбрана в ListBox
        //    if (dataGridView1.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("Сначала выберите картину из списка!", "Ошибка",
        //            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // Получаем выбранную картину непосредственно из ListBox
        //    Painting selectedPainting = (Painting)dataGridView1.SelectedRows[0].DataBoundItem;

        //    // Проверяем, что все поля заполнены
        //    if (string.IsNullOrWhiteSpace(textBox1.Text) ||
        //        string.IsNullOrWhiteSpace(textBox2.Text) ||
        //        string.IsNullOrWhiteSpace(textBox3.Text) ||
        //        string.IsNullOrWhiteSpace(textBox4.Text))
        //    {
        //        MessageBox.Show("Заполните все поля!", "Ошибка",
        //            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    try
        //    {
        //        if (int.TryParse(textBox3.Text, out int year))
        //        {
        //            // Выполняем обновление
        //            bool success = PaintingService.UpdatePainting(
        //                selectedPainting.Title, // старое название (для поиска)
        //                selectedPainting.Artist,
        //                textBox1.Text,          // новое название
        //                textBox2.Text,          // новый художник
        //                year,                   // новый год
        //                textBox4.Text           // новый жанр
        //            );

        //            if (success)
        //            {
        //                RefreshList();
        //                ClearFields();
        //                MessageBox.Show("Картина успешно обновлена!", "Успех",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Не удалось обновить картину!", "Ошибка",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Введите корректный год!", "Ошибка",
        //                MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedPainting = (Painting)dataGridView1.SelectedRows[0].DataBoundItem;
                textBox1.Text = selectedPainting.Title;
                textBox2.Text = selectedPainting.Artist;
                textBox3.Text = selectedPainting.Year.ToString();
                textBox4.Text = selectedPainting.Genre;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Бизнес-функция 2: Поиск по диапазону лет
            if (int.TryParse(textBox5.Text, out int startYear) &&
                int.TryParse(textBox6.Text, out int endYear))
            {
                var paintings = PaintingService.GetPaintingsByYearRange(startYear, endYear);
                string result = $"Картины с {startYear} по {endYear} год:\n";
                foreach (var painting in paintings)
                {
                    result += $"{painting.Title} - {painting.Artist} ({painting.Year}), {painting.Genre}\n";
                }
                MessageBox.Show(result);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Сначала выберите картину из списка!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Painting selectedPainting = (Painting)dataGridView1.SelectedRows[0].DataBoundItem;

            try
            {
                // Получаем данные из полей
                string newTitle = textBox1.Text.Trim();
                string newArtist = textBox2.Text.Trim();
                string newGenre = textBox4.Text.Trim();

                if (!int.TryParse(textBox3.Text, out int newYear))
                {
                    MessageBox.Show("Введите корректный год!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ВАЛИДАТОР проверяет ВСЕ поля (включая пустые):
                string validationMessage = PaintingValidator.ValidateWithMessage(newTitle, newArtist, newYear, newGenre);
                if (validationMessage != null)
                {
                    MessageBox.Show(validationMessage, "Ошибка валидации",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Вся бизнес-логика теперь в сервисе:
                bool success = PaintingService.UpdatePainting(
                    selectedPainting.Title,
                    selectedPainting.Artist,
                    newTitle,
                    newArtist,
                    newYear,
                    newGenre
                );

                if (success)
                {
                    RefreshList();
                    ClearFields();
                    MessageBox.Show("Картина успешно обновлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось обновить картину!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //private void listBoxPaintings_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (dataGridView1.SelectedRows.Count > 0)
        //    {
        //        selectedPainting = (Painting)dataGridView1.SelectedItem;
        //        textBox1.Text = selectedPainting.Title;
        //        textBox2.Text = selectedPainting.Artist;
        //        textBox3.Text = selectedPainting.Year.ToString();
        //        textBox4.Text = selectedPainting.Genre;
        //    }
        //}

        private void RefreshList()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = PaintingService.GetAllPaintings();

            // Опционально: настрой заголовки колонок
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["Id"].Visible = false; // Скрыть ID если не нужен
                dataGridView1.Columns["Title"].HeaderText = "Название";
                dataGridView1.Columns["Artist"].HeaderText = "Автор";
                dataGridView1.Columns["Year"].HeaderText = "Год";
                dataGridView1.Columns["Genre"].HeaderText = "Жанр";
            }
        }

        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            selectedPainting = null;
        }
        // Сортировка по алфавиту (А-Я) - для buttonSort1
       

        // Сортировка в обратном алфавитном порядке (Я-А) - для buttonSort2
      
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var groupedPaintings = PaintingService.GroupByGenre();

                if (groupedPaintings.Count == 0)
                {
                    MessageBox.Show("Нет картин для группировки!", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Создаем строку с результатами
                StringBuilder result = new StringBuilder();
                result.AppendLine("=== КАРТИНЫ ПО ЖАНРАМ ===");
                result.AppendLine();

                foreach (var genreGroup in groupedPaintings)
                {
                    result.AppendLine($"{genreGroup.Key.ToUpper()} ({genreGroup.Value.Count} картин):");

                    foreach (var painting in genreGroup.Value)
                    {
                        result.AppendLine($"   • {painting.Title} - {painting.Artist} ({painting.Year})");
                    }
                    result.AppendLine();
                }

                // Показываем результаты в MessageBox
                MessageBox.Show(result.ToString(), "Группировка по жанрам",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при группировке: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClearFields();
        }


        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = PaintingService.SortByTitleDescending();
                MessageBox.Show("Сортировка в обратном порядке выполнена", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сортировке: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;

            RefreshList();

        }

        private void sort1_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = PaintingService.SortByTitleAscending();
                MessageBox.Show("Сортировка по алфавиту выполнена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сортировке: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
