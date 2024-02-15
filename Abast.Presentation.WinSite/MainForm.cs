using Abast.Business.Rules;
using Abast.Common.Model;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
[assembly: XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace Abast.Presentation.WinSite
{
    public partial class MainForm : Form
    {
        public StudentBR business { get; set; }

        [Log]
        public MainForm()
        {
            this.business = new StudentBR();
            XmlConfigurator.Configure();
            InitializeComponent();

            Dictionary<string, string> languages = new Dictionary<string, string>
                {
                  { "en","English"},
                  { "es","Spanish"}
                };
            this.cbLanguage.ComboBox.DataSource = languages.ToList();
            this.cbLanguage.ComboBox.DisplayMember = "Value";
            this.cbLanguage.ComboBox.ValueMember = "Key";

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            this.progressBar1.Value=0;

            Student student = new Student();
            student.Name = this.tbName.Text;
            student.Surname = this.tbSurname.Text;
            student.Birthday = this.dtPBirthday.Value;

            this.progressBar1.Value = 50;

            await this.business.Add(student);
            this.LoadStudents();

            this.progressBar1.Value = 100;
        }


        [Log]
        private void resetUITexts()
        {

            foreach (var item in this.Controls)
            {
                if (item is Label label)
                {
                    ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
                    resources.ApplyResources(label, label.Name.ToString());
                }
            }
            this.PerformLayout();

        }

        private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string languageCode = "en";
            switch (cbLanguage.SelectedIndex)
            {
                case 0:
                    languageCode = "en";
                    break;
                case 1:
                    languageCode = "es";
                    break;
            }
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(languageCode);

            this.resetUITexts();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            this.LoadStudents();
        }

        private async void LoadStudents()
        {  
            List<Student> students = await this.business.GetAll();
            this.dtStudents.DataSource = students;
        }
    }
}
