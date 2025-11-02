using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Lab678.Forms
{
    partial class RepairWorkForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 500);
            this.Text = "RepairWorkForm";
            this.ResumeLayout(false);
        }
    }
}