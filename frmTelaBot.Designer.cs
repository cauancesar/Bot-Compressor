namespace CompressãoImagemBoy
{
    partial class frmTelaBot
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTelaBot));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnEstadoBot = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::CompressãoImagemBot.Properties.Resources.telegram_icone_icon;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = global::CompressãoImagemBot.Properties.Resources.telegram_icone_icon;
            this.pictureBox1.Location = new System.Drawing.Point(373, 48);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(415, 380);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnEstadoBot
            // 
            this.btnEstadoBot.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnEstadoBot.Font = new System.Drawing.Font("Segoe Print", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstadoBot.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnEstadoBot.Location = new System.Drawing.Point(61, 212);
            this.btnEstadoBot.Name = "btnEstadoBot";
            this.btnEstadoBot.Size = new System.Drawing.Size(233, 69);
            this.btnEstadoBot.TabIndex = 1;
            this.btnEstadoBot.Text = "Desconectado";
            this.btnEstadoBot.UseVisualStyleBackColor = false;
            this.btnEstadoBot.Click += new System.EventHandler(this.btnEstadoBot_Click);
            // 
            // frmTelaBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnEstadoBot);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTelaBot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bot de Compressão de Imagem";
            this.Load += new System.EventHandler(this.frmTelaBot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnEstadoBot;
    }
}

