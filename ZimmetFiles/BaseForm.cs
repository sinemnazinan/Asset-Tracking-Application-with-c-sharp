using System.Reflection;

namespace ZWebApp
{
    public class BaseForm : Form
    {
        public BaseForm()
        {
            SetIcon();
        }

        private void SetIcon()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream("ZWebApp.Bera_icon.ico"))
            {
                if (stream != null)
                    this.Icon = new Icon(stream);
            }
            this.StartPosition = FormStartPosition.CenterScreen;

        }
    }
}
