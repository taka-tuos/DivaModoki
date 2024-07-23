namespace DivaModoki
{
    public partial class Form_Launch : Form
    {
        public Form_Launch()
        {
            InitializeComponent();
        }

        private void button_Launch_Click(object sender, EventArgs e)
        {
            using (Game game = new Game(1280, 720, "LearnOpenTK")
            {
                VSync = checkBox_VSync.Checked ? OpenTK.Windowing.Common.VSyncMode.On : OpenTK.Windowing.Common.VSyncMode.Off,
                WindowBorder = checkBox_Borderless.Checked ? OpenTK.Windowing.Common.WindowBorder.Hidden : OpenTK.Windowing.Common.WindowBorder.Fixed
            })
            {
                game.Run();
            }

            this.Close();
        }
    }
}
