namespace ChatGPTWinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtPrompt = new TextBox();
            txtChat = new TextBox();
            lblTitulo = new Label();
            lblEstado = new Label();
            btnLimpiar = new Button();
            btnEnviar = new Button();
            SuspendLayout();
            // 
            // txtPrompt
            // 
            txtPrompt.Cursor = Cursors.IBeam;
            txtPrompt.Font = new Font("Share Tech Mono", 8.999999F);
            txtPrompt.Location = new Point(91, 416);
            txtPrompt.MaxLength = 1000;
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.Size = new Size(653, 84);
            txtPrompt.TabIndex = 0;
            txtPrompt.TextChanged += txtPrompt_TextChanged;
            // 
            // txtChat
            // 
            txtChat.BackColor = Color.FromArgb(12, 26, 50);
            txtChat.Font = new Font("Share Tech Mono", 8.999999F);
            txtChat.ForeColor = Color.White;
            txtChat.Location = new Point(91, 31);
            txtChat.Multiline = true;
            txtChat.Name = "txtChat";
            txtChat.ReadOnly = true;
            txtChat.ScrollBars = ScrollBars.Vertical;
            txtChat.Size = new Size(733, 379);
            txtChat.TabIndex = 1;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Share Tech Mono", 8.999999F);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(399, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(112, 17);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "ChatGPT Forms";
            // 
            // lblEstado
            // 
            lblEstado.Font = new Font("Share Tech Mono", 8.999999F);
            lblEstado.ForeColor = Color.White;
            lblEstado.Location = new Point(91, 503);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(733, 28);
            lblEstado.TabIndex = 3;
            lblEstado.TextAlign = ContentAlignment.TopRight;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.FromArgb(105, 147, 255);
            btnLimpiar.FlatStyle = FlatStyle.System;
            btnLimpiar.Font = new Font("Share Tech Mono", 8.999999F);
            btnLimpiar.Location = new Point(750, 461);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(74, 39);
            btnLimpiar.TabIndex = 4;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnEnviar
            // 
            btnEnviar.BackColor = SystemColors.ButtonHighlight;
            btnEnviar.FlatAppearance.BorderColor = Color.FromArgb(192, 255, 255);
            btnEnviar.FlatAppearance.BorderSize = 10;
            btnEnviar.FlatStyle = FlatStyle.System;
            btnEnviar.Font = new Font("Share Tech Mono", 8.999999F);
            btnEnviar.Location = new Point(750, 416);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(74, 39);
            btnEnviar.TabIndex = 5;
            btnEnviar.Text = "Enviar";
            btnEnviar.UseVisualStyleBackColor = false;
            btnEnviar.Click += btnEnviar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(26, 44, 74);
            ClientSize = new Size(870, 540);
            Controls.Add(btnEnviar);
            Controls.Add(btnLimpiar);
            Controls.Add(lblEstado);
            Controls.Add(lblTitulo);
            Controls.Add(txtChat);
            Controls.Add(txtPrompt);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtPrompt;
        private TextBox txtChat;
        private Label lblTitulo;
        private Label lblEstado;
        private Button btnLimpiar;
        private Button btnEnviar;
    }
}
