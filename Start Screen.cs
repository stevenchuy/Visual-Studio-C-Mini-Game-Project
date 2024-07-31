namespace MultiLevel_Game_Project_V._3
{
    public partial class Start_Screen : Form
    {
        public Start_Screen()
        {
            InitializeComponent();
            Background();
        }

        private void Background()
        {
            this.BackgroundImage = Image.FromFile("introBackground.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void OnClickEvent(object sender, EventArgs e)
        {
            Level_1 level1Window = new Level_1();
            level1Window.Show();
            this.Hide();
        }
    }
}
