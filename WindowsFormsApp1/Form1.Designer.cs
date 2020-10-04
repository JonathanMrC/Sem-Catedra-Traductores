namespace WindowsFormsApp1
{
    partial class Form
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtborigen = new System.Windows.Forms.TextBox();
            this.BtnAnalizar = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.Tokens = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lexemas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // txtborigen
            // 
            this.txtborigen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtborigen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtborigen.Location = new System.Drawing.Point(13, 13);
            this.txtborigen.MaximumSize = new System.Drawing.Size(345, 127);
            this.txtborigen.Multiline = true;
            this.txtborigen.Name = "txtborigen";
            this.txtborigen.Size = new System.Drawing.Size(345, 127);
            this.txtborigen.TabIndex = 0;
            // 
            // BtnAnalizar
            // 
            this.BtnAnalizar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAnalizar.Location = new System.Drawing.Point(364, 55);
            this.BtnAnalizar.MaximumSize = new System.Drawing.Size(75, 47);
            this.BtnAnalizar.Name = "BtnAnalizar";
            this.BtnAnalizar.Size = new System.Drawing.Size(75, 47);
            this.BtnAnalizar.TabIndex = 2;
            this.BtnAnalizar.Text = "Analizar";
            this.BtnAnalizar.UseVisualStyleBackColor = true;
            this.BtnAnalizar.Click += new System.EventHandler(this.BtnAnalizar_Click);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Tokens,
            this.lexemas});
            this.dgv.Location = new System.Drawing.Point(12, 146);
            this.dgv.MaximumSize = new System.Drawing.Size(424, 492);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.Size = new System.Drawing.Size(424, 492);
            this.dgv.TabIndex = 3;
            // 
            // Tokens
            // 
            this.Tokens.HeaderText = "Tokens";
            this.Tokens.Name = "Tokens";
            this.Tokens.ReadOnly = true;
            this.Tokens.Width = 68;
            // 
            // lexemas
            // 
            this.lexemas.HeaderText = "Palabras";
            this.lexemas.Name = "lexemas";
            this.lexemas.ReadOnly = true;
            this.lexemas.Width = 73;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.BtnAnalizar);
            this.Controls.Add(this.txtborigen);
            this.Name = "Form";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtborigen;
        private System.Windows.Forms.Button BtnAnalizar;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tokens;
        private System.Windows.Forms.DataGridViewTextBoxColumn lexemas;
    }
}

