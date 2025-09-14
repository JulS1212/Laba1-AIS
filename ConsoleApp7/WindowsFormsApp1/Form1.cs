using BusinessLogical;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Logic Logic { get; set; }
        private Painting selectedPainting;
        public Form1()
        {
            InitializeComponent();
            Logic = new Logic();
            listBox1.Format += (sender, e) =>
            {
                if (e.ListItem is Painting painting)
                {
                    e.Value = $"{painting.Title} - {painting.Artist} ({painting.Year}), {painting.Genre}";
                }
            };
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            RefreshList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                    string.IsNullOrWhiteSpace(textBox2.Text) ||
                    string.IsNullOrWhiteSpace(textBox3.Text) ||
                    string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (int.TryParse(textBox3.Text, out int year))
                {
                    string title = textBox1.Text.Trim();
                    string artist = textBox2.Text.Trim();
                    string genre = textBox4.Text.Trim();

                    // Проверяем, существует ли такая картина
                    if (Logic.PaintingExists(title, artist))
                    {
                        MessageBox.Show("Такая картина уже существует!\nВведите другую картину.",
                            "Дубликат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Прерываем выполнение
                    }

                    Logic.AddPainting(title, artist, year, genre);
                    RefreshList();
                    ClearFields();
                    MessageBox.Show("Картина добавлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Введите корректный год!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                var painting = (Painting)listBox1.SelectedItem;
                if (Logic.DeletePainting(painting.Title))
                {
                    RefreshList();
                    ClearFields();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Проверяем, что картина выбрана в ListBox
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Сначала выберите картину из списка!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем выбранную картину непосредственно из ListBox
            Painting selectedPainting = (Painting)listBox1.SelectedItem;

            // Проверяем, что все поля заполнены
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (int.TryParse(textBox3.Text, out int year))
                {
                    // Выполняем обновление
                    bool success = Logic.UpdatePainting(
                        selectedPainting.Title, // старое название (для поиска)
                        textBox1.Text,          // новое название
                        textBox2.Text,          // новый художник
                        year,                   // новый год
                        textBox4.Text           // новый жанр
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
                else
                {
                    MessageBox.Show("Введите корректный год!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null && listBox1.SelectedItem is Painting)
            {
                selectedPainting = (Painting)listBox1.SelectedItem;

                // Заполняем текстовые поля данными выбранной картины
                textBox1.Text = selectedPainting.Title;
                textBox2.Text = selectedPainting.Artist;
                textBox3.Text = selectedPainting.Year.ToString();
                textBox4.Text = selectedPainting.Genre;
            }
            else
            {
                selectedPainting = null;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Бизнес-функция 2: Поиск по диапазону лет
            if (int.TryParse(textBox5.Text, out int startYear) &&
                int.TryParse(textBox6.Text, out int endYear))
            {
                var paintings = Logic.GetPaintingsByYearRange(startYear, endYear);
                string result = $"Картины с {startYear} по {endYear} год:\n";
                foreach (var painting in paintings)
                {
                    result += $"{painting.Title} - {painting.Artist} ({painting.Year}), {painting.Genre}\n";
                }
                MessageBox.Show(result);
            }
        }

        

        private void listBoxPaintings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                selectedPainting = (Painting)listBox1.SelectedItem;
                textBox1.Text = selectedPainting.Title;
                textBox2.Text = selectedPainting.Artist;
                textBox3.Text = selectedPainting.Year.ToString();
                textBox4.Text = selectedPainting.Genre;
            }
        }

        private void RefreshList()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = Logic.GetAllPaintings();
            listBox1.DisplayMember = null;
        }

        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            selectedPainting = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var groupedPaintings = Logic.GroupByGenre();

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
    }
}
