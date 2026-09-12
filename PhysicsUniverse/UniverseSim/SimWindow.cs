using System.Windows.Forms;

namespace UniverseSim
{
    public class SimWindow : Form
    {
        private Renderer _renderer;
        public SimWindow(string title, int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Text = title;

            var hwnd = this.Handle;

            _renderer = new Renderer();
            _renderer.Initialize(hwnd, width, height);
        }

        public void RenderFrame()
        {
            _renderer.Render();
        }
    }
}
